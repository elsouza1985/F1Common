﻿// <copyright file="CertificateProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Secrets
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using FC1_CC.Common.Services.CommonBot;
    using Azure;
    using Azure.Security.KeyVault.Certificates;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// This class implements ICertficateProvider, which is used to retrieve certificates.
    /// </summary>
    public class CertificateProvider : ICertificateProvider
    {
        private readonly Dictionary<string, string> certificateNameMap;
        private readonly bool useCertificate;
        private readonly CertificateClient certificateClient;
        private readonly ILogger<CertificateProvider> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateProvider"/> class.
        /// A constructor that accepts a map of bot id list and credentials.
        /// </summary>
        /// <param name="botOptions">bot options.</param>
        /// <param name="certificateClient">certificate client.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public CertificateProvider(
            IOptions<BotOptions> botOptions,
            CertificateClient certificateClient,
            ILoggerFactory loggerFactory)
        {
            botOptions = botOptions ?? throw new ArgumentNullException(nameof(botOptions));
            logger = loggerFactory?.CreateLogger<CertificateProvider>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            useCertificate = botOptions.Value.UseCertificate;
            this.certificateClient = certificateClient ?? throw new ArgumentNullException(nameof(certificateClient));
            if (useCertificate)
            {
                certificateNameMap = CreateCertificateNameMap(botOptions.Value);
            }
        }

        /// <inheritdoc/>
        public async Task<X509Certificate2> GetCertificateAsync(string appId)
        {
            appId = appId ?? throw new ArgumentNullException(nameof(appId));

            var certificateName = certificateNameMap.ContainsKey(appId) ? certificateNameMap[appId] : null;

            if (string.IsNullOrEmpty(certificateName))
            {
                throw new InvalidOperationException("Certificate name not found.");
            }

            try
            {
                var response = await certificateClient.DownloadCertificateAsync(certificateName);
                return response.Value;
            }
            catch (InvalidDataException exception)
            {
                logger.LogError(exception, $"Certificate not found. Cert name: {certificateName} ");
            }
            catch (RequestFailedException exception)
            {
                logger.LogError(exception, $"Failed to fetch certificate. ErrorCode: {exception.ErrorCode} Cert name: {certificateName}.");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"Failed to fetch certificate. Cert name: {certificateName}.");
            }

            throw new Exception($"Certificate not found. Cert name: {certificateName} ");
        }

        /// <inheritdoc/>
        public bool IsCertificateAuthenticationEnabled()
        {
            return useCertificate;
        }

        private Dictionary<string, string> CreateCertificateNameMap(BotOptions botOptions)
        {
            var certificateNameMap = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(botOptions.UserAppId))
            {
                throw new Exception("User app id not found.");
            }
            else
            {
                certificateNameMap.Add(botOptions.UserAppId, botOptions.UserAppCertName);
            }

            if (string.IsNullOrEmpty(botOptions.AuthorAppId))
            {
                throw new Exception("Author app id not found.");
            }
            else
            {
                certificateNameMap.Add(botOptions.AuthorAppId, botOptions.AuthorAppCertName);
            }

            if (string.IsNullOrEmpty(botOptions.GraphAppId))
            {
                throw new Exception("Graph app id not found.");
            }
            else
            {
                certificateNameMap.Add(botOptions.GraphAppId, botOptions.GraphAppCertName);
            }

            return certificateNameMap;
        }
    }
}
