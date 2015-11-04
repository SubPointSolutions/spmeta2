using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdatAsToolbarTypeService : ExpectUpdateGenericService<ExpectUpdatAsToolbarType>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            newValue = RndService.RandomFromArray(new[]
            {
                BuiltInToolbarType.Freeform,
                BuiltInToolbarType.None,
                BuiltInToolbarType.Standard,
            });

            return newValue;
        }
    }
}
