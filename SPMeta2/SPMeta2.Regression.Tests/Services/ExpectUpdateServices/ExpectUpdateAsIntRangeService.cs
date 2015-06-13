using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsIntRangeService : ExpectUpdateGenericService<ExpectUpdateAsIntRange>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var typedAttr = attr as ExpectUpdateAsIntRange;

            var minValue = typedAttr.MinValue;
            var maxValue = typedAttr.MaxValue;

            var tmpValue = minValue + RndService.Int(maxValue - minValue);

            if (prop.PropertyType == typeof(double?) ||
                prop.PropertyType == typeof(double))
                newValue = Convert.ToDouble(tmpValue);
            else
            {
                // TODO, as per case
                newValue = tmpValue;
            }

            return newValue;
        }
    }
}
