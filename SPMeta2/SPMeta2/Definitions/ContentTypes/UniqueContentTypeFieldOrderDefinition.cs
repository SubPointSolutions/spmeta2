﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.ContentTypes
{
    //public class ContentTypeLinkValue
    //{
    //    public string ContentTypeName { get; set; }
    //    public string ContentTypeId { get; set; }
    //}

    //public class FieldLinkValue
    //{
    //    public string InternalName { get; set; }
    //    public Guid? Id { get; set; }
    //}

    /// <summary>
    /// Allows to reorder fields from the particular content type.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ContentTypeDefinition))]

    [Serializable]
    [DataContract]

    [ParentHostCapability(typeof(ContentTypeDefinition))]
    public class UniqueContentTypeFieldsOrderDefinition : DefinitionBase
    {
        #region constructors

        public UniqueContentTypeFieldsOrderDefinition()
        {
            Fields = new List<FieldLinkValue>();
        }

        #endregion

        #region properties

        [DataMember]
        [ExpectValidation]
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
