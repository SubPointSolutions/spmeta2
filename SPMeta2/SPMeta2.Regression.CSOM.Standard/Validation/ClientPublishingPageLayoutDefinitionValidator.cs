using System;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Regression.CSOM.Standard.Extensions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPublishingPageLayoutDefinitionValidator : PublishingPageLayoutModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

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

            var spObject = FindPublishingPage(folderModelHost.CurrentList, folder, definition);
            var spFile = spObject.File;

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);
            context.Load(spObject, o => o.ContentType);

            context.Load(spFile, f => f.ServerRelativeUrl);

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.GetFileName());


            if (!string.IsNullOrEmpty(definition.Title))
                assert.ShouldBeEqual(m => m.Title, o => o.GetTitle());
            else
                assert.SkipProperty(m => m.Title);

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.GetPublishingPageLayoutDescription());
            else
                assert.SkipProperty(m => m.Description);

            if (!string.IsNullOrEmpty(definition.AssociatedContentTypeId))
                assert.ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId());
            else
                assert.SkipProperty(m => m.AssociatedContentTypeId, "AssociatedContentTypeId is null or empty.");

            if (!string.IsNullOrEmpty(definition.PreviewImageUrl))
            {
                var urlValue = spObject.FieldValues["PublishingPreviewImage"] as FieldUrlValue;

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PreviewImageUrl);
                    var isValid = (urlValue != null) && (urlValue.Url == s.PreviewImageUrl);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.PreviewImageDescription);
                    var isValid = (urlValue != null) && (urlValue.Description == s.PreviewImageDescription);

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
                assert.SkipProperty(m => m.PreviewImageUrl, "MasterPageUrl is NULL");
                assert.SkipProperty(m => m.PreviewImageDescription, "MasterPageDescription is NULL");
            }

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spFile.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                var srcStringContent = s.Content;
                var dstStringContent = Encoding.UTF8.GetString(dstContent);

                srcStringContent = srcStringContent
                   .Replace("meta:webpartpageexpansion=\"full\" ", string.Empty);

                isContentValid = dstStringContent.Contains(srcStringContent);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isContentValid
                };
            });


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
