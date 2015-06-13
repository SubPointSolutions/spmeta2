using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAssCalculatedFieldReferencesService : ExpectUpdateGenericService<ExpectUpdateAssCalculatedFieldReferences>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInInternalFieldNames.ID);
            values.Add(BuiltInInternalFieldNames.FileRef);
            values.Add(BuiltInInternalFieldNames.FileType);
            values.Add(BuiltInInternalFieldNames.File_x0020_Size);
            values.Add(BuiltInInternalFieldNames.FirstName);

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            if (prop.PropertyType == typeof(Collection<string>))
            {
                newValue = RndService.RandomCollectionFromArray(values);
            }

            return newValue;
        }
    }
}
