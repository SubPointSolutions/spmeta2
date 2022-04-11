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

    [ExpectManyInstances]
    public class DependentLookupFieldDefinition : LookupFieldDefinition
    {
        #region constructors

        public DependentLookupFieldDefinition() : base()
        {
        }

        #endregion

        #region properties

        /// <summary>
        /// ID of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public override Guid Id { get; set; }

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

        [ExpectValidation]
        [DataMember]
        public override string ValidationMessage
        {
            get { return string.Empty; }
            set { }
        }

        [ExpectValidation]
        [DataMember]
        public override string ValidationFormula
        {
            get { return string.Empty; }
            set { }
        }

        #endregion

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("Title", Title)
                          .AddRawPropertyValue("InternalName", InternalName)
                          .AddRawPropertyValue("PrimaryLookupFieldId", PrimaryLookupFieldId)
                          .AddRawPropertyValue("AllowMultipleValues", AllowMultipleValues)
                          .ToString();
        }
    }
}
