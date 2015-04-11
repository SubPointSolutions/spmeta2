using System;
using System.Collections.Generic;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
        public List<ContentTypeLinkValue> ContentTypes { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<RemoveContentTypeLinksDefinition>(this)
                          .AddPropertyValue(p => p.ContentTypes)
                          .ToString();
        }

        #endregion
    }
}
