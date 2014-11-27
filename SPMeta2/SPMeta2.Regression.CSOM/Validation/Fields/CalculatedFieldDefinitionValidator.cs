using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class CalculatedFieldDefinitionValidator : ClientFieldDefinitionValidator
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

            var typedField = spObject.Context.CastTo<FieldCalculated>(spObject);

            var typedDefinition = model.WithAssertAndCast<CalculatedFieldDefinition>("model", value => value.RequireNotNull());
            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            typedFieldAssert.ShouldBeEqual(m => m.CurrencyLocaleId, o => o.GetCurrencyLocaleId());
            typedFieldAssert.ShouldBeEqual(m => m.DateFormat, o => o.GetDateFormatString());

            typedFieldAssert.ShouldBeEqual(m => m.OutputType, o => o.GetOutputType());
            typedFieldAssert.ShouldBeEqual(m => m.ShowAsPercentage, o => o.GetShowAsPercentage());
            typedFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormatString());

            // formula
            typedFieldAssert.ShouldBeEqual(m => m.Formula, o => o.Formula);
            typedFieldAssert.ShouldBeEqual(m => m.ValidationFormula, o => o.ValidationFormula);
            typedFieldAssert.ShouldBeEqual(m => m.ValidationMessage, o => o.ValidationMessage);

            // field refs
            if (typedDefinition.FieldReferences.Count > 0)
            {
                var hasFieldRefs = true;

                foreach (var dstFieldRef in typedField.GetFieldReferences())
                {
                    if (typedDefinition.FieldReferences.FirstOrDefault(c => c.ToUpper() == dstFieldRef.ToUpper()) == null)
                    {
                        hasFieldRefs = false;
                    }
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

    
}
