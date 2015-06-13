using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsInternalFieldNameService : ExpectUpdateGenericService<ExpectUpdateAsInternalFieldName>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInInternalFieldNames.ID);
            values.Add(BuiltInInternalFieldNames.Edit);
            values.Add(BuiltInInternalFieldNames.Created);
            values.Add(BuiltInInternalFieldNames._Author);

            if (prop.PropertyType == typeof(string))
                newValue = values[RndService.Int(values.Count - 1)];

            if (prop.PropertyType == typeof(Collection<string>))
            {
                newValue = RndService.RandomCollectionFromArray(values);
            }

            return newValue;
        }
    }
}
