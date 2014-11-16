using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class UserFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(UserFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject as SPFieldUser;
            var textDefinition = model.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            typedFieldAssert.ShouldBeEqual(m => m.AllowMultipleValues, o => o.AllowMultipleValues);
            typedFieldAssert.ShouldBeEqual(m => m.AllowDisplay, o => o.AllowDisplay);
            typedFieldAssert.ShouldBeEqual(m => m.Presence, o => o.Presence);
            typedFieldAssert.ShouldBeEqual(m => m.SelectionMode, o => o.GetSelectionMode());

            if (textDefinition.SelectionGroup.HasValue)
            {
                typedFieldAssert.ShouldBeEqual(m => m.SelectionGroup, o => o.SelectionGroup);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.SelectionGroup, "SelectionGroup is NULL. Skipping.");
            }
        }
    }

    internal static class SPFieldUserUtils
    {
        public static string GetSelectionMode(this SPFieldUser field)
        {
            return field.SelectionMode.ToString();
        }
    }
}
