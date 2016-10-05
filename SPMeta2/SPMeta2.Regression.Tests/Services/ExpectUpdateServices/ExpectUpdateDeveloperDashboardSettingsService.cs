using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using System.Collections.Generic;
using SPMeta2.Containers.Services;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateDeveloperDashboardSettingsService : ExpectUpdateGenericService<ExpectUpdateDeveloperDashboardSettings>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInDeveloperDashboardLevel.Off);
            values.Add(BuiltInDeveloperDashboardLevel.On);
            values.Add(BuiltInDeveloperDashboardLevel.OnDemand);

            newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
