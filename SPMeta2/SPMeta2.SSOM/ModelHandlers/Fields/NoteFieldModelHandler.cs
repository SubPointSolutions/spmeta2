using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class NoteFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(NoteFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldMultiLineText);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as SPFieldMultiLineText;

            typedField.NumberOfLines = typedFieldModel.NumberOfLines;
            typedField.AppendOnly = typedFieldModel.AppendOnly;
            typedField.RichText = typedFieldModel.RichText;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());

            if (typedFieldModel.NumberOfLines > 0)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.NumLines, typedFieldModel.NumberOfLines);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.RichText, typedFieldModel.RichText.ToString().ToUpper());
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.RichTextMode, typedFieldModel.RichTextMode);
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.AppendOnly, typedFieldModel.AppendOnly.ToString().ToUpper());
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.UnlimitedLengthInDocumentLibrary, typedFieldModel.UnlimitedLengthInDocumentLibrary.ToString().ToUpper());
        }

        #endregion
    }
}
