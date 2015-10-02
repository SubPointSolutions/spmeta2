using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Regression.Definitions.Extended
{

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

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

        //[ExpectValidation]
        // don't need that
        public string PageFileName { get; set; }

        [ExpectValidation]
        public List<WebPartDefinitionBase> WebPartDefinitions { get; set; }
    }
}
