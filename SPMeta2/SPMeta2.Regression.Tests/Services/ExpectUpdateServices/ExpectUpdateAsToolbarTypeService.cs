using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsToolbarTypeService : ExpectUpdateGenericService<ExpectUpdatAsToolbarType>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var value = prop.GetValue(obj) as string;
            var values = new List<string>(new[]
            {
                //BuiltInToolbarType.Freeform,
                BuiltInToolbarType.None,
                BuiltInToolbarType.Standard,
            });

            values.Remove(value);

            newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
