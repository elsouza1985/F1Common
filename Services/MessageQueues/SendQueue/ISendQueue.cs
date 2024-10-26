// <copyright file="ISendQueue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

using FC1_CC.Common.Services.MessageQueues;

namespace FC1_CC.Common.Services.MessageQueues.SendQueue
{
    /// <summary>
    /// interface for Send Queue.
    /// </summary>
    public interface ISendQueue : IBaseQueue<SendQueueMessageContent>
    {
    }
}
