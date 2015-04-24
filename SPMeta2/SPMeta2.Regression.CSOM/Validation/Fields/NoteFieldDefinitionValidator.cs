using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using System.Xml.Linq;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class NoteFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(NoteFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var textField = spObject.Context.CastTo<FieldMultiLineText>(spObject);
            var textDefinition = model.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            textFieldAssert.ShouldBeEqual(m => m.NumberOfLines, o => o.NumberOfLines);
            textFieldAssert.ShouldBeEqual(m => m.RichText, o => o.RichText);
            textFieldAssert.ShouldBeEqual(m => m.AppendOnly, o => o.AppendOnly);

            textFieldAssert.ShouldBeEqual(m => m.RichText, o => o.GetRichText());
            //textFieldAssert.ShouldBeEqual(m => m.RichTextMode, o => o.GetRichTextMode());

            if (!string.IsNullOrEmpty(textDefinition.RichTextMode))
                textFieldAssert.ShouldBeEqual(m => m.RichTextMode, o => o.GetRichTextMode());
            else
                textFieldAssert.SkipProperty(m => m.RichTextMode, "RichTextMode is null or empty. Skipping.");

            textFieldAssert.ShouldBeEqual(m => m.UnlimitedLengthInDocumentLibrary, o => o.GetUnlimitedLengthInDocumentLibrary());
        }
    }
}
