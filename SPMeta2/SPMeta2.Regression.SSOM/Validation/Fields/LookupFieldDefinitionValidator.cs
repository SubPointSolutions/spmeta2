using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class LookupFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(LookupFieldDefinition);
            }
        }
        protected override void CustomFieldTypeValidation(AssertPair<FieldDefinition, SPField> assert, SPField spObject, FieldDefinition definition)
        {
            var typedObject = spObject as SPFieldLookup;
            var typedDefinition = definition.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            // https://github.com/SubPointSolutions/spmeta2/issues/310
            // AllowMultipleValues - TRUE - LookupMulti
            // AllowMultipleValues - FALSE - Lookup
            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.FieldType);
                var dstProp = d.GetExpressionValue(m => d.TypeAsString);

                var isValid = typedDefinition.AllowMultipleValues
                    ? typedObject.TypeAsString == "LookupMulti"
                    : typedObject.TypeAsString == "Lookup";

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });
        }
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject as SPFieldLookup;
            var textDefinition = model.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            typedFieldAssert.ShouldBeEqual(m => m.AllowMultipleValues, o => o.AllowMultipleValues);

            if (textDefinition.LookupWebId.HasValue)
            {
                typedFieldAssert.ShouldBeEqual(m => m.LookupWebId, o => o.LookupWebId);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.LookupWebId, "LookupWebId is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(textDefinition.LookupList))
            {
                typedFieldAssert.ShouldBeEqual(m => m.LookupList, o => o.LookupList);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.LookupList, "LookupList is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(textDefinition.LookupField))
            {
                typedFieldAssert.ShouldBeEqual(m => m.LookupField, o => o.LookupField);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.LookupField, "LookupField is NULL. Skipping.");
            }
        }
    }
}
