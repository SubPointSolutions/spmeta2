using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Regression.Definitions
{

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    [DataContract]

    //[ExpectAddHostExtensionMethod]
    //[ExpectWithExtensionMethod]
    //[ExpectArrayExtensionMethod]

    //[ParentHostCapability(typeof(ListDefinition))]

    //[ExpectManyInstances]
    public class WebpartPresenceOnPageDefinition : DefinitionBase
    {
        public WebpartPresenceOnPageDefinition()
        {
            WebPartDefinitions = new List<WebPartDefinitionBase>();
        }

        [ExpectValidation]
        public string PageFileName { get; set; }

        [ExpectValidation]
        public List<WebPartDefinitionBase> WebPartDefinitions { get; set; }
    }
}
