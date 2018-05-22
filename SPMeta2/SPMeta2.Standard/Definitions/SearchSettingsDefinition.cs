using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Allows to define and deploy SharePoint audience.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPWeb", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Web", "Microsoft.SharePoint.Client")]


    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    public class SearchSettingsDefinition : DefinitionBase
    {
        #region constructors

        public SearchSettingsDefinition()
        {

        }

        #endregion

        #region properties

        [ExpectUpdateAsUrl(Extension = ".aspx")]
        [ExpectValidation]
        [DataMember]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string SearchCenterUrl { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? UseParentResultsPageUrl { get; set; }

        [ExpectUpdateAsUrl(Extension = ".aspx")]
        [ExpectValidation]
        [DataMember]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string UseCustomResultsPageUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? UseFirstSearchNavigationNode { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("SearchCenterUrl", SearchCenterUrl)
                          .AddRawPropertyValue("UseParentResultsPageUrl", UseParentResultsPageUrl)
                          .AddRawPropertyValue("UseCustomResultsPageUrl", UseCustomResultsPageUrl)
                          .AddRawPropertyValue("UseFirstSearchNavigationNode", UseFirstSearchNavigationNode)
                          .ToString();
        }

        #endregion
    }
}
