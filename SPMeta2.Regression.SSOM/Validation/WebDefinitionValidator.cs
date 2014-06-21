using System;
using System.Linq.Expressions;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebDefinitionValidator : WebModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var webModel = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());
            SPWeb parentWeb = null;

            if (modelHost is SiteModelHost)
                parentWeb = (modelHost as SiteModelHost).HostSite.RootWeb;

            if (modelHost is WebModelHost)
                parentWeb = (modelHost as WebModelHost).HostWeb;

            TraceUtils.WithScope(traceScope =>
            {
                var web = GetWeb(parentWeb, webModel);
                var pair = new ComparePair<WebDefinition, SPWeb>(webModel, web);

                traceScope.WriteLine(string.Format("Validating model:[{0}] web:[{1}]", webModel, web));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, w => w.Title)
                    .ShouldBeEqual(trace, m => m.Description, w => w.Description)
                    .ShouldBeEqual(trace, m => m.WebTemplate, w => string.Format("{0}#{1}", w.WebTemplate, w.Configuration)));
            });
        }
    }
}
