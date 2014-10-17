using System;
using System.Collections.Generic;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions.ContentTypes
{
    /// <summary>
    /// Allows to hide particular fields in the target content type.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ContentTypeDefinition))]

    [Serializable]
    public class HideContentTypeFieldLinksDefinition : DefinitionBase
    {
        #region constructors

        public HideContentTypeFieldLinksDefinition()
        {
            Fields = new List<FieldLinkValue>();
        }

        #endregion

        #region properties

        public List<FieldLinkValue> Fields { get; set; }

        #endregion
    }
}
