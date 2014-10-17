using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;
using System.Text;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebPartPageDefinitionValidator : WebPartPageModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModel = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModel.CurrentLibraryFolder;
            var spObject = FindWebPartPage(folder, definition);


            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           .ShouldBeEqual(m => m.FileName, o => o.Name);
            //.ShouldBeEqual(m => m.Title, o => o.Title);

            assert.SkipProperty(m => m.Title, "Web part pages don't have title. Skipping.");

            if (!string.IsNullOrEmpty(definition.CustomPageLayout))
            {
                var custmPageContent = Encoding.UTF8.GetBytes(definition.CustomPageLayout);

                assert.ShouldBeEqual(m => m.GetCustomnPageContent(), o => o.GetContent());
                assert.SkipProperty(m => m.CustomPageLayout, "CustomPageLayout validated with GetCustomnPageContent() call before.");
            }
            else
            {
                assert.SkipProperty(m => m.CustomPageLayout, "CustomPageLayout is null or empty. Skipping.");
            }

            if (definition.PageLayoutTemplate > 0)
            {
                var pageTemplateContent = definition.GetWebPartPageTemplateContent();

                assert.ShouldBeEqual(m => m.GetWebPartPageTemplateContent(), o => o.GetContent());
                assert.SkipProperty(m => m.PageLayoutTemplate, "PageLayoutTemplate validated with GetWebPartPageTemplateContent() call before.");
            }
            else
            {
                assert.SkipProperty(m => m.CustomPageLayout, "PageLayoutTemplate is o or less. Skipping.");
            }

        }
    }

    internal static class WebPartPageDefinitionEx
    {
        public static byte[] GetWebPartPageTemplateContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(WebPartPageModelHandler.GetWebPartPageTemplateContent(definition));
        }

        public static byte[] GetCustomnPageContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(definition.CustomPageLayout);
        }
    }

    internal static class SPListItemExtensions
    {
        public static byte[] GetContent(this SPListItem item)
        {
            byte[] result = null;

            using (var stream = item.File.OpenBinaryStream())
                result = ModuleFileUtils.ReadFully(stream);

            return result;
        }
    }
}
