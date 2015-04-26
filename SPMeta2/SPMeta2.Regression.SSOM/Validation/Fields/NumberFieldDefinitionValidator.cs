using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class NumberFieldDefinitionValidator : FieldDefinitionValidator
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

            var textField = spObject as SPFieldNumber;
            var textDefinition = model.WithAssertAndCast<NumberFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            typedFieldAssert.ShouldBeEqual(m => m.MaximumValue, o => o.MaximumValue);
            typedFieldAssert.ShouldBeEqual(m => m.MinimumValue, o => o.MinimumValue);
            typedFieldAssert.ShouldBeEqual(m => m.ShowAsPercentage, o => o.ShowAsPercentage);


            if (!string.IsNullOrEmpty(textDefinition.DisplayFormat))
                typedFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormat());
            else
                typedFieldAssert.SkipProperty(m => m.DisplayFormat);


        }


    }

    internal static class SPFieldNumberUtils
    {
        public static string GetDisplayFormat(this SPFieldNumber field)
        {
            return field.DisplayFormat.ToString();
        }
    }

}
