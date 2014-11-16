using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class BooleanFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(BooleanFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject as SPFieldBoolean;
            var textDefinition = model.WithAssertAndCast<BooleanFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

        }
    }
}
