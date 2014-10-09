using System;
using System.Collections.Generic;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.ContentTypes
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPList", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.SPList", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    public class RemoveContentTypeLinksDefinition : DefinitionBase
    {
        #region constructors

        public RemoveContentTypeLinksDefinition()
        {
            ContentTypes = new List<ContentTypeValue>();
        }

        #endregion

        #region properties

        public List<ContentTypeValue> ContentTypes { get; set; }

        #endregion
    }
}
