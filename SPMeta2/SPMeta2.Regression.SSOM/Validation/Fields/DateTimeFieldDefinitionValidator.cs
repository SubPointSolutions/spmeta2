using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class DateTimeFieldDefinitionValidator : FieldDefinitionValidator
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

            var textField = spObject as SPFieldDateTime;
            var textDefinition = model.WithAssertAndCast<DateTimeFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            if (!string.IsNullOrEmpty(textDefinition.CalendarType))
                textFieldAssert.ShouldBeEqual(m => m.CalendarType, o => o.GetCalendarType());
            else
                textFieldAssert.SkipProperty(m => m.CalendarType);

            if (!string.IsNullOrEmpty(textDefinition.FriendlyDisplayFormat))
                textFieldAssert.ShouldBeEqual(m => m.FriendlyDisplayFormat, o => o.GetFriendlyDisplayFormat());
            else
                textFieldAssert.SkipProperty(m => m.FriendlyDisplayFormat);

            if (!string.IsNullOrEmpty(textDefinition.DisplayFormat))
                textFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormat());
            else
                textFieldAssert.SkipProperty(m => m.DisplayFormat);
        }
    }

    internal static class SPFieldDateTimeUtils
    {
        public static string GetCalendarType(this SPFieldDateTime field)
        {
            return field.CalendarType.ToString();
        }

        public static string GetFriendlyDisplayFormat(this SPFieldDateTime field)
        {
            return field.FriendlyDisplayFormat.ToString();
        }

        public static string GetDisplayFormat(this SPFieldDateTime field)
        {
            return field.DisplayFormat.ToString();
        }
    }
}
