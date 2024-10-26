// <copyright file="IPrepareToSendQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

using FC1_CC.Common.Services.MessageQueues;

namespace FC1_CC.Common.Services.MessageQueues.PrepareToSendQueue
{
    /// <summary>
    /// interface for Prepare to send Queue.
    /// </summary>
    public interface IPrepareToSendQueue : IBaseQueue<PrepareToSendQueueMessageContent>
    {
    }
}
