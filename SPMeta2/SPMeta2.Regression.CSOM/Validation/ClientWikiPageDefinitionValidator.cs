using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
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
                //.ShouldBePartOf(m => m.Content, o => o.GetWikiPageContent())
                                           .SkipProperty(m => m.Title, "Title field is not available for wiki pages.");




            if (!string.IsNullOrEmpty(definition.Content))
            {

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Content);

                    var srcContent = Regex.Replace(definition.Content, @"<[^>]*>", string.Empty);
                    var dstContent = Regex.Replace(spObject.GetWikiPageContent(), @"<[^>]*>", string.Empty);

                    // crazy lazy
                    var isValid =
                        dstContent.Trim().Replace((char)8203, char.Parse(" ")).Replace(" ", "")
                        == srcContent.Trim().Replace((char)8203, char.Parse(" ")).Replace(" ", "");

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.Content, "Content is NULL. Skiping.");
            }
        }

        #endregion
    }

    internal static class LIstItemUtils
    {
        public static string GetName(this ListItem item)
        {
            return item.FieldValues["FileLeafRef"] as string;
        }

        public static string GetWikiPageContent(this ListItem pageItem)
        {
            return pageItem["WikiField"] as string;
        }
    }
}
