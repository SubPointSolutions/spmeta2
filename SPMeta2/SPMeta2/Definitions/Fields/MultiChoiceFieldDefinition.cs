using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

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
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldMultiChoice", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]
    public class MultiChoiceFieldDefinition : FieldDefinition
    {
        #region constructors

        public MultiChoiceFieldDefinition()
        {
            Choices = new Collection<string>();
            Mappings = new Collection<string>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override string FieldType
        {
            get { return BuiltInFieldTypes.MultiChoice; }
            set
            {

            }
        }

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
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("FillInChoice", FillInChoice)
                          .AddRawPropertyValue("Choices", Choices)
                          .ToString();
        }

        #endregion
    }
}
