using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsCamlQueryService : ExpectUpdateGenericService<ExpectUpdateAsCamlQuery>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            newValue =
                    string.Format(
                        "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>",
                        RndService.String());

            return newValue;
        }
    }
}
