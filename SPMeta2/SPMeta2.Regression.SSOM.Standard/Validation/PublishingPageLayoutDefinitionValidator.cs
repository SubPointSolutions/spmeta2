using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class PublishingPageLayoutDefinitionValidator : PublishingPageLayoutModelHandler
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.Name)
                                             .ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription())
                                             .ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId())
                                             .ShouldBeEqual(m => m.Title, o => o.Title);

        }
    }

    public static class PublishingPageLayoutItemHelper
    {
        public static string GetPublishingPageLayoutDescription(this SPListItem item)
        {
            return item[BuiltInPublishingFieldId.Description] as string;
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeId(this SPListItem item)
        {
            var value = item[BuiltInPublishingFieldId.AssociatedContentType].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[2];
        }

        public static string GetPublishingPageLayoutAssociatedContentTypeName(this SPListItem item)
        {
            var value = item[BuiltInPublishingFieldId.AssociatedContentType].ToString();
            var values = value.Split(new string[] { ";#" }, StringSplitOptions.None);

            return values[1];
        }
    }
}
