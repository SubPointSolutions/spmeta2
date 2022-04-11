using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint content type.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]
    public class ContentTypeDefinition : DefinitionBase
    {
        #region constructors

        public ContentTypeDefinition()
        {
            NameResource = new List<ValueForUICulture>();
            DescriptionResource = new List<ValueForUICulture>();
        }

        #endregion

        #region properties

        /// <summary>
        /// ID of the target content type. 
        /// Final content type id is calculated based on ParentContentTypeId, IdNumberValue or Id properties with ContentTypeDefinitionSyntax.GetContentTypeId() method.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired(GroupName = "ContentType Id")]
        [DataMember]
        [IdentityKey]
        public Guid Id { get; set; }

        /// <summary>
        /// IdNumberValue of the target content type. 
        /// Final content type id is calculated based on ParentContentTypeId, IdNumberValue or Id properties with ContentTypeDefinitionSyntax.GetContentTypeId() method.
        /// </summary>
        /// 

        [ExpectRequired(GroupName = "ContentType Id")]
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string IdNumberValue { get; set; }

        /// <summary>
        /// Name of the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }


        /// <summary>
        /// Corresponds to NameResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> NameResource { get; set; }

        /// <summary>
        /// Description if the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// Corresponds to DescriptionResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> DescriptionResource { get; set; }

        /// <summary>
        /// Group of the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Group { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool Hidden { get; set; }

        /// <summary>
        /// Parent content type id. BuiltInContentTypeId class could be used to utilize out of the box content type ids.
        /// </summary>
        /// 
        [ExpectRequired(GroupName = "Parent ContentType Id")]
        [DataMember]
        public string ParentContentTypeId { get; set; }

        /// <summary>
        /// Optional parent content type name. Must be used carefully in multi langual environments, because ct names will be different
        /// </summary>
        [ExpectRequired(GroupName = "Parent ContentType Id")]
        [DataMember]
        public string ParentContentTypeName { get; set; }

        [ExpectValidation]
        [DataMember]
        [SiteCollectionTokenCapability]
        [WebTokenCapability]
        public string DocumentTemplate { get; set; }

        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string JSLink { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        public bool? ReadOnly { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        public bool? Sealed { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        [ExpectUpdate]
        public string NewFormUrl { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        [ExpectUpdate]
        public string NewFormTemplateName { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        [ExpectUpdate]
        public string EditFormUrl { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        [ExpectUpdate]
        public string EditFormTemplateName { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        [ExpectUpdate]
        public string DisplayFormUrl { get; set; }

        [DataMember]
        [ExpectNullable]
        [ExpectValidation]
        [ExpectUpdate]
        public string DisplayFormTemplateName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Id:[{1}] IdNumberValue:[{2}]", Name, Id, IdNumberValue);
        }

        #endregion
    }
}
