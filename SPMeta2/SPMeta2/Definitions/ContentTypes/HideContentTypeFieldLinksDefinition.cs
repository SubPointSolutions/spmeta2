using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

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
    [DataContract]

    [ParentHostCapability(typeof(ContentTypeDefinition))]
    public class HideContentTypeFieldLinksDefinition : DefinitionBase
    {
        #region constructors

        public HideContentTypeFieldLinksDefinition()
        {
            Fields = new List<FieldLinkValue>();
        }

        #endregion

        #region properties

        [DataMember]
        [IdentityKey]
        public List<FieldLinkValue> Fields { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Fields", Fields)
                          .ToString();
        }

        #endregion
    }
}
