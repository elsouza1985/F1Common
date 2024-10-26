using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos.Table;

namespace FC1_CC.Common.Repositories.CardTemplateData
{
    public class CardTemplateEntity: TableEntity
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        public int CardLayout { get; set; }
        public string CardName { get; set; }
        /// <summary>
        /// Gets or sets the json of card aka https://adaptivecards.io/designer/
        /// </summary>
        public string CardJson { get; set; }

        public bool IsValid { get; set; }   
    }
}
