using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsBasePermissionService : ExpectUpdateGenericService<ExpectUpdateAsBasePermission>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInBasePermissions.AddAndCustomizePages);
            values.Add(BuiltInBasePermissions.AnonymousSearchAccessWebLists);
            values.Add(BuiltInBasePermissions.ApproveItems);
            values.Add(BuiltInBasePermissions.CancelCheckout);
            values.Add(BuiltInBasePermissions.CreateSSCSite);
            values.Add(BuiltInBasePermissions.EditMyUserInfo);

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
