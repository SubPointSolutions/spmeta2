using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;
using System.Text;
using SPMeta2.Containers.Assertion;

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

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {

            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeId, "ContentTypeId is null or empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeName);
                    var currentContentTypeName = d.ListItemAllFields["ContentType"] as string;

                    var isValis = s.ContentTypeName == currentContentTypeName;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValis
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is null or empty. Skipping.");
            }


            if (definition.DefaultValues.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    foreach (var srcValue in s.DefaultValues)
                    {
                        // big TODO here for == != 

                        if (!string.IsNullOrEmpty(srcValue.FieldName))
                        {
                            if (d.ListItemAllFields[srcValue.FieldName].ToString() != srcValue.Value.ToString())
                                isValid = false;
                        }
                        else if (srcValue.FieldId.HasValue)
                        {
                            if (d.ListItemAllFields[srcValue.FieldId.Value].ToString() != srcValue.Value.ToString())
                                isValid = false;
                        }

                        if (!isValid)
                            break;
                    }

                    var srcProp = s.GetExpressionValue(def => def.DefaultValues);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.DefaultValues, "DefaultValues.Count == 0. Skipping.");
            }
        }
    }

    internal static class WebPartPageDefinitionEx
    {
        public static byte[] GetWebPartPageTemplateContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(new WebPartPageModelHandler().GetWebPartPageTemplateContent(definition));
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
