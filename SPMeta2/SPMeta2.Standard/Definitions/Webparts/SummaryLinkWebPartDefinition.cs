using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Site Feed' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]


    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.Publishing.WebControls.SummaryLinkWebPart, Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class SummaryLinkWebPartDefinition : WebPartDefinition
    {
        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          //.AddRawPropertyValue(p => p.Group)
                          .ToString();
        }
    }
}
