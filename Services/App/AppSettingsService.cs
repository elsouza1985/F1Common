// <copyright file="AppSettingsService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services
{
    using System;
    using System.Threading.Tasks;
    using FC1_CC.Common.Repositories.AppConfig;

    /// <summary>
    /// App settings service implementation.
    /// </summary>
    public class AppSettingsService : IAppSettingsService
    {
        private readonly IAppConfigRepository repository;

        private string serviceUrl;
        private string userAppId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsService"/> class.
        /// </summary>
        /// <param name="repository">App configuration repository.</param>
        public AppSettingsService(IAppConfigRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc/>
        public async Task<string> GetServiceUrlAsync()
        {
            // check in-memory cache.
            if (!string.IsNullOrWhiteSpace(serviceUrl))
            {
                return serviceUrl;
            }

            var appConfig = await repository.GetAsync(
                AppConfigTableName.SettingsPartition,
                AppConfigTableName.ServiceUrlRowKey);

            serviceUrl = appConfig?.Value;
            return serviceUrl;
        }

        /// <inheritdoc/>
        public async Task<string> GetUserAppIdAsync()
        {
            // check in-memory cache.
            if (!string.IsNullOrWhiteSpace(userAppId))
            {
                return userAppId;
            }

            var appConfig = await repository.GetAsync(
                AppConfigTableName.SettingsPartition,
                AppConfigTableName.UserAppIdRowKey);

            userAppId = appConfig?.Value;
            return userAppId;
        }

        /// <inheritdoc/>
        public async Task SetServiceUrlAsync(string serviceUrl)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentNullException(nameof(serviceUrl));
            }

            var appConfig = new AppConfigEntity()
            {
                PartitionKey = AppConfigTableName.SettingsPartition,
                RowKey = AppConfigTableName.ServiceUrlRowKey,
                Value = serviceUrl,
            };

            await repository.InsertOrMergeAsync(appConfig);

            // Update in-memory cache.
            this.serviceUrl = serviceUrl;
        }

        /// <inheritdoc/>
        public async Task SetUserAppIdAsync(string userAppId)
        {
            if (string.IsNullOrWhiteSpace(userAppId))
            {
                throw new ArgumentNullException(nameof(userAppId));
            }

            var appConfig = new AppConfigEntity()
            {
                PartitionKey = AppConfigTableName.SettingsPartition,
                RowKey = AppConfigTableName.UserAppIdRowKey,
                Value = userAppId,
            };

            await repository.InsertOrMergeAsync(appConfig);

            // Update in-memory cache.
            this.userAppId = userAppId;
        }

        /// <inheritdoc/>
        public async Task DeleteUserAppIdAsync()
        {
            var appId = await GetUserAppIdAsync();
            if (string.IsNullOrEmpty(appId))
            {
                // User App id isn't cached.
                return;
            }

            var appConfig = new AppConfigEntity()
            {
                PartitionKey = AppConfigTableName.SettingsPartition,
                RowKey = AppConfigTableName.UserAppIdRowKey,
            };

            await repository.DeleteAsync(appConfig);

            // Clear in-memory cache.
            userAppId = null;
        }
    }
}
