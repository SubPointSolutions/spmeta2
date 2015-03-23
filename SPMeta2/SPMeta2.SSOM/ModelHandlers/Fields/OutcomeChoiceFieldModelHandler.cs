using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class OutcomeChoiceFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(OutcomeChoiceFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(OutcomeChoiceField);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<OutcomeChoiceFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as OutcomeChoiceField;

        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<OutcomeChoiceFieldDefinition>("model", value => value.RequireNotNull());
        }

        #endregion
    }
}
