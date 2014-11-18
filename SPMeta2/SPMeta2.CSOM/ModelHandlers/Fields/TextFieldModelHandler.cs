using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers.Fields
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
            return typeof(FieldText);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            field.ValidationMessage = fieldModel.ValidationMessage ?? string.Empty;
            field.ValidationFormula = fieldModel.ValidationFormula ?? string.Empty;

            var tmpField = field.Context.CastTo<FieldText>(field);

            var spField = tmpField.WithAssertAndCast<FieldText>("field", value => value.RequireNotNull());
            var typedFieldModel = fieldModel.WithAssertAndCast<TextFieldDefinition>("model", value => value.RequireNotNull());

        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<TextFieldDefinition>("model", value => value.RequireNotNull());

            if (typedFieldModel.MaxLength.HasValue)
                fieldTemplate
                 .SetAttribute(BuiltInFieldAttributes.MaxLength, typedFieldModel.MaxLength.Value);
        }

        #endregion
    }
}
