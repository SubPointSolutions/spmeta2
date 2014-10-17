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
