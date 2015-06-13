using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsStandaloneService : ExpectUpdateGenericService<ExpectUpdateAsStandalone>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>
            {
                "Override", 
                "Standalone"
            };

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
