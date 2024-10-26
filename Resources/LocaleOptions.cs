﻿// <copyright file="LocaleOptions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace FC1_CC.Common.Resources
{
    /// <summary>
    /// Options used for setting locale.
    /// </summary>
    public class LocaleOptions
    {
        /// <summary>
        /// Gets or sets the default culture.
        /// </summary>
        public string DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the supported cultures.
        /// </summary>
        public string SupportedCultures { get; set; }
    }
}
