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
    [DataContract]
    public class ContentTypeLinkValue
    {
        [DataMember]
        [IdentityKey]
        public string ContentTypeName { get; set; }

        [DataMember]
        [IdentityKey]
        public string ContentTypeId { get; set; }
    }

    [DataContract]

    public class FieldLinkValue
    {
        [DataMember]
        public string InternalName { get; set; }

        [DataMember]
        public Guid? Id { get; set; }

        public override string ToString()
        {
            return new ToStringResult<FieldLinkValue>(this)
                         .AddPropertyValue(p => p.InternalName)
                         .AddPropertyValue(p => p.Id)
                         .ToString();
        }
    }

    /// <summary>
    /// Allows to reorder content types in the particular list.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    public class UniqueContentTypeOrderDefinition : DefinitionBase
    {
        #region constructors

        public UniqueContentTypeOrderDefinition()
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
            return new ToStringResult<UniqueContentTypeOrderDefinition>(this)
                          .AddPropertyValue(p => p.ContentTypes)
                          .ToString();
        }

        #endregion
    }
}
