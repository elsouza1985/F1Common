﻿// <copyright file="DataQueueMessageContent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MessageQueues.DataQueue
{
    /// <summary>
    /// Azure service bus data queue message content class.
    /// </summary>
    public class DataQueueMessageContent
    {
        /// <summary>
        /// Gets or sets the notification id value.
        /// </summary>
        public string NotificationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data function should force the
        /// corresponding notification to be marked as complete.
        /// </summary>
        public bool ForceMessageComplete { get; set; }
    }
}
