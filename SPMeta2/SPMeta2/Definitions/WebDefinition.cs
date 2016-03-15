﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [Serializable]
    [DataContract]
    public class IndexedPropertyValue
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// Allows too define and deploy SharePoint web site.
    /// </summary>

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]


    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]

    public class WebDefinition : DefinitionBase
    {
        #region constructors

        public WebDefinition()
        {
            Url = "/";
            LCID = 1033;

            TitleResource = new List<ValueForUICulture>();
            DescriptionResource = new List<ValueForUICulture>();
            IndexedPropertyKeys = new List<IndexedPropertyValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target web.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Corresponds to TitleResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> TitleResource { get; set; }

        /// <summary>
        /// Description of the target web.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// Corresponds to DescriptionResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> DescriptionResource { get; set; }

        /// <summary>
        /// LCID of the target web.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        //[ExpectUpdateAsLCID]
        public uint LCID { get; set; }

        /// <summary>
        /// Should unique permissions be used for the new web.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool UseUniquePermission { get; set; }

        /// <summary>
        /// Convert to the new web template if web if there.
        /// </summary>
        /// 

        [DataMember]
        public bool ConvertIfThere { get; set; }

        /// <summary>
        /// URL of the target. 
        /// Should be relative to the current web. 
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Url { get; set; }

        /// <summary>
        /// WebTemplate of the target web.
        /// 
        /// BuiltInWebTemplates class can be used to utilize out of the box SharePoint web templates.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web Template")]
        [DataMember]
        public string WebTemplate { get; set; }

        /// <summary>
        /// Custom web template name of the target web.
        /// </summary>
        [ExpectRequired(GroupName = "Web Template")]
        [DataMember]
        [ExpectValidation]
        public string CustomWebTemplate { get; set; }

        [DataMember]
        [ExpectValidation]
        //[ExpectUpdateAsUrl(Extension = ".png")]
        [ExpectNullable]
        public string SiteLogoUrl { get; set; }

        [DataMember]
        [ExpectValidation]
        //[ExpectUpdateAsUrl(Extension = ".css")]
        [ExpectNullable]
        public string AlternateCssUrl { get; set; }

        /// <summary>
        /// Gets the set of property keys for properties that need to be exposed through Site Data Web Service.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public List<IndexedPropertyValue> IndexedPropertyKeys { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Description)
                          .AddPropertyValue(p => p.LCID)
                          .AddPropertyValue(p => p.UseUniquePermission)
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.WebTemplate)
                          .AddPropertyValue(p => p.CustomWebTemplate)

                          .ToString();
        }

        #endregion
    }
}