using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsChromeTypeService : ExpectUpdateGenericService<ExpectUpdateAsChromeType>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInPartChromeType.BorderOnly);
            values.Add(BuiltInPartChromeType.Default);
            values.Add(BuiltInPartChromeType.None);
            values.Add(BuiltInPartChromeType.TitleAndBorder);
            values.Add(BuiltInPartChromeType.TitleOnly);

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
