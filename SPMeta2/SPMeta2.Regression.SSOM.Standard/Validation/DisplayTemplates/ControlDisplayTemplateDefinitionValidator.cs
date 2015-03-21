using System.Collections.Generic;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.DisplayTemplates
{
    public class ControlDisplayTemplateDefinitionValidator : ControlDisplayTemplateModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ControlDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.Title, o => o.Title)
                                             .ShouldBeEqual(m => m.FileName, o => o.Name)

                                             .ShouldBeEqual(m => m.PreviewURL, o => o.GetPreviewURL())
                                             .ShouldBeEqual(m => m.PreviewDescription, o => o.GetPreviewDescription())

                                             .ShouldBeEqual(m => m.CrawlerXSLFile, o => o.GetCrawlerXSLFile())
                                             .ShouldBeEqual(m => m.HiddenTemplate, o => o.GetHiddenTemplate())
                                             .ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription())
                                             ;




        }
    }

    public static class SPListItemHelper
    {
        public static string GetPreviewURL(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetPreviewDescription(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }


        public static bool GetHiddenTemplate(this SPListItem item)
        {
            var res = ConvertUtils.ToBool(item["TemplateHidden"]);

            return res.HasValue ? res.Value : false;
        }

        public static string GetMasterPageDescription(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetCrawlerXSLFile(this SPListItem item)
        {
            return item["CrawlerXSLFile"] as string;
        }
    }
}
