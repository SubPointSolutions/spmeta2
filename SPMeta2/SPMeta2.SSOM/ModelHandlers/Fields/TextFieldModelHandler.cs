using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class TextFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(TextFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldText);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            field.ValidationMessage = fieldModel.ValidationMessage ?? string.Empty;
            field.ValidationFormula = fieldModel.ValidationFormula ?? string.Empty;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<TextFieldDefinition>("model", value => value.RequireNotNull());

            if (typedFieldModel.MaxLength.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.MaxLength, typedFieldModel.MaxLength.Value);
        }

        #endregion
    }
}
