using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsDateFormatService : ExpectUpdateGenericService<ExpectUpdateAsDateFormat>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var curentValue = prop.GetValue(obj) as string;

            if (curentValue == BuiltInDateTimeFieldFormatType.DateOnly)
            {
                newValue = BuiltInDateTimeFieldFormatType.DateTime;
            }
            if (curentValue == BuiltInDateTimeFieldFormatType.DateTime)
            {
                newValue = BuiltInDateTimeFieldFormatType.DateOnly;
            }
            else
            {
                newValue = RndService.RandomFromArray(new string[]{
                    BuiltInDateTimeFieldFormatType.DateOnly,
                    BuiltInDateTimeFieldFormatType.DateTime
                });
            }

            return newValue;
        }
    }
}
