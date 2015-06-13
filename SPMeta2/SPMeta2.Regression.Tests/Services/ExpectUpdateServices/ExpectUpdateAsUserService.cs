using System.Reflection;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsUserService : ExpectUpdateGenericService<ExpectUpdateAsUser>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            return RndService.UserLogin();
        }
    }
}
