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
    public class ExpectUpdateAsPublishingPageContentTypeService : ExpectUpdateGenericService<ExpectUpdateAsPublishingPageContentType>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInPublishingContentTypeId.ArticlePage);
            values.Add(BuiltInPublishingContentTypeId.EnterpriseWikiPage);
            values.Add(BuiltInPublishingContentTypeId.ErrorPage);
            values.Add(BuiltInPublishingContentTypeId.RedirectPage);

            if (prop.PropertyType == typeof(string))
                newValue = RndService.RandomFromArray(values);

            if (prop.PropertyType == typeof(Collection<string>))
            {
                newValue = RndService.RandomCollectionFromArray(values);
            }

            return newValue;
        }
    }
}
