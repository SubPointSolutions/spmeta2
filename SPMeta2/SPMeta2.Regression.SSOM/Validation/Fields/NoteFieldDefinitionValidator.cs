using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class NoteFieldDefinitionValidator : FieldDefinitionValidator
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

            var textField = spObject as SPFieldMultiLineText;
            var textDefinition = model.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            textFieldAssert.ShouldBeEqual(m => m.NumberOfLines, o => o.NumberOfLines);
            textFieldAssert.ShouldBeEqual(m => m.RichText, o => o.RichText);
            textFieldAssert.ShouldBeEqual(m => m.AppendOnly, o => o.AppendOnly);

            textFieldAssert.ShouldBeEqual(m => m.RichText, o => o.RichText);
            textFieldAssert.ShouldBeEqual(m => m.UnlimitedLengthInDocumentLibrary, o => o.UnlimitedLengthInDocumentLibrary);

            if (!string.IsNullOrEmpty(textDefinition.RichTextMode))
                textFieldAssert.ShouldBeEqual(m => m.RichTextMode, o => o.GetRichTextMode());
            else
                textFieldAssert.SkipProperty(m => m.RichTextMode);
        }
    }

    internal static class SPFieldMultiLineTextUtil
    {
        public static string GetRichTextMode(this SPFieldMultiLineText field)
        {
            return field.RichTextMode.ToString();
        }
    }
}
