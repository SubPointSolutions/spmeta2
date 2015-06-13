using System.Reflection;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsLCIDService : ExpectUpdateGenericService<ExpectUpdateAsLCID>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var newLocaleIdValue = 1033 + RndService.Int(5);

            if (prop.PropertyType == typeof(int))
                newValue = newLocaleIdValue;
            else if (prop.PropertyType == typeof(int?))
                newValue = RndService.Bool() ? (int?)null : newLocaleIdValue;
            else if (prop.PropertyType == typeof(uint))
                newValue = (uint)newLocaleIdValue;
            else if (prop.PropertyType == typeof(uint?))
                newValue = (uint?)(RndService.Bool() ? (uint?)null : (uint?)newLocaleIdValue);

            return newValue;
        }
    }
}
