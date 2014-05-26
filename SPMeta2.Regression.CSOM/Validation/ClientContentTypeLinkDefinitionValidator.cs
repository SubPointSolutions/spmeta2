using System.Diagnostics;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeLinkDefinitionValidator : ContentTypeLinkModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var context = list.Context;

            context.Load(list, l => l.ContentTypesEnabled);
            context.ExecuteQuery();

            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} ContentTypeLink:{1}", list, contentTypeLinkModel));

                var web = list.ParentWeb;

                context.Load(list, l => l.ContentTypesEnabled);
                context.ExecuteQuery();

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    if (list.ContentTypesEnabled)
                    {
                        context.Load(web, w => w.AvailableContentTypes);
                        context.Load(list, l => l.ContentTypes);

                        context.ExecuteQuery();

                        var listContentType = FindListContentType(list, contentTypeLinkModel);

                        // presence
                        trace.WriteLine(string.Format("Validate list content type presence (not null): model:[{0}] content type link:[{1}]", contentTypeLinkModel.ContentTypeId, listContentType));
                        Assert.IsNotNull(listContentType);

                        // child of
                        trace.WriteLine(string.Format("SSkipping checking ChildOf as in CSOM ct is referenced to list by name."));
                    }
                    else
                    {
                        trace.WriteLine("Skipping content type link check as List.ContentTypesEnabled is false");
                    }
                });
            });
        }
    }
}
