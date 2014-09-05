using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class PublishingPageDefinitionValidator : PublishingPageModelHandler
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = FindPublishingPage(folder, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.Name)
                                             .ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription())
                                             .ShouldBeEndOf(m => m.PageLayoutFileName, o => o.GetPublishingPagePageLayoutFileName())
                                             .ShouldBeEqual(m => m.Title, o => o.Title);


        }
    }

    public static class SPListItemHelper
    {
        public static string GetPublishingPageDescription(this SPListItem item)
        {
            return item[BuiltInPublishingFieldId.Description] as string;
        }

        public static string GetPublishingPagePageLayoutFileName(this SPListItem item)
        {
            return (new SPFieldUrlValue(item[BuiltInPublishingFieldId.PageLayout].ToString())).Url;
        }
    }
}
