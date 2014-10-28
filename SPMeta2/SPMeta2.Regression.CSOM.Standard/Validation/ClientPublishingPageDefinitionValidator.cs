using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPublishingPageDefinitionValidator : PublishingPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var spObject = FindPublishingPage(folderModelHost.CurrentList, folder, definition);

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                           .ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription())
                                           .ShouldBeEndOf(m => m.PageLayoutFileName, o => o.GetPublishingPagePageLayoutFileName())
                                           .ShouldBeEqual(m => m.Title, o => o.GetTitle());

        }

        #endregion
    }

    internal static class SPListItemHelper
    {
        public static string GetTitle(this ListItem item)
        {
            return item["Title"] as string;
        }

        public static string GetFileName(this ListItem item)
        {
            return item["FileLeafRef"] as string;
        }

        public static string GetPublishingPageDescription(this ListItem item)
        {
            return item["Comments"] as string;
        }

        public static string GetPublishingPagePageLayoutFileName(this ListItem item)
        {
            var result = item["PublishingPageLayout"] as FieldUrlValue;

            if (result != null)
                return result.Url;

            return string.Empty;
        }
    }
}
