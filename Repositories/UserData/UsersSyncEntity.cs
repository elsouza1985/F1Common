// <copyright file="UsersSyncEntity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Repositories.UserData
{
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Users sync information.
    /// </summary>
    public class UsersSyncEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the entity value.
        /// </summary>
        public string Value { get; set; }
    }
}
