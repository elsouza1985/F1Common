// <copyright file="PrepareToSendQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MessageQueues.PrepareToSendQueue
{
    using FC1_CC.Common.Services.MessageQueues;
    using Azure.Messaging.ServiceBus;

    /// <summary>
    /// The message queue service connected to the "company-communicator-prep" queue in Azure service bus.
    /// </summary>
    public class PrepareToSendQueue : BaseQueue<PrepareToSendQueueMessageContent>, IPrepareToSendQueue
    {
        /// <summary>
        /// Queue name of the prepare to send queue.
        /// </summary>
        public const string QueueName = "fc1-prep";

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareToSendQueue"/> class.
        /// </summary>
        /// <param name="serviceBusClient">The service bus client.</param>
        public PrepareToSendQueue(ServiceBusClient serviceBusClient)
            : base(
                  serviceBusClient: serviceBusClient,
                  queueName: QueueName)
        {
        }
    }
}
