// <copyright file="PrepareToSendQueueMessageContent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MessageQueues.PrepareToSendQueue
{
    /// <summary>
    /// Azure service bus prepare to send queue message content class.
    /// </summary>
    public class PrepareToSendQueueMessageContent
    {
        /// <summary>
        /// Gets or sets the notification id value.
        /// </summary>
        public string NotificationId { get; set; }
    }
}
