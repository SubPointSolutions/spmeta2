using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class URLFieldDefinitionValidator : ClientFieldDefinitionValidator
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

            var typedField = spObject.Context.CastTo<FieldUrl>(spObject);
            var typedDefinition = model.WithAssertAndCast<URLFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            if (!string.IsNullOrEmpty(typedDefinition.DisplayFormat))
                typedFieldAssert.ShouldBeEqual(m => m.DisplayFormat, o => o.GetDisplayFormat());
            else
                typedFieldAssert.SkipProperty(m => m.DisplayFormat, "DisplayFormat is null. Skiping.");
        }
    }

    internal static class SPFieldUrlExtensions
    {
        public static string GetDisplayFormat(this FieldUrl field)
        {
            return field.DisplayFormat.ToString();
        }
    }
}
