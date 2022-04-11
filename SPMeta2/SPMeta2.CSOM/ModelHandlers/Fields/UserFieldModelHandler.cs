﻿using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;

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
            var typedFieldModel = fieldModel.WithAssertAndCast<UserFieldDefinition>("model", value => value.RequireNotNull());
            int? groupId = null;

            var web = HostWeb;
            var context = web.Context;

            if (!string.IsNullOrEmpty(typedFieldModel.SelectionGroupName))
            {
#if !NET35

                var group = web.SiteGroups.GetByName(typedFieldModel.SelectionGroupName);
                context.Load(group);
                context.Load(group, g => g.Id);
                context.ExecuteQueryWithTrace();

                groupId = group.Id;

#endif

#if NET35

                // TODO, https://github.com/SubPointSolutions/spmeta2/issues/762
                throw new SPMeta2NotImplementedException("SelectionGroupName is not implemented yet for SP2010 - https://github.com/SubPointSolutions/spmeta2/issues/762");
#endif

            }

            // lokup the group
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field.Context.CastTo<FieldUser>(field);

            // let base setting be setup
            typedField.AllowDisplay = typedFieldModel.AllowDisplay;
            typedField.AllowMultipleValues = typedFieldModel.AllowMultipleValues;
            typedField.Presence = typedFieldModel.Presence;

            if (!string.IsNullOrEmpty(typedFieldModel.SelectionMode))
                typedField.SelectionMode = (FieldUserSelectionMode)Enum.Parse(typeof(FieldUserSelectionMode), typedFieldModel.SelectionMode);

            if (typedFieldModel.SelectionGroup.HasValue)
            {
                typedField.SelectionGroup = typedFieldModel.SelectionGroup.Value;
            }
            else if (!string.IsNullOrEmpty(typedFieldModel.SelectionGroupName))
            {
                typedField.SelectionGroup = groupId.Value;
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
            {
                typedField.LookupField = typedFieldModel.LookupField;
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

            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowField, typedFieldModel.LookupField);
        }

        #endregion
    }
}
