using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsWebPartPageLayoutTemplateService : ExpectUpdateGenericService<ExpectUpdateAsWebPartPageLayoutTemplate>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<int>();

            values.Add(BuiltInWebpartPageTemplateId.spstd1);
            values.Add(BuiltInWebpartPageTemplateId.spstd2);
            values.Add(BuiltInWebpartPageTemplateId.spstd3);
            values.Add(BuiltInWebpartPageTemplateId.spstd4);
            values.Add(BuiltInWebpartPageTemplateId.spstd5);
            values.Add(BuiltInWebpartPageTemplateId.spstd6);
            values.Add(BuiltInWebpartPageTemplateId.spstd7);

            if (prop.PropertyType == typeof (int))
                newValue = RndService.RandomFromArray(values);

            return newValue;
        }
    }
}
