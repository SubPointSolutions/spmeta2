using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsDateTimeFieldCalendarTypeService : ExpectUpdateGenericService<ExpectUpdateAsDateTimeFieldCalendarType>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInCalendarType.Gregorian);
            values.Add(BuiltInCalendarType.Korea);
            values.Add(BuiltInCalendarType.Hebrew);
            values.Add(BuiltInCalendarType.GregorianArabic);
            values.Add(BuiltInCalendarType.SakaEra);

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
