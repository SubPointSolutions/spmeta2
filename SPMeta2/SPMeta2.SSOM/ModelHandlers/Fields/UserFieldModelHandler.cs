using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
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
            return typeof(SPFieldUser);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field as SPFieldUser;
            var typedFieldModel = fieldModel.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());

            typedField.AllowDisplay = typedFieldModel.AllowDisplay;
            typedField.Presence = typedFieldModel.Presence;
            typedField.AllowMultipleValues = typedFieldModel.AllowMultipleValues;

            if (!string.IsNullOrEmpty(typedFieldModel.SelectionMode))
                typedField.SelectionMode = (SPFieldUserSelectionMode)Enum.Parse(typeof(SPFieldUserSelectionMode), typedFieldModel.SelectionMode);

            if (typedFieldModel.SelectionGroup.HasValue)
            {
                typedField.SelectionGroup = typedFieldModel.SelectionGroup.Value;
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.SelectionGroupName))
            {
                var group = GetCurrentWeb().SiteGroups.OfType<SPGroup>().FirstOrDefault(g => g.Name.ToUpper() == typedFieldModel.SelectionGroupName.ToUpper());
                typedField.SelectionGroup = group.ID;
            }
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
