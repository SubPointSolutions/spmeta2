using System;
using System.Collections.Generic;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

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

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ContentTypeDefinition))]

    [Serializable]
    public class UniqueContentTypeFieldsOrderDefinition : DefinitionBase
    {
        #region constructors

        public UniqueContentTypeFieldsOrderDefinition()
        {
            Fields = new List<FieldLinkValue>();
        }

        #endregion

        #region properties

        public List<FieldLinkValue> Fields { get; set; }

        #endregion
    }
}
