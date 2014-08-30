using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWikiPageDefinitionValidator : WikiPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var context = folder.Context;

            var pageName = GetSafeWikiPageFileName(wikiPageModel);
            var file = GetWikiPageFile(folderModelHost.CurrentList.ParentWeb, folder, wikiPageModel);
            var pageItem = file.ListItemAllFields;

            context.Load(pageItem);
            context.ExecuteQuery();

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] wiki page:[{1}]", wikiPageModel, pageItem));
                Assert.IsNotNull(pageItem);

                traceScope.WithTraceIndent(trace =>
                {
                    var fileName = pageItem["FileLeafRef"];

                    traceScope.WriteLine(string.Format("Validate FileName model:[{0}] web part page:[{1}]", wikiPageModel.FileName, fileName));
                    Assert.AreEqual(pageName, fileName);
                });
            });
        }

        #endregion
    }
}
