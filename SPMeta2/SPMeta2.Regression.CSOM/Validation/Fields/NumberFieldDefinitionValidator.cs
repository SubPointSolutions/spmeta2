using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class NumberFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(NumberFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject.Context.CastTo<FieldNumber>(spObject);
            var textDefinition = model.WithAssertAndCast<NumberFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            typedFieldAssert.ShouldBeEqual(m => m.MaximumValue, o => o.MaximumValue);
            typedFieldAssert.ShouldBeEqual(m => m.MinimumValue, o => o.MinimumValue);
            typedFieldAssert.ShouldBeEqual(m => m.ShowAsPercentage, o => o.GetShowAsPercentage());

            if (!string.IsNullOrEmpty(textDefinition.DisplayFormat))
                typedFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDecimalsAsString());
            else
                typedFieldAssert.SkipProperty(m => m.DisplayFormat, "DisplayFormat is null or empty. Skipping.");
        }
    }
}
