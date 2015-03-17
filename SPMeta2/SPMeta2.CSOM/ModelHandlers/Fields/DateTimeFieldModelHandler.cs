using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class DateTimeFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DateTimeFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(FieldDateTime);
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

            var typedFieldModel = fieldModel.WithAssertAndCast<DateTimeFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field.Context.CastTo<FieldDateTime>(field);

            if (!string.IsNullOrEmpty(typedFieldModel.CalendarType))
                typedField.DateTimeCalendarType = (CalendarType)Enum.Parse(typeof(CalendarType), typedFieldModel.CalendarType);

            if (!string.IsNullOrEmpty(typedFieldModel.DisplayFormat))
                typedField.DisplayFormat = (DateTimeFieldFormatType)Enum.Parse(typeof(DateTimeFieldFormatType), typedFieldModel.DisplayFormat);

            if (!string.IsNullOrEmpty(typedFieldModel.FriendlyDisplayFormat))
                typedField.FriendlyDisplayFormat = (DateTimeFieldFriendlyFormatType)Enum.Parse(typeof(DateTimeFieldFriendlyFormatType), typedFieldModel.FriendlyDisplayFormat);
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<DateTimeFieldDefinition>("model", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedFieldModel.CalendarType))
            {
                var value = (CalendarType)Enum.Parse(typeof(CalendarType), typedFieldModel.CalendarType);
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.CalType, (int)value);
            }

            if (!string.IsNullOrEmpty(typedFieldModel.DisplayFormat))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Format, typedFieldModel.DisplayFormat);

            if (!string.IsNullOrEmpty(typedFieldModel.FriendlyDisplayFormat))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.FriendlyDisplayFormat, typedFieldModel.FriendlyDisplayFormat);
        }

        #endregion
    }
}
