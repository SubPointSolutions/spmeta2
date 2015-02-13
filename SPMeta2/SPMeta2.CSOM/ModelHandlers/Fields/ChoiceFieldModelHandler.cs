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
    public class ChoiceFieldModelHandler : MultiChoiceFieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ChoiceFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(FieldChoice);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            if (!string.IsNullOrEmpty(fieldModel.ValidationMessage))
                field.ValidationMessage = fieldModel.ValidationMessage;

            if (!string.IsNullOrEmpty(fieldModel.ValidationFormula))
                field.ValidationFormula = fieldModel.ValidationFormula;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<ChoiceFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Format, typedFieldModel.EditFormat);
        }

        #endregion
    }
}
