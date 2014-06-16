using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeDefinitionValidator : ContentTypeModelHandler
    {
        protected ContentType FindByName(ContentTypeCollection contentTypes, string name)
        {
            foreach (var ct in contentTypes)
            {
                if (String.Compare(ct.Name, name, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return ct;
            }

            return null;
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;

            var context = site.Context;
            var rootWeb = site.RootWeb;

            var contentTypes = rootWeb.ContentTypes;

            context.Load(rootWeb);
            context.Load(contentTypes);

            context.ExecuteQuery();

            var contentTypeId = contentTypeModel.GetContentTypeId();
            var spObject = contentTypes.FirstOrDefault(c => c.StringId.ToLower() == contentTypeId.ToLower());

            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<ContentTypeDefinition, ContentType>(contentTypeModel, spObject);

                traceScope.WriteLine(string.Format("Validating model:[{0}] field:[{1}]", model, spObject));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Name, o => o.Name)
                    .ShouldBeEqual(trace, m => m.Description, o => o.Description)
                    .ShouldBeEqual(trace, m => m.Group, o => o.Group)
                    .ShouldBeEqual(trace, m => m.GetContentTypeId().ToLowerInvariant(), o => o.Id.ToString().ToLowerInvariant())
                    .ShouldBeEqual(trace, m => m.Group, o => o.Group));
            });
        }
    }
}
