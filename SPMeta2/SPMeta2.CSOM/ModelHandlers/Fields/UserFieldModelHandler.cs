using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class UserFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(FieldUser);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field.Context.CastTo<FieldUser>(field);
            var typedFieldModel = fieldModel.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());

            typedField.AllowDisplay = typedFieldModel.AllowDisplay;
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Mult, typedFieldModel.AllowMultipleValues.ToString().ToUpper());
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.ForcedDisplay, typedFieldModel.AllowDisplay.ToString().ToUpper());
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Presence, typedFieldModel.Presence.ToString().ToUpper());

            if (typedFieldModel.SelectionGroup.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.UserSelectionScope, typedFieldModel.SelectionGroup.ToString());

            if (!string.IsNullOrEmpty(typedFieldModel.SelectionMode))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.UserSelectionMode, typedFieldModel.SelectionMode);
        }

        #endregion
    }
}
