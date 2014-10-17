using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
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
            var definition = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var context = folder.Context;

            var pageName = GetSafeWikiPageFileName(definition);
            var file = GetWikiPageFile(folderModelHost.CurrentList.ParentWeb, folder, definition);
            var spObject = file.ListItemAllFields;

            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           .ShouldBeEqual(m => m.FileName, o => o.GetName())
                                           .SkipProperty(m => m.Title, "Title field is not available for wiki pages.");

        }

        #endregion
    }

    internal static class LIstItemUtils
    {
        public static string GetName(this ListItem item)
        {
            return item.FieldValues["FileLeafRef"] as string;
        }
    }
}
