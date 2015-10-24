using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class ChoiceFieldDefinitionValidator : MultiChoiceFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(ChoiceFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var textField = spObject as SPFieldChoice;
            var textDefinition = model.WithAssertAndCast<ChoiceFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            if (!string.IsNullOrEmpty(textDefinition.EditFormat))
                textFieldAssert.ShouldBeEqual(m => m.EditFormat, o => o.GetEditFormat());
            else
            {
                textFieldAssert.SkipProperty(m => m.EditFormat);
            }
        }
    }

    internal static class SPFieldChoiceUtil
    {
        public static string GetEditFormat(this SPFieldChoice field)
        {
            return field.EditFormat.ToString();
        }
    }

}
