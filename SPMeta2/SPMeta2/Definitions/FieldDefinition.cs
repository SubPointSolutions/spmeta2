using System.Collections.Generic;
using System.Collections.ObjectModel;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Common;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
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

        [ExpectValidation]
        /// <summary>
        /// Reflects AddToDefaultView option while adding field to the list
        /// </summary>
        public bool AddToDefaultView { get; set; }

        [ExpectValidation]
        /// <summary>
        /// Reflects SharePoint's AddFieldOptions while provisioning field for the first time
        /// </summary>
        public BuiltInAddFieldOptions AddFieldOptions { get; set; }

        /// <summary>
        /// Raw field XML to be used during the first provision
        /// </summary>
        [ExpectValidation]
        public string RawXml { get; set; }

        /// <summary>
        /// Additional attributes to be written for Field XML during the first provision
        /// </summary>
        [ExpectValidation]
        public List<FieldAttributeValue> AdditionalAttributes { get; set; }

        /// <summary>
        /// Internal name of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        public string InternalName { get; set; }

        /// <summary>
        /// Title of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        public string Title { get; set; }

        /// <summary>
        /// Description of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        public string Description { get; set; }

        /// <summary>
        /// Group of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        public string Group { get; set; }

        /// <summary>
        /// ID of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        public Guid Id { get; set; }

        /// <summary>
        /// Type of the target field.
        /// BuiltInFieldTypes class can be used to utilize out of the box SharePoint fields.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        public string FieldType { get; set; }

        /// <summary>
        /// Required flag for the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        public bool Required { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public string JSLink { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public virtual string DefaultValue { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool Hidden { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? ShowInDisplayForm { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? ShowInEditForm { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? ShowInListSettings { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? ShowInNewForm { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? ShowInVersionHistory { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? ShowInViewForms { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public bool? AllowDeletion { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public virtual bool Indexed { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        public virtual string ValidationFormula { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
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
