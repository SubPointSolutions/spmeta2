using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Fields
{
    public class ClientMediaFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(MediaFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);
        }
    }
}
