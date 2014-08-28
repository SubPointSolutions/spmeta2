using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class PublishingPageDefinitionValidator : PublishingPageModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            ValidatePublishingPage(modelHost, list, publishingPageModel);


        }

        private void ValidatePublishingPage(object modelHost, SPList list, PublishingPageDefinition publishingPageModel)
        {
            //TraceUtils.WithScope(traceScope =>
            //       {
            //           traceScope.WriteLine(string.Format("Validate model:[{0}] web part page:[{1}]", webpartPageModel, spWebPartPage));

            //           // asserting it exists
            //           traceScope.WriteLine(string.Format("Validating existance..."));
            //           Assert.IsNotNull(spWebPartPage);

            //           traceScope.WriteLine(string.Format("Web part page exists!"));

            //           // assert base properties
            //           traceScope.WithTraceIndent(trace =>
            //           {
            //               var originalFileName = GetSafeWebPartPageFileName(webpartPageModel);

            //               trace.WriteLine(string.Format("Validate Name: model:[{0}] field:[{1}]", originalFileName, spWebPartPage.Name));
            //               Assert.AreEqual(originalFileName, spWebPartPage.Name);
            //           });
            //       });
        }
    }
}
