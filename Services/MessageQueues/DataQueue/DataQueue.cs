// <copyright file="DataQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MessageQueues.DataQueue
{
    using System;
    using System.Threading.Tasks;
    using FC1_CC.Common.Services.MessageQueues;
    using Azure.Messaging.ServiceBus;

    /// <summary>
    /// The message queue service connected to the "company-communicator-data" queue in Azure service bus.
    /// </summary>
    public class DataQueue : BaseQueue<DataQueueMessageContent>, IDataQueue
    {
        /// <summary>
        /// Queue name of the data queue.
        /// </summary>
        public const string QueueName = "fc1-data";

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueue"/> class.
        /// </summary>
        /// <param name="serviceBusClient">The service bus client.</param>
        public DataQueue(ServiceBusClient serviceBusClient)
            : base(
                  serviceBusClient: serviceBusClient,
                  queueName: QueueName)
        {
        }

        /// <inheritdoc/>
        public async Task SendMessageAsync(string notificationId, TimeSpan messageDelay)
        {
            var dataQueueMessageContent = new DataQueueMessageContent
            {
                NotificationId = notificationId,
                ForceMessageComplete = false,
            };

            await SendDelayedAsync(
                dataQueueMessageContent,
                messageDelay.TotalSeconds);
        }
    }
}
