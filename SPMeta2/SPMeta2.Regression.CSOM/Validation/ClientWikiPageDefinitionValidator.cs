using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Regression.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWikiPageDefinitionValidator : WikiPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var context = folder.Context;

            var pageName = GetSafeWikiPageFileName(definition);
            var file = GetWikiPageFile(folderModelHost.CurrentList.ParentWeb, folder, definition);
            var spObject = file.ListItemAllFields;

            context.Load(spObject);
            context.Load(spObject, o => o.ContentType);
            context.ExecuteQueryWithTrace();

            var stringCustomContentType = string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentTypeName)
                || !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                if (!string.IsNullOrEmpty(definition.ContentTypeName))
                {
                    stringCustomContentType = ContentTypeLookupService
                                                    .LookupContentTypeByName(folderModelHost.CurrentList, definition.ContentTypeName)
                                                    .Name;
                }
            }

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);


            assert
                .ShouldNotBeNull(spObject)

                .ShouldBeEqual(m => m.FileName, o => o.GetFileLeafRef())
                .SkipProperty(m => m.Title, "Title field is not available for wiki pages.");

            if (!string.IsNullOrEmpty(definition.Content))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Content);

                    var srcContent = Regex.Replace(definition.Content, @"<[^>]*>", string.Empty);
                    var dstContent = Regex.Replace(spObject.GetWikiField(), @"<[^>]*>", string.Empty);

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

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                // TODO
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
                    var currentContentTypeName = d.ContentType.Name;

                    var isValis = stringCustomContentType == currentContentTypeName;

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
                            if (d[srcValue.FieldName].ToString() != srcValue.Value.ToString())
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

            if (definition.Values.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    foreach (var srcValue in s.Values)
                    {
                        // big TODO here for == != 

                        if (!string.IsNullOrEmpty(srcValue.FieldName))
                        {
                            if (d[srcValue.FieldName].ToString() != srcValue.Value.ToString())
                                isValid = false;
                        }

                        if (!isValid)
                            break;
                    }

                    var srcProp = s.GetExpressionValue(def => def.Values);

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
                assert.SkipProperty(m => m.Values, "Values.Count == 0. Skipping.");
            }
        }

        #endregion
    }
}
