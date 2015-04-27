using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy boolean field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldLookup", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldLookup", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class DependentLookupFieldDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string InternalName { get; set; }

        /// <summary>
        /// Title of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Description of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// Group of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string Group { get; set; }

        [ExpectValidation]
        [DataMember]
        public string LookupField { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "PrimaryLookup")]
        public string PrimaryLookupFieldInternalName { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "PrimaryLookup")]
        public string PrimaryLookupFieldTitle { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectRequired(GroupName = "PrimaryLookup")]
        public Guid? PrimaryLookupFieldId { get; set; }

        #endregion

        public override string ToString()
        {
            return new ToStringResult<DependentLookupFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.InternalName)
                          .ToString();
        }
    }
}
