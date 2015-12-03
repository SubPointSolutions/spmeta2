using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class ClientWebPartGalleryFileDefinitionValidator : WebPartGalleryFileModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var definition = model.WithAssertAndCast<WebPartGalleryFileDefinition>("model", value => value.RequireNotNull());

            var file = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = file.ListItemAllFields;

            var context = spObject.Context;

            context.Load(file, f => f.ServerRelativeUrl);
            context.Load(spObject);
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

            var assert = ServiceFactory.AssertService
                                         .NewAssert(definition, spObject)
                                         .ShouldNotBeNull(spObject)
                                         .ShouldBeEqual(m => m.Title, o => o.GetTitle())
                                         .ShouldBeEqual(m => m.FileName, o => o.GetFileLeafRef())

                                         .ShouldBeEqual(m => m.Description, o => o.GetWebPartGalleryFileDescription())
                                         .ShouldBeEqual(m => m.Group, o => o.GetWebPartGalleryFileGroup())
                                         ;

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = true;

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, file.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(dstContent);

                srcStringContent = WebpartXmlExtensions
                                    .LoadWebpartXmlDocument(srcStringContent)
                                    .SetTitle(s.Title)
                                    .SetOrUpdateProperty("Description", s.Description)
                                    .ToString();


                dstStringContent = WebpartXmlExtensions.LoadWebpartXmlDocument(dstStringContent).ToString();


                isContentValid = dstStringContent.Contains(srcStringContent);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isContentValid
                };
            });

            if (definition.RecommendationSettings.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.RecommendationSettings);
                    var isValid = true;

                    // TODO
                    //var targetControlTypeValue = d.GetWebPartGalleryFileRecommendationSettings();
                    //var targetControlTypeValues = new List<string>();

                    //for (var i = 0; i < targetControlTypeValue.Count; i++)
                    //    targetControlTypeValues.Add(targetControlTypeValue[i].ToUpper());

                    //foreach (var v in s.RecommendationSettings)
                    //{
                    //    if (!targetControlTypeValues.Contains(v.ToUpper()))
                    //        isValid = false;
                    //}

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
                assert.SkipProperty(m => m.RecommendationSettings, "RecommendationSettings is empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual(m => m.ContentTypeName, o => o.GetContentTypeName());
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is NULL. Skipping.");
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
    }

    internal static class ClientWebPartGalleryFileDefinitionValidatorExtensions
    {
        public static string GetWebPartGalleryFileDescription(this ListItem item)
        {
            return item["WebPartDescription"] as string;
        }

        public static string GetWebPartGalleryFileGroup(this ListItem item)
        {
            return item["Group"] as string;
        }

        //public static object GetWebPartGalleryFileRecommendationSettings(this ListItem item)
        //{
        //    if (item["QuickAddGroups"] != null)
        //        return item["QuickAddGroups"];

        //    return null;
        //}
    }
}
