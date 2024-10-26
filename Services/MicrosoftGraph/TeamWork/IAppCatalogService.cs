﻿// <copyright file="IAppCatalogService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MicrosoftGraph
{
    using System.Threading.Tasks;

    /// <summary>
    /// Read information about the apps published in the Teams app store and organization's app catalog.
    /// </summary>
    public interface IAppCatalogService
    {
        /// <summary>
        /// Gets teamsAppId from externalId of an app in a tenant's app catalog.
        ///
        /// ExternalId is provided in the app manifest and its unique for all the apps in a tenant's app catalog.
        /// </summary>
        /// <param name="externalId">Teams app's external id.</param>
        /// <returns>teamsAppId.</returns>
        public Task<string> GetTeamsAppIdAsync(string externalId);
    }
}
