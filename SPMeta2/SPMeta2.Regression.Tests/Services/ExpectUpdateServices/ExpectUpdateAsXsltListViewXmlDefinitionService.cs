using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsXsltListViewXmlDefinitionService : ExpectUpdateGenericService<ExpectUpdateAsXsltListViewXmlDefinition>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            newValue = string.Format("<View BaseViewID='{0}'/>", RndService.Int(100) + 1);

            return newValue;
        }
    }
}
