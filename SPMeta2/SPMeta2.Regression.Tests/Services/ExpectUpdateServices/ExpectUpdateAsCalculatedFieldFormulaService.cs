using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsCalculatedFieldFormulaService : ExpectUpdateGenericService<ExpectUpdateAsCalculatedFieldFormula>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            newValue = string.Format("=ID*{0}", RndService.Int(100));

            return newValue;
        }
    }
}
