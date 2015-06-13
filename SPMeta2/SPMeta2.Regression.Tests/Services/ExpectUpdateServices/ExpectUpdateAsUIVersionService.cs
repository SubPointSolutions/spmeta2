using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsUIVersionService : ExpectUpdateGenericService<ExpectUpdateAsUIVersion>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add("4");
            values.Add("15");

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            if (prop.PropertyType == typeof(Collection<string>))
            {
                newValue = RndService.RandomCollectionFromArray(values); ;
            }

            if (prop.PropertyType == typeof(List<string>))
            {
                newValue = RndService.RandomListFromArray(values);
            }

            return newValue;
        }
    }
}
