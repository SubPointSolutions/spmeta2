using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using System.Text;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebPartPageDefinitionValidator : WebPartPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var context = folder.Context;

            var pageName = GetSafeWebPartPageFileName(definition);
            var pageFile = GetCurrentWebPartPage(folderModelHost.CurrentList, folder, pageName);

            var spObject = pageFile.ListItemAllFields;

            context.Load(spObject);
            context.Load(spObject, s => s.File);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           .ShouldBeEqual(m => m.FileName, o => o.GetName());

            assert.SkipProperty(m => m.Title, "Web part pages don't have title. Skipping.");

            if (!string.IsNullOrEmpty(definition.CustomPageLayout))
            {
                var custmPageContent = Encoding.UTF8.GetBytes(definition.CustomPageLayout);
                var pageTemplateContent = definition.GetWebPartPageTemplateContent();

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spObject.File.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PageLayoutTemplate);

                    var isValidPageLayoutTemplate = custmPageContent.SequenceEqual(dstContent);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValidPageLayoutTemplate
                    };
                });

                assert.SkipProperty(m => m.PageLayoutTemplate, "PageLayoutTemplate validated with GetWebPartPageTemplateContent() call before.");
                assert.SkipProperty(m => m.CustomPageLayout, "CustomPageLayout validated with GetCustomnPageContent() call before.");
            }
            else
            {
                assert.SkipProperty(m => m.CustomPageLayout, "CustomPageLayout is null or empty. Skipping.");
            }

            if (definition.PageLayoutTemplate > 0)
            {
                var pageTemplateContent = definition.GetWebPartPageTemplateContent();

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spObject.File.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PageLayoutTemplate);

                    var isValidPageLayoutTemplate = pageTemplateContent.SequenceEqual(dstContent);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValidPageLayoutTemplate
                    };
                });

                assert.SkipProperty(m => m.PageLayoutTemplate, "PageLayoutTemplate validated with GetWebPartPageTemplateContent() call before.");
            }
            else
            {
                assert.SkipProperty(m => m.CustomPageLayout, "PageLayoutTemplate is o or less. Skipping.");
            }
        }

        #endregion


    }

    internal static class WebPartPageDefinitionEx
    {
        public static byte[] GetWebPartPageTemplateContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(WebPartPageModelHandler.GetWebPartTemplateContent(definition));
        }

        public static byte[] GetCustomnPageContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(definition.CustomPageLayout);
        }
    }

    //internal static class SPListItemExtensions
    //{
    //    public static byte[] GetContent(this ListItem item)
    //    {
    //        byte[] result = null;

    //        using (var stream = File.OpenBinaryDirect(folderHost.HostClientContext, item.File.ServerRelativeUrl).Stream)
    //            dstContent = ModuleFileUtils.ReadFully(stream);

    //        return result;
    //    }
    //}
}
