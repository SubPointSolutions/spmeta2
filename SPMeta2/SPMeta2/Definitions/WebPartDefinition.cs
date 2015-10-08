using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{

   

    /// <summary>
    /// Allows to define and deploy SharePoint web part.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WikiPageDefinition))]
    [ParentHostCapability(typeof(WebPartPageDefinition))]

    [ExpectManyInstances]

    public class WebPartDefinition : WebPartDefinitionBase
    {
        #region constructors

        public WebPartDefinition()
        {

        }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WebPartDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Id)
                          .AddPropertyValue(p => p.ZoneId)
                          .AddPropertyValue(p => p.ZoneIndex)

                          .AddPropertyValue(p => p.WebpartFileName)
                          .AddPropertyValue(p => p.WebpartType)
                // TODO, this is too big to put into ToString()
                //.AddPropertyValue(p => p.WebpartXmlTemplate)

                          .AddPropertyValue(p => p.AddToPageContent)
                          .ToString();
        }

        #endregion
    }
}
