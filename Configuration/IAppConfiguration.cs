// <copyright file="IAppConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Configuration
{
    /// <summary>
    /// App configuration interface.
    /// </summary>
    public interface IAppConfiguration
    {
        public string AzureAd_Instance { get; }

        public string AzureAd_ValidIssuers { get; }

        public string AuthorityUri { get; }

        public string GraphBaseUrl { get; }

        public string GraphDefaultScope { get; }

        public string GraphUserReadScope { get; }

        public string TeamsLicenseId { get; }
    }
}
