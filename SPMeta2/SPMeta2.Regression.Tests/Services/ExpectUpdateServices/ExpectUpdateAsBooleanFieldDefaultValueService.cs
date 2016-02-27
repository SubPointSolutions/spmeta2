using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsBooleanFieldDefaultValueService : ExpectUpdateGenericService<ExpectUpdateAsBooleanFieldDefaultValue>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = RndService.Bool() ? "1" : "0";

            return newValue;
        }
    }
}
