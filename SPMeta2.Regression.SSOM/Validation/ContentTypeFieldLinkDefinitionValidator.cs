using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var spModel = modelHost.WithAssertAndCast<SPContentType>("modelHost", value => value.RequireNotNull());
            var fieldlinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var spFieldLink = spModel.FieldLinks[fieldlinkModel.FieldId];

            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} ContentType:{1}", fieldlinkModel, spModel));

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
