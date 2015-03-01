using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldNumber", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class NumberFieldDefinition : FieldDefinition
    {
        #region constructors

        public NumberFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Number;
            DisplayFormat = BuiltInNumberFormatTypes.Automatic;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public string DisplayFormat { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 1000, MaxValue = 5000)]
        public double? MaximumValue { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 100, MaxValue = 500)]
        public double? MinimumValue { get; set; }

        [ExpectValidation]
        //[ExpectUpdate], only SSOM
        public bool ShowAsPercentage { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<NumberFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(d => d.MaximumValue)
                          .AddPropertyValue(d => d.MinimumValue)
                          .AddPropertyValue(d => d.ShowAsPercentage)
                          .ToString();
        }

        #endregion
    }
}
