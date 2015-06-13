using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsTargetControlTypeService : ExpectUpdateGenericService<ExpectUpdateAsTargetControlType>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInTargetControlType.ContentWebParts);
            values.Add(BuiltInTargetControlType.Custom);
            values.Add(BuiltInTargetControlType.Refinement);
            values.Add(BuiltInTargetControlType.SearchBox);
            values.Add(BuiltInTargetControlType.SearchHoverPanel);
            values.Add(BuiltInTargetControlType.SearchResults);

            if (prop.PropertyType == typeof (string))
                newValue = RndService.RandomFromArray(values);

            if (prop.PropertyType == typeof(List<string>))
            {
                newValue = RndService.RandomArrayFromArray(values).ToList();
            }

            return newValue;
        }
    }
}
