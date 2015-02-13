using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using System.Xml.Linq;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class TextFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(TextFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject.Context.CastTo<FieldText>(spObject);
            var textDefinition = model.WithAssertAndCast<TextFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            if (textDefinition.MaxLength.HasValue)
                textFieldAssert.ShouldBeEqual(m => m.MaxLength, o => o.MaxLength);
            else
                textFieldAssert.SkipProperty(m => m.MaxLength, "MaxLength is NULL. Skipping.");
        }
    }
}
