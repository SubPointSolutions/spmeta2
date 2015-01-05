using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPublishingPageLayoutDefinitionValidator : PublishingPageLayoutModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            var spObject = FindPublishingPage(folderModelHost.CurrentList, folder, definition);

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                             .ShouldBeEqual(m => m.Description, o => o.GetPublishingPageLayoutDescription())
                                             .ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId())
                                             .ShouldBeEqual(m => m.Title, o => o.GetTitle());

        }

        #endregion
    }

    internal static class PublishingPageLayoutItemHelper
    {
        public static string GetPublishingPageLayoutDescription(this ListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeId(this ListItem item)
        {
            var value = item["PublishingAssociatedContentType"].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[2];
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeName(this ListItem item)
        {
            var value = item["PublishingAssociatedContentType"].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[1];
        }
    }
}
