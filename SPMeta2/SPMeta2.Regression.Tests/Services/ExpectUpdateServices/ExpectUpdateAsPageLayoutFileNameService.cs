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
    public class ExpectUpdateAsPageLayoutFileNameService : ExpectUpdateGenericService<ExpectUpdateAsPageLayoutFileName>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            var values = new List<string>();

            values.Add(BuiltInPublishingPageLayoutNames.ArticleLeft);
            values.Add(BuiltInPublishingPageLayoutNames.ArticleLinks);
            values.Add(BuiltInPublishingPageLayoutNames.ArticleRight);
            values.Add(BuiltInPublishingPageLayoutNames.BlankWebPartPage);
            values.Add(BuiltInPublishingPageLayoutNames.CatalogArticle);
            values.Add(BuiltInPublishingPageLayoutNames.CatalogWelcome);
            values.Add(BuiltInPublishingPageLayoutNames.EnterpriseWiki);

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
