using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy choice field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldChoice", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldChoice", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    [ExpectManyInstances]

    public class ChoiceFieldDefinition : MultiChoiceFieldDefinition
    {
        #region constructors

        public ChoiceFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Choice;
            EditFormat = BuiltInChoiceFormatType.Dropdown;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override sealed string FieldType { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsChoiceFieldEditFormat]
        public string EditFormat { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("EditFormat", EditFormat)
                          .ToString();
        }

        #endregion
    }


}
