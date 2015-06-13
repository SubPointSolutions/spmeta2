using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsDateTimeFieldFriendlyDisplayFormatService : ExpectUpdateGenericService<ExpectUpdateAsDateTimeFieldFriendlyDisplayFormat>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var curentValue = prop.GetValue(obj) as string;

            if (curentValue == BuiltInDateTimeFieldFriendlyFormatType.Disabled)
                newValue = BuiltInDateTimeFieldFriendlyFormatType.Relative;
            else if (curentValue == BuiltInDateTimeFieldFriendlyFormatType.Relative)
                newValue = BuiltInDateTimeFieldFriendlyFormatType.Unspecified;
            else if (curentValue == BuiltInDateTimeFieldFriendlyFormatType.Unspecified)
                newValue = BuiltInDateTimeFieldFriendlyFormatType.Disabled;

            return newValue;
        }
    }
}
