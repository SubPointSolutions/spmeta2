using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsUrlService : ExpectUpdateGenericService<ExpectUpdateAsUrl>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var typedAttr = attr as ExpectUpdateAsUrl;
            var fileExtension = typedAttr.Extension;

            if (!fileExtension.StartsWith("."))
                fileExtension = "." + fileExtension;

            newValue = string.Format("http://regression-ci.com/{0}{1}",
                RndService.String(), fileExtension);

            return newValue;
        }
    }
}
