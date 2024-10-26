// <copyright file="GroupsService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MicrosoftGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FC1_CC.Common.Extensions;
    using Microsoft.Graph;

    /// <summary>
    /// Groups Service.
    /// </summary>
    internal class GroupsService : IGroupsService
    {
        private readonly IGraphServiceClient graphServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsService"/> class.
        /// </summary>
        /// <param name="graphServiceClient">graph service client.</param>
        internal GroupsService(IGraphServiceClient graphServiceClient)
        {
            this.graphServiceClient = graphServiceClient ?? throw new ArgumentNullException(nameof(graphServiceClient));
        }

        private int MaxResultCount { get; set; } = 25;

        private int MaxRetry { get; set; } = 2;

        /// <summary>
        /// get groups by ids.
        /// </summary>
        /// <param name="groupIds">list of group ids.</param>
        /// <returns>list of groups.</returns>
        public async IAsyncEnumerable<Group> GetByIdsAsync(IEnumerable<string> groupIds)
        {
            foreach (var id in groupIds)
            {
                var group = await graphServiceClient
                                .Groups[id]
                                .Request()
                                .WithMaxRetry(MaxRetry)
                                .Select(gr => new { gr.Id, gr.Mail, gr.DisplayName, gr.Visibility, })
                                .Header(FC1_CC.Common.Constants.PermissionTypeKey, GraphPermissionType.Delegate.ToString())
                                .GetAsync();
                yield return group;
            }
        }

        /// <summary>
        /// check if list has hidden membership group.
        /// </summary>
        /// <param name="groupIds">list of group ids.</param>
        /// <returns>boolean.</returns>
        public async Task<bool> ContainsHiddenMembershipAsync(IEnumerable<string> groupIds)
        {
            var groups = GetByIdsAsync(groupIds);
            await foreach (var group in groups)
            {
                if (group.IsHiddenMembership())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Search M365 groups,distribution groups, security groups based on query.
        /// </summary>
        /// <param name="query">query param.</param>
        /// <returns>list of group.</returns>
        public async Task<IList<Group>> SearchAsync(string query)
        {
            query = Uri.EscapeDataString(query);
            var groupList = await SearchM365GroupsAsync(query, MaxResultCount);
            groupList.AddRange(await AddDistributionGroupAsync(query, MaxResultCount - groupList.Count()));
            groupList.AddRange(await AddSecurityGroupAsync(query, MaxResultCount - groupList.Count()));
            return groupList;
        }

        /// <summary>
        /// Search M365 groups, distribution groups, security groups based on query and visibilty.
        /// </summary>
        /// <param name="query">query param.</param>
        /// <param name="resultCount">page size.</param>
        /// <param name="includeHiddenMembership">boolean to filter hidden membership.</param>
        /// <returns>list of group.</returns>
        private async Task<List<Group>> SearchM365GroupsAsync(string query, int resultCount, bool includeHiddenMembership = false)
        {
            string filterQuery = $"groupTypes/any(c:c+eq+'Unified') and mailEnabled eq true and (startsWith(mail,'{query}') or startsWith(displayName,'{query}'))";
            var groupsPaged = await SearchAsync(filterQuery, resultCount);
            if (includeHiddenMembership)
            {
                return groupsPaged.CurrentPage.ToList();
            }

            var groupList = groupsPaged.CurrentPage.
                                        Where(group => !group.IsHiddenMembership()).
                                        ToList();
            while (groupsPaged.NextPageRequest != null && groupList.Count() < resultCount)
            {
                groupsPaged = await groupsPaged.NextPageRequest.GetAsync();
                groupList.AddRange(groupsPaged.CurrentPage.
                          Where(group => !group.IsHiddenMembership()));
            }

            return groupList.Take(resultCount).ToList();
        }

        /// <summary>
        /// Search Distribution Groups based on query.
        /// </summary>
        /// <param name="query">query param.</param>
        /// <param name="resultCount">total page size.</param>
        /// <returns>list of distribution group.</returns>
        private async Task<IEnumerable<Group>> AddDistributionGroupAsync(string query, int resultCount)
        {
            if (resultCount == 0)
            {
                return new List<Group>();
            }

            string filterforDL = $"mailEnabled eq true and (startsWith(mail,'{query}') or startsWith(displayName,'{query}'))";
            var distributionGroups = await SearchAsync(filterforDL, resultCount);

            // Filtering the result only for distribution groups.
            var distributionGroupList = distributionGroups.CurrentPage.
                                                           Where(dg => dg.GroupTypes.IsNullOrEmpty()).ToList();
            while (distributionGroups.NextPageRequest != null && distributionGroupList.Count() < resultCount)
            {
                distributionGroups = await distributionGroups.NextPageRequest.GetAsync();
                distributionGroupList.AddRange(distributionGroups.CurrentPage.Where(dg => dg.GroupTypes.IsNullOrEmpty()));
            }

            return distributionGroupList.Take(resultCount);
        }

        /// <summary>
        /// Search Security Groups based on query.
        /// </summary>
        /// <param name="query">query param.</param>
        /// <param name="resultCount">total page size.</param>
        /// <returns>list of security group.</returns>
        private async Task<IEnumerable<Group>> AddSecurityGroupAsync(string query, int resultCount)
        {
            if (resultCount == 0)
            {
                return new List<Group>();
            }

            string filterforSG = $"mailEnabled eq false and securityEnabled eq true and startsWith(displayName,'{query}')";
            var sgGroups = await SearchAsync(filterforSG, resultCount);
            return sgGroups.CurrentPage.Take(resultCount);
        }

        /// <summary>
        /// Search M365 groups,sistribution groups, security groups based on query.
        /// </summary>
        /// <param name="filterQuery">query param.</param>
        /// <param name="resultCount">page size.</param>
        /// <returns>graph group collection.</returns>
        private async Task<IGraphServiceGroupsCollectionPage> SearchAsync(string filterQuery, int resultCount)
        {
            return await graphServiceClient
                                   .Groups
                                   .Request()
                                   .WithMaxRetry(MaxRetry)
                                   .Filter(filterQuery)
                                   .Select(group => new
                                   {
                                       group.Id,
                                       group.Mail,
                                       group.DisplayName,
                                       group.Visibility,
                                       group.GroupTypes,
                                   }).
                                   Top(resultCount)
                                   .Header(FC1_CC.Common.Constants.PermissionTypeKey, GraphPermissionType.Delegate.ToString())
                                   .GetAsync();
        }
    }
}
