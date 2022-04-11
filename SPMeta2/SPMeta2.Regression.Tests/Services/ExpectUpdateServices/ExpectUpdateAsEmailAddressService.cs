using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsEmailAddressService : ExpectUpdateGenericService<ExpectUpdateAsEmailAddress>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = string.Format("{0}@m2-regression-test.com", RndService.String());

            return newValue;
        }
    }
}
