using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsFieldUserSelectionModeService : ExpectUpdateGenericService<ExpectUpdateAsFieldUserSelectionMode>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var curentValue = prop.GetValue(obj) as string;

            if (curentValue == BuiltInFieldUserSelectionMode.PeopleAndGroups)
                newValue = BuiltInFieldUserSelectionMode.PeopleOnly;
            else
                newValue = BuiltInFieldUserSelectionMode.PeopleAndGroups;

            return newValue;
        }
    }
}
