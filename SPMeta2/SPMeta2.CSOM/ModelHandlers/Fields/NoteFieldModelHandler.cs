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

        protected override bool PreloadProperties(Field field)
        {
            base.PreloadProperties(field);

            var context = field.Context;

            context.Load(field, f => f.SchemaXml);

            return true;
        }


        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            var typedFieldModel = fieldModel.WithAssertAndCast<NoteFieldDefinition>("model", value => value.RequireNotNull());

            // the XML update goes first
            // then the rest of the normal props with base.ProcessFieldProperties(field, fieldModel);
            // then specific to NoteField props
            // as crazy as it sounds

            // RichTextMode  update
            // https://github.com/SubPointSolutions/spmeta2/issues/673
            if (!string.IsNullOrEmpty(typedFieldModel.RichTextMode))
            {
                var fieldXml = XDocument.Parse(field.SchemaXml);
                fieldXml.Root.SetAttribute("RichTextMode", typedFieldModel.RichTextMode);

                field.SchemaXml = fieldXml.ToString();
            }

            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

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
