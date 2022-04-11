using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsNumberFieldDisplayFormatService : ExpectUpdateGenericService<ExpectUpdateAsNumberFieldDisplayFormat>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInNumberFormatTypes.Automatic);

            values.Add(BuiltInNumberFormatTypes.FiveDecimals);
            values.Add(BuiltInNumberFormatTypes.FourDecimals);
            values.Add(BuiltInNumberFormatTypes.ThreeDecimals);
            values.Add(BuiltInNumberFormatTypes.TwoDecimals);
            values.Add(BuiltInNumberFormatTypes.OneDecimal);
            values.Add(BuiltInNumberFormatTypes.NoDecimal);

            newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
