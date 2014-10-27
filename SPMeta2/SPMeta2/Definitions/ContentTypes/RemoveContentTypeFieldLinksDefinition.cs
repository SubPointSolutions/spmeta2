using System;
using System.Collections.Generic;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.ContentTypes
{
    /// <summary>
    /// Allows to remove fields from the particular content type.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ContentTypeDefinition))]

    [Serializable]
    public class RemoveContentTypeFieldLinksDefinition : DefinitionBase
    {
        #region constructors

        public RemoveContentTypeFieldLinksDefinition()
        {
            Fields = new List<FieldLinkValue>();
        }

        #endregion

        #region properties

        public List<FieldLinkValue> Fields { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<RemoveContentTypeFieldLinksDefinition>(this)
                          .AddPropertyValue(p => p.Fields)
                          .ToString();
        }

        #endregion
    }
}
