using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'XsltListView' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.XsltListViewWebPart , Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class XsltListViewWebPartDefinition : WebPartDefinition
    {
        #region constructors

        public XsltListViewWebPartDefinition()
        {
            TitleUrl = string.Empty;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "Target List")]
        public string ListTitle { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "Target List")]
        public string ListUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "Target List")]
        public Guid? ListId { get; set; }

        [ExpectValidation]
        [DataMember]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string WebUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public Guid? WebId { get; set; }


        [ExpectValidation]
        [DataMember]
        public string ViewName { get; set; }

        [ExpectValidation]
        [DataMember]
        public Guid? ViewId { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ViewUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        public string JSLink { get; set; }

        [ExpectValidation]
        [ExpectUpdatAsToolbarType]
        [ExpectNullable]
        [DataMember]
        public string Toolbar { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [ExpectNullable]
        [DataMember]
        public bool? ToolbarShowAlways { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? ShowTimelineIfAvailable { get; set; }

        /// <summary>
        /// Enable Data View Caching prop.
        /// </summary>
        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? CacheXslStorage { get; set; }

        /// <summary>
        /// Data View Caching Time-out (seconds)
        /// </summary>
        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public int? CacheXslTimeOut { get; set; }


        //[ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? DisableSaveAsNewViewButton { get; set; }


        //[ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? DisableViewSelectorMenu { get; set; }


        //[ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? DisableColumnFiltering { get; set; }

        //[ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool? InplaceSearchEnabled { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public string BaseXsltHashKey { get; set; }


        #region xml-xslt props

        [ExpectValidation]
        [DataMember]

        [XmlPropertyCapability]
        public string XmlDefinition { get; set; }

        [ExpectValidation]
        [DataMember]
        public string XmlDefinitionLink { get; set; }

        [ExpectValidation]
        [DataMember]

        [XsltPropertyCapability]
        public string Xsl { get; set; }

        [ExpectValidation]
        [DataMember]
        public string XslLink { get; set; }

        [ExpectValidation]
        [DataMember]
        public string GhostedXslLink { get; set; }

        #endregion

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<XsltListViewWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.ListTitle)
                          .AddPropertyValue(p => p.ListUrl)
                          .AddPropertyValue(p => p.ListId)

                          .AddPropertyValue(p => p.ViewName)
                          .AddPropertyValue(p => p.ViewId)

                          .AddPropertyValue(p => p.JSLink)
                          .ToString();
        }

        #endregion
    }
}
