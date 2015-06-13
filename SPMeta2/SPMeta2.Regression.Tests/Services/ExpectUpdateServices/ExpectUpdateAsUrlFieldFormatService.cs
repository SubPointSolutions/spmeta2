using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsUrlFieldFormatService : ExpectUpdateGenericService<ExpectUpdateAsUrlFieldFormat>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var curentValue = prop.GetValue(obj) as string;

            if (curentValue == BuiltInUrlFieldFormatType.Hyperlink)
                newValue = BuiltInUrlFieldFormatType.Image;
            else
                newValue = BuiltInUrlFieldFormatType.Hyperlink;

            return newValue;
        }
    }
}
