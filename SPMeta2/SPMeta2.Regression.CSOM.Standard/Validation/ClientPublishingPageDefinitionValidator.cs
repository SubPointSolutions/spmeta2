using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPublishingPageDefinitionValidator : PublishingPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var definition = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

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

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);
            context.Load(spObject, o => o.ContentType);

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                           //.ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription())
                                           .ShouldBeEndOf(m => m.PageLayoutFileName, o => o.GetPublishingPagePageLayoutFileName())
                                           .ShouldBeEqual(m => m.Title, o => o.GetTitle());

            if (!string.IsNullOrEmpty(definition.Description))
            {
                assert.ShouldBeEqual(m => m.Description, o => o.GetPublishingPageDescription());
            }
            else
            {
                assert.SkipProperty(m => m.Description, "Description is NULL. Skipping.");
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

        #endregion
    }


}
