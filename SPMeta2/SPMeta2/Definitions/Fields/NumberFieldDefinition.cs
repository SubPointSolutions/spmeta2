using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy number field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldNumber", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldNumber", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    [ExpectManyInstances]

    public class NumberFieldDefinition : FieldDefinition
    {
        #region constructors

        public NumberFieldDefinition()
        {
            DisplayFormat = BuiltInNumberFormatTypes.Automatic;
        }

        #endregion

        #region properties


        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public override string FieldType
        {
            get
            {
                return BuiltInFieldTypes.Number;
            }
            set
            {

            }
        }

        /// <summary>
        /// Can be updated in SSOM.
        /// CSOM API does not support changes/updates.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsNumberFieldDisplayFormat()]
        public string DisplayFormat { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 1000, MaxValue = 5000)]
        [DataMember]
        public double? MaximumValue { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 100, MaxValue = 500)]
        [DataMember]
        public double? MinimumValue { get; set; }

        [ExpectValidation]
        //[ExpectUpdate], only SSOM
        [DataMember]
        public bool ShowAsPercentage { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("MaximumValue", MaximumValue)
                          .AddRawPropertyValue("MinimumValue", MinimumValue)
                          .AddRawPropertyValue("ShowAsPercentage", ShowAsPercentage)
                          .ToString();
        }

        #endregion
    }
}
