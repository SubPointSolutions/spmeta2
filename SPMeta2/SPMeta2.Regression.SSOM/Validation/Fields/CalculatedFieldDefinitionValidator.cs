using System;
using System.Linq;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Containers.Utils;
using System.Xml.Linq;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class CalculatedFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(CalculatedFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var typedField = spObject as SPFieldCalculated;

            var typedDefinition = model.WithAssertAndCast<CalculatedFieldDefinition>("model", value => value.RequireNotNull());
            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            typedFieldAssert.ShouldBeEqual(m => m.CurrencyLocaleId, o => o.CurrencyLocaleId);
            typedFieldAssert.ShouldBeEqual(m => m.DateFormat, o => o.GetDateFormat());



            typedFieldAssert.ShouldBeEqual(m => m.OutputType, o => o.GetOutputType());
            //
            typedFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormat());

            if (typedDefinition.ShowAsPercentage.HasValue)
                typedFieldAssert.ShouldBeEqual(m => m.ShowAsPercentage, o => o.ShowAsPercentage);
            else
                typedFieldAssert.SkipProperty(m => m.ShowAsPercentage, "ShowAsPercentage is NULL. Skipping.");

            if (!string.IsNullOrEmpty(typedDefinition.Formula))
                typedFieldAssert.ShouldBeEqual(m => m.Formula, o => o.Formula);
            else
                typedFieldAssert.SkipProperty(m => m.Formula);

            IndentableTrace.WithScope(s =>
            {
                s.WriteLine(string.Format("Formula: Src:[{0}] Dst:[{1}]", typedDefinition.Formula, typedField.Formula));
            });

            // field refs
            if (typedDefinition.FieldReferences.Count > 0)
            {
                var hasFieldRefs = true;

                if (typedField.FieldReferences != null)
                {
                    foreach (var dstFieldRef in typedField.FieldReferences)
                    {
                        if (typedDefinition.FieldReferences.FirstOrDefault(c => c.ToUpper() == dstFieldRef.ToUpper()) ==
                            null)
                        {
                            hasFieldRefs = false;
                        }
                    }

                    if (typedField.FieldReferences.Length == 0)
                        hasFieldRefs = false;
                }
                else
                {
                    hasFieldRefs = false;
                }

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.FieldReferences);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = hasFieldRefs == true
                    };
                });
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.FieldReferences, "FieldReferences.Count == 0. Skipping.");
            }

        }
    }

    internal static class SPFieldCalculatedUtils
    {
        public static string GetOutputType(this SPFieldCalculated field)
        {
            return field.OutputType.ToString();
        }

        public static string GetDisplayFormat(this SPFieldCalculated field)
        {
            return field.DisplayFormat.ToString();
        }

        public static string GetDateFormat(this SPFieldCalculated field)
        {
            var value = field.DateFormat.ToString(); ;

            // also, wihtint expected range
            field.GetDateFormatString();

            return field.DateFormat.ToString();
        }

        public static string GetDateFormatString(this SPField field)
        {
            var value = GetDateFormat(field);

            // Format="0" when provisioning CalculatedField #969
            // https://github.com/SubPointSolutions/spmeta2/issues/969

            if (string.Compare(value, BuiltInDateTimeFieldFormatType.DateOnly, true) == 0)
                return BuiltInDateTimeFieldFormatType.DateOnly;

            if (string.Compare(value, BuiltInDateTimeFieldFormatType.DateTime, true) == 0)
                return BuiltInDateTimeFieldFormatType.DateTime;

            throw new ArgumentException("BuiltInDateTimeFieldFormatType was:" + value);

        }

        public static string GetDateFormat(this SPField field)
        {
            var xml = field.SchemaXml;

            return ConvertUtils.ToString(XElement.Parse(xml).GetAttributeValue(BuiltInFieldAttributes.Format));
        }
    }


}
