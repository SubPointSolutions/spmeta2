using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class ComputedFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(ComputedFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject.Context.CastTo<FieldComputed>(spObject);
            var textDefinition = model.WithAssertAndCast<ComputedFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            if (textDefinition.EnableLookup.HasValue)
            {
                textFieldAssert.ShouldBeEqual(m => m.EnableLookup, o => o.EnableLookup);
            }
            else
            {
                textFieldAssert.SkipProperty(m => m.EnableLookup, "EnableLookup is NULL. Skipping.");
            }
        }
    }
}
