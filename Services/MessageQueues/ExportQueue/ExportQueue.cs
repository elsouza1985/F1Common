// <copyright file="ExportQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Services.MessageQueues.ExportQueue
{
    using Azure.Messaging.ServiceBus;
    using FC1_CC.Common.Services.MessageQueues;

    /// <summary>
    /// The message queue service connected to the "company-communicator-export" queue in Azure service bus.
    /// </summary>
    public class ExportQueue : BaseQueue<ExportQueueMessageContent>, IExportQueue
    {
        /// <summary>
        /// Queue name of the export queue.
        /// </summary>
        public const string QueueName = "fc1-export";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportQueue"/> class.
        /// </summary>
        /// <param name="serviceBusClient">The service bus client.</param>
        public ExportQueue(ServiceBusClient serviceBusClient)
            : base(
                  serviceBusClient: serviceBusClient,
                  queueName: QueueName)
        {
        }
    }
}
