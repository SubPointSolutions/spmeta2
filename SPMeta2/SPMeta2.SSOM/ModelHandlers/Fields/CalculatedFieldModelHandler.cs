using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class CalculatedFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(CalculatedFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldCalculated);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<CalculatedFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as SPFieldCalculated;

            typedField.Formula = typedFieldModel.Formula ?? string.Empty;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<CalculatedFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.LCID, typedFieldModel.CurrencyLocaleId);
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Formula, typedFieldModel.Formula);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Format, (int)Enum.Parse(typeof(SPDateTimeFieldFormatType), typedFieldModel.DateFormat));

            if (typedFieldModel.ShowAsPercentage.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Percentage, typedFieldModel.ShowAsPercentage.Value.ToString().ToUpper());

            if (!string.IsNullOrEmpty(typedFieldModel.DisplayFormat))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Decimals, NumberFieldModelHandler.GetDecimalsValue(typedFieldModel.DisplayFormat));

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.ResultType, typedFieldModel.OutputType);

            if (typedFieldModel.FieldReferences.Count > 0)
            {
                var fieldRefsNode = new XElement("FieldRefs");

                foreach (var fieldRef in typedFieldModel.FieldReferences)
                {
                    var fieldRefNode = new XElement("FieldRef");

                    fieldRefNode.SetAttribute("Name", fieldRef);
                    fieldRefsNode.Add(fieldRefNode);
                }

                fieldTemplate.Add(fieldRefsNode);
            }
        }

        #endregion
    }
}
