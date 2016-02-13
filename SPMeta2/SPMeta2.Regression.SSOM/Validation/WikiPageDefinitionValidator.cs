using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WikiPageDefinitionValidator : WikiPageModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var spObject = FindWikiPageItem(folder, definition);

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                            .ShouldNotBeNull(spObject)
                                            .ShouldBeEqual(m => m.FileName, o => o.Name)
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
        }
    }

}
