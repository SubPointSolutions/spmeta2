using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeLinkDefinitionValidator : ContentTypeLinkModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var spList = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} ContentTypeLink:{1}", spList, contentTypeLinkModel));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    if (spList.ContentTypesEnabled)
                    {
                        var contentTypeId = new SPContentTypeId(contentTypeLinkModel.ContentTypeId);
                        var targetContentType = spList.ParentWeb.ContentTypes[contentTypeId];

                        var listContentType = spList.ContentTypes[targetContentType.Name];

                        // presence
                        trace.WriteLine(string.Format("Validate list content type presence (not null): model:[{0}] content type link:[{1}]", contentTypeLinkModel.ContentTypeId, listContentType));
                        Assert.IsNotNull(listContentType);

                        // child of
                        trace.WriteLine(string.Format("Validate ChildOf: model:[{0}] content type link:[{1}]", contentTypeLinkModel.ContentTypeId, listContentType));
                        Assert.IsTrue(listContentType.Id.IsChildOf(new SPContentTypeId(contentTypeLinkModel.ContentTypeId)));
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
