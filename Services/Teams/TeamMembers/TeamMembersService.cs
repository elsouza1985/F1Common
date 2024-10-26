﻿// <copyright file="TeamMembersService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FC1_CC.Common.Adapter;
    using FC1_CC.Common.Extensions;
    using FC1_CC.Common.Repositories.UserData;
    using FC1_CC.Common.Services.CommonBot;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Teams member service.
    /// </summary>
    public class TeamMembersService : ITeamMembersService
    {
        private readonly ICCBotFrameworkHttpAdapter botAdapter;
        private readonly string userAppId;
        private readonly string authorAppId;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamMembersService"/> class.
        /// </summary>
        /// <param name="botAdapter">Bot adapter.</param>
        /// <param name="botOptions">Bot options.</param>
        public TeamMembersService(
            ICCBotFrameworkHttpAdapter botAdapter,
            IOptions<BotOptions> botOptions)
        {
            this.botAdapter = botAdapter ?? throw new ArgumentNullException(nameof(botAdapter));
            userAppId = botOptions?.Value?.UserAppId ?? throw new ArgumentNullException(nameof(botOptions));
            authorAppId = botOptions?.Value?.AuthorAppId ?? throw new ArgumentNullException(nameof(botOptions));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDataEntity>> GetUsersAsync(string teamId, string tenantId, string serviceUrl)
        {
            return await GetMembersAsync(teamId, tenantId, serviceUrl, userAppId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDataEntity>> GetAuthorsAsync(string teamId, string tenantId, string serviceUrl)
        {
            return await GetMembersAsync(teamId, tenantId, serviceUrl, authorAppId);
        }

        private async Task<IEnumerable<UserDataEntity>> GetMembersAsync(string teamId, string tenantId, string serviceUrl, string appId)
        {
            var conversationReference = new ConversationReference
            {
                ServiceUrl = serviceUrl,
                Conversation = new ConversationAccount
                {
                    Id = teamId,
                },
            };

            IEnumerable<UserDataEntity> userDataEntitiesResult = null;

            await botAdapter.ContinueConversationAsync(
                appId,
                conversationReference,
                async (turnContext, cancellationToken) =>
                {
                    var members = await GetMembersAsync(turnContext, cancellationToken);

                    userDataEntitiesResult = members.Select(member =>
                    {
                        var userDataEntity = new UserDataEntity
                        {
                            UserId = member.Id,

                            // Set the conversation ID to null because it is not known at this time and
                            // may not have been created yet.
                            ConversationId = null,
                            ServiceUrl = serviceUrl,
                            AadId = member.AadObjectId,
                            TenantId = tenantId,
                            UserType = member.UserPrincipalName.GetUserType(),
                        };

                        return userDataEntity;
                    });
                },
                CancellationToken.None);

            return userDataEntitiesResult;
        }

        /// <summary>
        /// Fetches the roster with the new paginated calls to handles larger teams.
        /// https://docs.microsoft.com/en-us/microsoftteams/platform/bots/how-to/get-teams-context?tabs=dotnet#fetching-the-roster-or-user-profile.
        /// </summary>
        /// <param name="turnContext">The context object for this turn.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects.</param>
        /// <returns>The roster fetched by calling the new paginated SDK API.</returns>
        private async Task<IEnumerable<TeamsChannelAccount>> GetMembersAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken)
        {
            var members = new List<TeamsChannelAccount>();
            string continuationToken = null;
            const int pageSize = 500;

            do
            {
                var currentPage = await TeamsInfo.GetPagedMembersAsync(
                    turnContext,
                    pageSize,
                    continuationToken,
                    cancellationToken);
                continuationToken = currentPage.ContinuationToken;
                members.AddRange(currentPage.Members);
            }
            while (continuationToken != null && !cancellationToken.IsCancellationRequested);

            return members;
        }
    }
}
