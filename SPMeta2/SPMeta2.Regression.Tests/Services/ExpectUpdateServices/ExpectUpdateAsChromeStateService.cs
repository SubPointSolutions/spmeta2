using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsChromeStateService : ExpectUpdateGenericService<ExpectUpdateAsChromeState>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInPartChromeState.Minimized);
            values.Add(BuiltInPartChromeState.Normal);

            if (prop.PropertyType == typeof (string))
                newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
