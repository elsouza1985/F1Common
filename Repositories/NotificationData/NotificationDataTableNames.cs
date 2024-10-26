﻿// <copyright file="NotificationDataTableNames.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Repositories.NotificationData
{
    /// <summary>
    /// Notification data table names.
    /// </summary>
    public static class NotificationDataTableNames
    {
        /// <summary>
        /// Table name for the notification data table.
        /// </summary>
        public static readonly string TableName = "NotificationData";

        /// <summary>
        /// Draft notifications partition key name.
        /// </summary>
        public static readonly string DraftNotificationsPartition = "DraftNotifications";

        /// <summary>
        /// Sending notifications partition key name.
        /// </summary>
        public static readonly string SendingNotificationsPartition = "SendingNotifications";

        /// <summary>
        /// Global sending notification data partition key name.
        /// </summary>
        public static readonly string GlobalSendingNotificationDataPartition = "GlobalSendingNotificationData";

        /// <summary>
        /// Sent notifications partition key name.
        /// </summary>
        public static readonly string SentNotificationsPartition = "SentNotifications";
    }
}
