using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Fields
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
            return typeof(FieldCalculated);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<CalculatedFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field.Context.CastTo<FieldCalculated>(field);

            typedField.Formula = typedFieldModel.Formula ?? string.Empty;
            typedField.OutputType = typedField.OutputType = (FieldType)Enum.Parse(typeof(FieldType), typedFieldModel.OutputType);
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<CalculatedFieldDefinition>("model", value => value.RequireNotNull());

            if (typedFieldModel.CurrencyLocaleId.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.LCID, typedFieldModel.CurrencyLocaleId);

            // should be a new XML node
            var formulaNode = new XElement(BuiltInFieldAttributes.Formula, typedFieldModel.Formula);
            fieldTemplate.Add(formulaNode);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Format, (int)Enum.Parse(typeof(DateTimeFieldFormatType), typedFieldModel.DateFormat));

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
