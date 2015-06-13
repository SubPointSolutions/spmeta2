using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsCompatibleSearchDataTypeService : ExpectUpdateGenericService<ExpectUpdateAsCompatibleSearchDataTypes>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInCompatibleSearchDataTypes.Integer);
            values.Add(BuiltInCompatibleSearchDataTypes.DateTime);
            values.Add(BuiltInCompatibleSearchDataTypes.Decimal);
            values.Add(BuiltInCompatibleSearchDataTypes.Text);
            values.Add(BuiltInCompatibleSearchDataTypes.YesNo);

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            if (prop.PropertyType == typeof(List<string>))
            {
                newValue = RndService.RandomArrayFromArray(values).ToList();
            }

            return newValue;
        }
    }
}
