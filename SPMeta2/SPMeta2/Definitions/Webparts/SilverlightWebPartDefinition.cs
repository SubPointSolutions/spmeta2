using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Silverlight' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.SilverlightWebPart, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class SilverlightWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        [ExpectValidation]
        [ExpectUpdateAsUrl(Extension = "xap")]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]

        public string Url { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectUpdate]
        public string CustomInitParameters { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("Url", Url)
                          .AddRawPropertyValue("CustomInitParameters", CustomInitParameters)
                          .ToString();
        }

        #endregion
    }
}
