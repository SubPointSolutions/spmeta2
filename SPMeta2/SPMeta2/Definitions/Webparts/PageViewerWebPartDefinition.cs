using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'apps part' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]

    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.PageViewerWebPart, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class PageViewerWebPartDefinition : WebPartDefinition
    {
        #region constructors


        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsUrl]

        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string ContentLink { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string SourceType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("ContentLink", ContentLink)
                          .AddRawPropertyValue("SourceType", SourceType)
                          .ToString();
        }

        #endregion
    }
}
