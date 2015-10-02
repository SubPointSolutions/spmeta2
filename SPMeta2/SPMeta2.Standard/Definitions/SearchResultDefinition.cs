﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Standard.Definitions
{

    /// <summary>
    /// Allows to define and deploy SharePoint Search Result
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.Search.Administration.Query.Source", "Microsoft.Office.Server.Search")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Search.Portability.SearchConfigurationPortability", "Microsoft.SharePoint.Client.Search")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

    public class SearchResultDefinition : DefinitionBase
    {
        #region constructors

        public SearchResultDefinition()
        {
            Description = string.Empty;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        public bool IsDefault { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string Query { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ProviderName { get; set; }

        [ExpectValidation]
        [DataMember]
        public Guid? ProviderId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SearchResultDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Description)
                          .AddPropertyValue(p => p.ProviderName)
                          .AddPropertyValue(p => p.ProviderId)
                          .AddPropertyValue(p => p.IsDefault)
                          .AddPropertyValue(p => p.Query)
                          .ToString();
        }

        #endregion
    }
}
