using System.Collections.Generic;
using System.Collections.ObjectModel;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Common;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    [DataContract]
    public class FieldAttributeValue : KeyNameValue
    {
        public FieldAttributeValue()
        {

        }

        public FieldAttributeValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }


    /// <summary>
    /// Allows to define and deploy SharePoint field.
    /// </summary>
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPField", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHostAttribute(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class FieldDefinition : DefinitionBase
    {
        #region constructors

        public FieldDefinition()
        {
            // it needs to be string.Empty to avoid challenges with null VS string.Empty test cases for strings
            Description = string.Empty;
            Group = string.Empty;

            RawXml = string.Empty;

            AdditionalAttributes = new List<FieldAttributeValue>();
            AddFieldOptions = BuiltInAddFieldOptions.DefaultValue;
        }

        #endregion

        #region properties


        /// <summary>
        /// Reflects AddToDefaultView option while adding field to the list
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool AddToDefaultView { get; set; }


        /// <summary>
        /// Reflects SharePoint's AddFieldOptions while provisioning field for the first time
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public BuiltInAddFieldOptions AddFieldOptions { get; set; }

        /// <summary>
        /// Raw field XML to be used during the first provision
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public string RawXml { get; set; }

        /// <summary>
        /// Additional attributes to be written for Field XML during the first provision
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public List<FieldAttributeValue> AdditionalAttributes { get; set; }

        /// <summary>
        /// Internal name of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "IdOrInternalName")]
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
        [ExpectNullable]
        public string Group { get; set; }

        /// <summary>
        /// ID of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "IdOrInternalName")]
        [DataMember]
        [IdentityKey]
        public Guid Id { get; set; }

        /// <summary>
        /// Type of the target field.
        /// BuiltInFieldTypes class can be used to utilize out of the box SharePoint fields.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string FieldType { get; set; }

        /// <summary>
        /// Required flag for the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool Required { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string StaticName { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public string JSLink { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public virtual string DefaultValue { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool Hidden { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? ShowInDisplayForm { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? ShowInEditForm { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? ShowInListSettings { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? ShowInNewForm { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? ShowInVersionHistory { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? ShowInViewForms { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? AllowDeletion { get; set; }


        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? EnforceUniqueValues { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public virtual bool Indexed { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public virtual string ValidationFormula { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public virtual string ValidationMessage { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<FieldDefinition>(this, base.ToString())
                         .AddPropertyValue(p => p.InternalName)
                         .AddPropertyValue(p => p.Id)
                         .AddPropertyValue(p => p.Title)
                         .AddPropertyValue(p => p.FieldType)
                         .ToString();
        }

        #endregion
    }
}
