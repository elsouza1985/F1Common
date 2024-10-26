﻿// <copyright file="Cache.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Secrets
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure;

    /// <summary>
    /// Maintains a cache of <see cref="CachedResponse"/> items.
    ///
    /// Source: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/keyvault/samples/keyvaultproxy/src.
    /// </summary>
    internal class Cache : IDisposable
    {
        private readonly Dictionary<string, CachedResponse> cache = new Dictionary<string, CachedResponse>(StringComparer.OrdinalIgnoreCase);
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        /// <inheritdoc/>
        public void Dispose()
        {
            if (semaphore is { })
            {
                semaphore.Dispose();
                semaphore = null;
            }
        }

        /// <summary>
        /// Gets a valid <see cref="Response"/> or requests and caches a <see cref="CachedResponse"/>.
        /// </summary>
        /// <param name="isAsync">Whether certain operations should be performed asynchronously.</param>
        /// <param name="uri">The URI sans query parameters to cache.</param>
        /// <param name="ttl">The amount of time for which the cached item is valid.</param>
        /// <param name="action">The action to request a response.</param>
        /// <returns>A new <see cref="Response"/>.</returns>
        internal async ValueTask<Response> GetOrAddAsync(bool isAsync, string uri, TimeSpan ttl, Func<ValueTask<Response>> action)
        {
            ThrowIfDisposed();

            if (isAsync)
            {
                await semaphore!.WaitAsync().ConfigureAwait(false);
            }
            else
            {
                semaphore!.Wait();
            }

            try
            {
                // Try to get a valid cached response inside the lock before fetching.
                if (cache.TryGetValue(uri, out CachedResponse cachedResponse) && cachedResponse.IsValid)
                {
                    return await cachedResponse.CloneAsync(isAsync).ConfigureAwait(false);
                }

                Response response = await action().ConfigureAwait(false);
                if (response.Status == 200 && response.ContentStream is { })
                {
                    cachedResponse = await CachedResponse.CreateAsync(isAsync, response, ttl).ConfigureAwait(false);
                    cache[uri] = cachedResponse;
                }

                return response;
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        internal void Clear()
        {
            ThrowIfDisposed();

            semaphore!.Wait();
            try
            {
                cache.Clear();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void ThrowIfDisposed()
        {
            if (semaphore is null)
            {
                throw new ObjectDisposedException(nameof(semaphore));
            }
        }
    }
}