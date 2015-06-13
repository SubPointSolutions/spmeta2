using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsByteService : ExpectUpdateGenericService<ExpectUpdateAsByte>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            if (prop.PropertyType == typeof(int?) ||
                    prop.PropertyType == typeof(int?))
                newValue = Convert.ToInt32(RndService.Byte().ToString());
            else
            {
                // TODO, as per case
                newValue = RndService.Byte();
            }

            return newValue;
        }
    }
}
