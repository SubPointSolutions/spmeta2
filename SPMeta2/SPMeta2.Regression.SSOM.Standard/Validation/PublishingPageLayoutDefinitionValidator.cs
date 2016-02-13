using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.Regression.SSOM.Standard.Extensions;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class PublishingPageLayoutDefinitionValidator : PublishingPageLayoutModelHandler
    {

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);
            var file = spObject.File;

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.Name);

            if (!string.IsNullOrEmpty(definition.Title))
                assert.ShouldBeEndOf(m => m.Title, o => o.Title);
            else
                assert.SkipProperty(m => m.Title);

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEndOf(m => m.Description, o => o.GetPublishingPageLayoutDescription());
            else
                assert.SkipProperty(m => m.Description);

            if (!string.IsNullOrEmpty(definition.AssociatedContentTypeId))
                assert.ShouldBeEndOf(m => m.AssociatedContentTypeId, o => o.GetPublishingPageLayoutAssociatedContentTypeId());
            else
                assert.SkipProperty(m => m.AssociatedContentTypeId);

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
                    var currentContentTypeName = d["ContentType"] as string;

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

            if (definition.DefaultValues.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DefaultValues);

                    var isValid = true;

                    foreach (var value in definition.DefaultValues)
                    {
                        object itemValue = null;

                        if (value.FieldId.HasValue)
                            itemValue = spObject[value.FieldId.Value];
                        else
                            itemValue = spObject[value.FieldName];

                        if (!Equals(itemValue, value.Value))
                        {
                            isValid = false;
                        }
                    }

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
                assert.SkipProperty(m => m.DefaultValues, "DefaultValues is empty. Skipping.");
            }


            if (definition.Values.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.Values);

                    var isValid = true;

                    foreach (var value in definition.Values)
                    {
                        object itemValue = null;

                        if (value.FieldId.HasValue)
                            itemValue = spObject[value.FieldId.Value];
                        else
                            itemValue = spObject[value.FieldName];

                        if (!Equals(itemValue, value.Value))
                        {
                            isValid = false;
                        }
                    }

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
                assert.SkipProperty(m => m.Values, "Values is empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.PreviewImageUrl))
            {
                var urlValue = new SPFieldUrlValue(spObject["PublishingPreviewImage"].ToString());

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

                var srcStringContent = s.Content;
                var dstStringContent = Encoding.UTF8.GetString(file.GetContent());

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
        }
    }

}
