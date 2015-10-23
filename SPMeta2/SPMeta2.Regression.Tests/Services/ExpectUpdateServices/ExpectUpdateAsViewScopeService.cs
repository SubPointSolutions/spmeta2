using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsViewScopeService : ExpectUpdateGenericService<ExpectUpdateAsViewScope>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var list = new List<string>(new[]
            {
                BuiltInViewScope.Default,
                BuiltInViewScope.FilesOnly,
                BuiltInViewScope.Recursive,
                BuiltInViewScope.RecursiveAll
            });

            newValue = RndService.RandomFromArray(list);

            return newValue;
        }
    }
}
