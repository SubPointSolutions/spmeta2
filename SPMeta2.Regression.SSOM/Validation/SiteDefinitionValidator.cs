using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;

namespace SPMeta2.Regression.Validation.ServerModelHandlers
{
    public class SiteDefinitionValidator : SiteModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var site = modelHost as SPSite;

            if (site == null) throw new ArgumentException("modelHost");
            var rootWeb = site.RootWeb;

            var siteModel = model as SiteDefinition;
        }
    }
}
