using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class DateTimeFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(DateTimeFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject.Context.CastTo<FieldDateTime>(spObject);
            var textDefinition = model.WithAssertAndCast<DateTimeFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            if (!string.IsNullOrEmpty(textDefinition.CalendarType))
                textFieldAssert.ShouldBeEqual(m => m.CalendarType, o => o.GetCalendarType());
            else
                textFieldAssert.SkipProperty(m => m.CalendarType, "CalendarType is null or empty. Skipping.");

            if (!string.IsNullOrEmpty(textDefinition.FriendlyDisplayFormat))
                textFieldAssert.ShouldBeEqual(m => m.FriendlyDisplayFormat, o => o.GetFriendlyDisplayFormat());
            else
                textFieldAssert.SkipProperty(m => m.FriendlyDisplayFormat, "FriendlyDisplayFormat is null or empty. Skipping.");

            if (!string.IsNullOrEmpty(textDefinition.DisplayFormat))
                textFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormat());
            else
                textFieldAssert.SkipProperty(m => m.DisplayFormat, "FriendlyDisplayFormat is null or empty. Skipping.");
            
            
        }
    }
}
