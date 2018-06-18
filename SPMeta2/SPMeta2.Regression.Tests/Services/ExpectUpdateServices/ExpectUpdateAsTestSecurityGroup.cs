using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsTestSecurityGroupService : ExpectUpdateGenericService<ExpectUpdateAsTestSecurityGroup>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            return RndService.RandomFromArray(new[] {
                 "SPMeta2 Test Group 1",
                 "SPMeta2 Test Group 2",
                 "SPMeta2 Test Group 3",
                 "SPMeta2 Test Group 4",
                 "SPMeta2 Test Group 5"
             });
        }
    }
}
