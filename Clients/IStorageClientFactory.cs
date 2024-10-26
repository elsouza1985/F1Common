// <copyright file="IStorageClientFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Clients
{
    using Azure.Storage.Blobs;

    /// <summary>
    /// Storage client factory.
    /// </summary>
    public interface IStorageClientFactory
    {
        /// <summary>
        /// Create the blob container client instance.
        /// </summary>
        /// <returns>BlobContainerClient instance.</returns>
        BlobContainerClient CreateBlobContainerClient();

        /// <summary>
        /// Create the blob container client instance.
        /// </summary>
        /// <param name="blobContainerName">blob container name</param>
        /// <returns>BlobContainerClient instance.</returns>
        BlobContainerClient CreateBlobContainerClient(string blobContainerName);
    }
}
