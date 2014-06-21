using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ContentTypeDefinitionValidator : ContentTypeModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definitionModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var rootWeb = site.RootWeb;

            var contentTypes = rootWeb.AvailableContentTypes;

            var spModel = contentTypes[definitionModel.Name];

            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} ContentType:{1}", definitionModel, spModel));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate Name: model:[{0}] ct:[{1}]", definitionModel.Name, spModel.Name));
                    Assert.AreEqual(definitionModel.Name, spModel.Name);

                    trace.WriteLine(string.Format("Validate Description: model:[{0}] ct:[{1}]", definitionModel.Description, spModel.Description));
                    Assert.AreEqual(definitionModel.Description, spModel.Description);

                    trace.WriteLine(string.Format("Validate Id: model:[{0}] ct:[{1}]", definitionModel.GetContentTypeId(), spModel.Id));
                    Assert.AreEqual(new SPContentTypeId(definitionModel.GetContentTypeId()), spModel.Id);

                    trace.WriteLine(string.Format("Validate Group: model:[{0}] ct:[{1}]", definitionModel.Group, spModel.Group));
                    Assert.AreEqual(definitionModel.Group, spModel.Group);
                });
            });
        }
    }
}
