using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class OutcomeChoiceFieldDefinitionValidator : ChoiceFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(OutcomeChoiceFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);
            
        }
    }
}
