using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class URLFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(URLFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var typedField = spObject as SPFieldUrl;
            var typedDefinition = model.WithAssertAndCast<URLFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            typedFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormat());

        }
    }

    internal static class SPFieldUrlExtensions
    {
        public static string GetDisplayFormat(this SPFieldUrl field)
        {
            return field.DisplayFormat.ToString();
        }
    }
}
