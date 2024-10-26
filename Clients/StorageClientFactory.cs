// <copyright file="StorageClientFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Clients
{
    using System;
    using FC1_CC.Common;
    using FC1_CC.Common.Repositories;
    using Azure.Core;
    using Azure.Storage.Blobs;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Storage client factory.
    /// </summary>
    public class StorageClientFactory : IStorageClientFactory
    {
        private readonly string storageConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageClientFactory"/> class.
        /// </summary>
        /// <param name="repositoryOptions">User data repository.</param>
        public StorageClientFactory(IOptions<RepositoryOptions> repositoryOptions)
        {
            storageConnectionString = repositoryOptions.Value.StorageAccountConnectionString;
        }

        /// <inheritdoc/>
        public BlobContainerClient CreateBlobContainerClient()
        {
            var options = new BlobClientOptions();

            // configure retries
            options.Retry.MaxRetries = 5; // default is 3
            options.Retry.Mode = RetryMode.Exponential; // default is fixed retry policy
            options.Retry.Delay = TimeSpan.FromSeconds(1); // default is 0.8s

            return new BlobContainerClient(
                storageConnectionString,
                Constants.BlobContainerName,
                options);
        }

        /// <inheritdoc/>
        public BlobContainerClient CreateBlobContainerClient(string blobContainerName)
        {
            var options = new BlobClientOptions();

            // configure retries
            options.Retry.MaxRetries = 5; // default is 3
            options.Retry.Mode = RetryMode.Exponential; // default is fixed retry policy
            options.Retry.Delay = TimeSpan.FromSeconds(1); // default is 0.8s

            return new BlobContainerClient(
                storageConnectionString,
                blobContainerName,
                options);
        }
    }
}
