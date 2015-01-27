using System.Collections.Generic;
using System.Collections.ObjectModel;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Common;

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
        }

        #endregion

        #region properties

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
        public string InternalName { get; set; }

        /// <summary>
        /// Title of the target field.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
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
        public Guid Id { get; set; }

        /// <summary>
        /// Type of the target field.
        /// BuiltInFieldTypes class can be used to utilize out of the box SharePoint fields.
        /// </summary>
        /// 
        [ExpectValidation]
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
        public string DefaultValue { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool Hidden { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? ShowInDisplayForm { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? ShowInEditForm { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? ShowInListSettings { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? ShowInNewForm { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? ShowInVersionHistory { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? ShowInViewForms { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool? AllowDeletion { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public bool Indexed { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public string ValidationFormula { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public string ValidationMessage { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("InternalName:[{0}] Id:[{1}] Title:[{2}]", InternalName, Id, Title);
        }

        #endregion
    }
}
