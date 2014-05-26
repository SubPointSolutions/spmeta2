using System;
using System.Diagnostics;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        protected FieldLink FindFieldLinkById(FieldLinkCollection fiedLinks, Guid fieldId)
        {
            foreach (var fieldLink in fiedLinks)
            {
                if (fieldLink.Id == fieldId)
                    return fieldLink;
            }

            return null;
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var modelHostContext = modelHost.WithAssertAndCast<ModelHostContext>("modelHost", value => value.RequireNotNull());
            var fieldlinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var site = modelHostContext.Site;
            var contentType = modelHostContext.ContentType;

            var context = site.Context;

            context.Load(contentType, ct => ct.FieldLinks);
            context.ExecuteQuery();

            var spFieldLink = FindFieldLinkById(contentType.FieldLinks, fieldlinkModel.FieldId);

            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} ContentType:{1}", fieldlinkModel, contentType));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate FieldId: model:[{0}] ct field link:[{1}]", fieldlinkModel.FieldId, spFieldLink.Id));
                    Assert.AreEqual(fieldlinkModel.FieldId, spFieldLink.Id);
                });
            });
        }
    }
}
