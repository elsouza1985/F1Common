// <copyright file="GraphServiceFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MicrosoftGraph
{
    using System;
    using Microsoft.Graph;


    /// <summary>
    /// Graph Service Factory.
    /// </summary>
    public class GraphServiceFactory : IGraphServiceFactory
    {
        private readonly IGraphServiceClient serviceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceFactory"/> class.
        /// </summary>
        /// <param name="serviceClient">V1 Graph service client.</param>
        public GraphServiceFactory(
            IGraphServiceClient serviceClient)
        {
            this.serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }

        /// <inheritdoc/>
        public IUsersService GetUsersService()
        {
            return new UsersService(serviceClient);
        }

        /// <inheritdoc/>
        public IGroupsService GetGroupsService()
        {
            return new GroupsService(serviceClient);
        }

        /// <inheritdoc/>
        public IGroupMembersService GetGroupMembersService()
        {
            return new GroupMembersService(serviceClient);
        }

        /// <inheritdoc/>
        public IChatsService GetChatsService()
        {
            return new ChatsService(serviceClient, GetAppManagerService());
        }

        /// <inheritdoc/>
        public IAppManagerService GetAppManagerService()
        {
            return new AppManagerService(serviceClient);
        }

        /// <inheritdoc/>
        public IAppCatalogService GetAppCatalogService()
        {
            return new AppCatalogService(serviceClient);
        }
    }
}
