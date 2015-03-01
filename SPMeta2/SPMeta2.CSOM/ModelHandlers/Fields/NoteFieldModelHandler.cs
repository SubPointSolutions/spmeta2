using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers.Fields
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
            return typeof(FieldMultiLineText);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field.Context.CastTo<FieldMultiLineText>(field);

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
