using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class NumberFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(NumberFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldNumber);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            if (!string.IsNullOrEmpty(fieldModel.ValidationMessage))
                field.ValidationMessage = fieldModel.ValidationMessage;

            if (!string.IsNullOrEmpty(fieldModel.ValidationFormula))
                field.ValidationFormula = fieldModel.ValidationFormula;

            var typedFieldModel = fieldModel.WithAssertAndCast<NumberFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as SPFieldNumber;

            if (typedFieldModel.MinimumValue.HasValue)
                typedField.MinimumValue = typedFieldModel.MinimumValue.Value;

            if (typedFieldModel.MaximumValue.HasValue)
                typedField.MaximumValue = typedFieldModel.MaximumValue.Value;

            typedField.ShowAsPercentage = typedFieldModel.ShowAsPercentage;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<NumberFieldDefinition>("model", value => value.RequireNotNull());

            if (typedFieldModel.MinimumValue.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Min, typedFieldModel.MinimumValue);

            if (typedFieldModel.MaximumValue.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Max, typedFieldModel.MaximumValue);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Percentage, typedFieldModel.ShowAsPercentage.ToString().ToUpper());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Decimals, GetDecimalsValue(typedFieldModel.DisplayFormat));
        }

        internal static int GetDecimalsValue(string value)
        {
            if (value == BuiltInNumberFormatTypes.Automatic)
                return -1;

            if (value == BuiltInNumberFormatTypes.NoDecimal)
                return 0;

            if (value == BuiltInNumberFormatTypes.OneDecimal)
                return 1;

            if (value == BuiltInNumberFormatTypes.TwoDecimals)
                return 2;

            if (value == BuiltInNumberFormatTypes.ThreeDecimals)
                return 3;

            if (value == BuiltInNumberFormatTypes.FourDecimals)
                return 4;

            if (value == BuiltInNumberFormatTypes.FiveDecimals)
                return 5;

            throw new ArgumentException("DisplayFormat");
        }

        #endregion
    }
}
