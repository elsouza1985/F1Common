﻿// <copyright file="CCBotFrameworkHttpAdapter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Adapter
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FC1_CC.Common.Secrets;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// Bot framework http adapter instance.
    /// </summary>
    public class CCBotFrameworkHttpAdapter : BotFrameworkHttpAdapter, ICCBotFrameworkHttpAdapter
    {
        private readonly ICertificateProvider certificateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CCBotFrameworkHttpAdapter"/> class.
        /// </summary>
        /// <param name="credentialProvider">credential provider.</param>
        /// <param name="certificateProvider">certificate provider.</param>
        public CCBotFrameworkHttpAdapter(
            ICredentialProvider credentialProvider,
            ICertificateProvider certificateProvider)
            : base(credentialProvider)
        {
            this.certificateProvider = certificateProvider;
        }

        /// <inheritdoc/>
        public async Task CreateConversationUsingCertificateAsync(string channelId, string serviceUrl, AppCredentials appCredentials, ConversationParameters conversationParameters, BotCallbackHandler callback, CancellationToken cancellationToken)
        {
            var cert = await certificateProvider.GetCertificateAsync(appCredentials.MicrosoftAppId);
            var options = new CertificateAppCredentialsOptions()
            {
                AppId = appCredentials.MicrosoftAppId,
                ClientCertificate = cert,
            };

            await CreateConversationAsync(channelId, serviceUrl, new CertificateAppCredentials(options) as AppCredentials, conversationParameters, callback, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CreateConversationUsingSecretAsync(string channelId, string serviceUrl, MicrosoftAppCredentials credentials, ConversationParameters conversationParameters, BotCallbackHandler callback, CancellationToken cancellationToken)
        {
            await CreateConversationAsync(channelId, serviceUrl, credentials, conversationParameters, callback, cancellationToken);
        }

        /// <inheritdoc/>
        protected override async Task<AppCredentials> BuildCredentialsAsync(string appId, string oAuthScope = null)
        {
            appId = appId ?? throw new ArgumentNullException(nameof(appId));

            if (certificateProvider.IsCertificateAuthenticationEnabled())
            {
                var cert = await certificateProvider.GetCertificateAsync(appId);
                var options = new CertificateAppCredentialsOptions()
                {
                    AppId = appId,
                    ClientCertificate = cert,
                    OauthScope = oAuthScope,
                };

                var certificateAppCredentials = new CertificateAppCredentials(options) as AppCredentials;
                return certificateAppCredentials;
            }
            else
            {
                return await base.BuildCredentialsAsync(appId, oAuthScope);
            }
        }
    }
}
