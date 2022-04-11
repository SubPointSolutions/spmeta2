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
    /// Allows to remove content types from the particular list.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPList", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.List", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(ListDefinition))]
    public class RemoveContentTypeLinksDefinition : DefinitionBase
    {
        #region constructors

        public RemoveContentTypeLinksDefinition()
        {
            ContentTypes = new List<ContentTypeLinkValue>();
        }

        #endregion

        #region properties

        [DataMember]
        [IdentityKey]
        [ExpectValidation]
        public List<ContentTypeLinkValue> ContentTypes { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ContentTypes", ContentTypes)
                          .ToString();
        }

        #endregion
    }
}
