using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint content type.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultParentHostAttribute(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ContentTypeDefinition : DefinitionBase
    {
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
        /// Description if the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

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
        [ExpectRequired]
        [DataMember]
        public string ParentContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        public string DocumentTemplate { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Id:[{1}] IdNumberValue:[{2}]", Name, Id, IdNumberValue);
        }

        #endregion
    }
}
