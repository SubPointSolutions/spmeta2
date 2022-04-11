using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Common;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [DataContract]
    [Serializable]
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
    /// Corresponds to USerResource.SetValueForUICulture() methods and Title/Description resources.
    /// </summary>
    [DataContract]
    [Serializable]

    public class ValueForUICulture
    {
        #region properties

        [DataMember]
        public int? CultureId { get; set; }


        [DataMember]
        public string CultureName { get; set; }

        [DataMember]
        public string Value { get; set; }

        #endregion
    }


    /// <summary>
    /// Allows to define and deploy SharePoint field.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPField", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]
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

            TitleResource = new List<ValueForUICulture>();
            DescriptionResource = new List<ValueForUICulture>();
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

        [XmlPropertyCapability]
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
        /// Corresponds to TitleResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> TitleResource { get; set; }

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
        /// Corresponds to DescriptionResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> DescriptionResource { get; set; }

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
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Type of the target field.
        /// BuiltInFieldTypes class can be used to utilize out of the box SharePoint fields.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public virtual string FieldType { get; set; }

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

        /// <summary>
        /// Corresponds to SPField.DefaultFormula
        /// Writeable for SSOM and read-only, first-time provision for CSOM.
        /// </summary>
        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]

        public virtual string DefaultFormula { get; set; }

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
        [DataMember]
        public bool? PushChangesToLists { get; set; }

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

        /// <summary>
        /// Gets or sets a Boolean value that specifies whether values in the field can be modified.
        /// Corresponds to SPField.ReadOnlyField property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]

        public bool? ReadOnlyField { get; set; }


        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                         .AddRawPropertyValue("InternalName", InternalName)
                         .AddRawPropertyValue("Id", Id)
                         .AddRawPropertyValue("Title", Title)
                         .AddRawPropertyValue("FieldType", FieldType)
                         .ToString();
        }

        #endregion
    }
}
