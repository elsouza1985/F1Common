﻿// <copyright file="AppConfigRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Repositories.AppConfig
{
    using FC1_CC.Common.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// App configuration repository.
    /// </summary>
    public class AppConfigRepository : BaseRepository<AppConfigEntity>, IAppConfigRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppConfigRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AppConfigRepository(
            ILogger<AppConfigRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: AppConfigTableName.TableName,
                  defaultPartitionKey: AppConfigTableName.SettingsPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
