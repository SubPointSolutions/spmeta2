using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WikiPageDefinitionValidator : WikiPageModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            var spWikiPageItem = FindWikiPage(list, wikiPageModel);

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] wiki page:[{1}]", wikiPageModel, spWikiPageItem));

                // asserting it exists
                traceScope.WriteLine(string.Format("Validating existance..."));
                Assert.IsNotNull(spWikiPageItem);

                traceScope.WriteLine(string.Format("Wiki page exists!"));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    var originalFileName = GetSafeWikiPageFileName(wikiPageModel);

                    trace.WriteLine(string.Format("Validate Name: model:[{0}] field:[{1}]", originalFileName, spWikiPageItem.Name));
                    Assert.AreEqual(originalFileName, spWikiPageItem.Name);
                });
            });
        }
    }
}
