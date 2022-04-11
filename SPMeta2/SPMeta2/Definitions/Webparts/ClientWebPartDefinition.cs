using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
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

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.ClientWebPart, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class ClientWebPartDefinition : WebPartDefinition
    {
        #region properties

        [ExpectRequired]
        [DataMember]
        public Guid FeatureId { get; set; }

        [ExpectRequired]
        [DataMember]
        public Guid? ProductId { get; set; }

        // Remoed ExpectRequired
        // Enhance 'ClientWebPart' provision - ProductWebId should be current web by default #623
        // https://github.com/SubPointSolutions/spmeta2/issues/623
        // [ExpectRequired]

        [DataMember]
        public Guid ProductWebId { get; set; }

        [ExpectRequired]
        [DataMember]
        public string WebPartName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("FeatureId", FeatureId)
                          .AddRawPropertyValue("ProductId", ProductId)
                          .AddRawPropertyValue("ProductWebId", ProductWebId)
                          .AddRawPropertyValue("WebPartName", WebPartName)
                          .ToString();
        }

        #endregion
    }
}
