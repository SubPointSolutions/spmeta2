using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class DateTimeFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(DateTimeFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new DateTimeFieldDefinition
            {
                CalendarType = BuiltInCalendarType.Gregorian,
                DisplayFormat = BuiltInDateTimeFieldFormatType.DateOnly,
                FriendlyDisplayFormat = BuiltInDateTimeFieldFriendlyFormatType.Relative,
                ValidationMessage = string.Format("validatin_msg_{0}", Rnd.String()),
                ValidationFormula = string.Format("=[ID] * {0}", Rnd.Int(100))
            };
        }
    }
}
