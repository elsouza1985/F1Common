// <copyright file="RecipientsInfo.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.Recipients
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Recipient information.
    /// </summary>
    public class RecipientsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecipientsInfo"/> class.
        /// </summary>
        /// <param name="notificationId">notification id.</param>
        public RecipientsInfo(string notificationId)
        {
            if (string.IsNullOrEmpty(notificationId))
            {
                throw new ArgumentNullException(nameof(notificationId));
            }

            // Initialize properties.
            TotalRecipientCount = 0;
            BatchKeys = new List<string>();
            HasRecipientsPendingInstallation = false;
            NotificationId = notificationId;
        }

        /// <summary>
        /// Gets the notification id.
        /// </summary>
        public string NotificationId { get; private set; }

        /// <summary>
        /// Gets or sets the total recipient count of the message.
        /// </summary>
        public int TotalRecipientCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are user app installations pending(recipients who have no conversation id in database) for recipients.
        /// </summary>
        public bool HasRecipientsPendingInstallation { get; set; }

        /// <summary>
        /// Gets or sets the batch keys of the recipients.
        /// </summary>
        public List<string> BatchKeys { get; set; }
    }
}
