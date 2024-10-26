// <copyright file="ExportStatus.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Repositories.ExportData
{
    /// <summary>
    /// Export telemetry status.
    /// </summary>
    public enum ExportStatus
    {
        /// <summary>
        /// This represents the export is scheduled.
        /// </summary>
        New,

        /// <summary>
        /// This represents the export is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// This reprsents the export is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// This represents the export is failed.
        /// </summary>
        Failed,
    }
}
