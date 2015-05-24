using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions.Fields
{
    public static class BuiltInChoiceFormatType
    {
        public static string Dropdown = "Dropdown";
        public static string RadioButtons = "RadioButtons";
    }

    /// <summary>
    /// Allows to define and deploy multi choice field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldMultiChoice", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldMultiChoice", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class MultiChoiceFieldDefinition : FieldDefinition
    {
        #region constructors

        public MultiChoiceFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.MultiChoice;

            Choices = new Collection<string>();
            Mappings = new Collection<string>();
        }

        #endregion

        #region properties

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

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public Collection<string> Choices { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public Collection<string> Mappings { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool FillInChoice { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<MultiChoiceFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.FillInChoice)
                          .AddPropertyValue(p => p.Choices)
                          .ToString();
        }

        #endregion
    }
}
