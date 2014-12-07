using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Fields
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
            return typeof(FieldNumber);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            field.ValidationMessage = fieldModel.ValidationMessage ?? string.Empty;
            field.ValidationFormula = fieldModel.ValidationFormula ?? string.Empty;

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
