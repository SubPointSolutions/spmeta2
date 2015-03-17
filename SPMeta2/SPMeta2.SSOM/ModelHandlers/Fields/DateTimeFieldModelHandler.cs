using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Fields
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
            return typeof(SPFieldDateTime);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            if (!string.IsNullOrEmpty(fieldModel.ValidationMessage))
                field.ValidationMessage = fieldModel.ValidationMessage;

            if (!string.IsNullOrEmpty(fieldModel.ValidationFormula))
                field.ValidationFormula = fieldModel.ValidationFormula;


            var typedFieldModel = fieldModel.WithAssertAndCast<DateTimeFieldDefinition>("model", value => value.RequireNotNull());
            var typedField = field as SPFieldDateTime;

            if (!string.IsNullOrEmpty(typedFieldModel.CalendarType))
                typedField.CalendarType = (SPCalendarType)Enum.Parse(typeof(SPCalendarType), typedFieldModel.CalendarType);

            if (!string.IsNullOrEmpty(typedFieldModel.DisplayFormat))
                typedField.DisplayFormat = (SPDateTimeFieldFormatType)Enum.Parse(typeof(SPDateTimeFieldFormatType), typedFieldModel.DisplayFormat);

#if !NET35
            if (!string.IsNullOrEmpty(typedFieldModel.FriendlyDisplayFormat))
                typedField.FriendlyDisplayFormat = (SPDateTimeFieldFriendlyFormatType)Enum.Parse(typeof(SPDateTimeFieldFriendlyFormatType), typedFieldModel.FriendlyDisplayFormat);
#endif
        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<DateTimeFieldDefinition>("model", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedFieldModel.CalendarType))
            {
                var value = (SPCalendarType)Enum.Parse(typeof(SPCalendarType), typedFieldModel.CalendarType);
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.CalType, (int)value);
            }

            if (!string.IsNullOrEmpty(typedFieldModel.DisplayFormat))
            {
                var value = (SPDateTimeFieldFormatType)Enum.Parse(typeof(SPDateTimeFieldFormatType), typedFieldModel.DisplayFormat);
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Format, value);
            }

#if !NET35
            if (!string.IsNullOrEmpty(typedFieldModel.FriendlyDisplayFormat))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.FriendlyDisplayFormat, typedFieldModel.FriendlyDisplayFormat);
#endif
        }

        #endregion
    }
}
