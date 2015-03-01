using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class ComputedFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ComputedFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldComputed);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<ComputedFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as SPFieldComputed;

            if (typedFieldModel.EnableLookup.HasValue)
                typedField.EnableLookup = typedFieldModel.EnableLookup.Value;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<ComputedFieldDefinition>("model", value => value.RequireNotNull());

            if (typedFieldModel.EnableLookup.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.EnableLookup, typedFieldModel.EnableLookup.ToString().ToUpper());
        }

        #endregion
    }
}
