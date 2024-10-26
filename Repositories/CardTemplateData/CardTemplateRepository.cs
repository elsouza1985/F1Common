// <copyright file="AppConfigRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Repositories.CardTemplateData
{
    using FC1_CC.Common.Repositories;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// App configuration repository.
    /// </summary>
    public class CardTemplateRepository : BaseRepository<CardTemplateEntity>, ICardTemplateRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardTemplateRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public CardTemplateRepository(
            ILogger<CardTemplateRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: CardTemplateTableName.TableName,
                  defaultPartitionKey: CardTemplateTableName.SettingsPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
