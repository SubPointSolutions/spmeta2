using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.ModelHandlers.Webparts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Collections.Generic;
using SPMeta2.Containers.Assertion;
using System.Text;

namespace SPMeta2.Regression.SSOM.Validation.Webparts
{
    public class WebPartGalleryFileDefinitionValidator : WebPartGalleryFileModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<WebPartGalleryFileDefinition>("model", value => value.RequireNotNull());

            CurrentModel = definition;

            var spObject = GetCurrentObject(folder, definition);
            var file = spObject.File;

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject)
                                        .ShouldBeEqual(m => m.Title, o => o.Title)
                                        .ShouldBeEqual(m => m.FileName, o => o.Name)

                                        .ShouldBeEqual(m => m.Description, o => o.GetWebPartGalleryFileDescription())
                                        .ShouldBeEqual(m => m.Group, o => o.GetWebPartGalleryFileGroup())
                                        ;


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = true;

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(file.GetContent());

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

                    var targetControlTypeValue = d.GetWebPartGalleryFileRecommendationSettings();
                    var targetControlTypeValues = new List<string>();

                    for (var i = 0; i < targetControlTypeValue.Count; i++)
                        targetControlTypeValues.Add(targetControlTypeValue[i].ToUpper());

                    foreach (var v in s.RecommendationSettings)
                    {
                        if (!targetControlTypeValues.Contains(v.ToUpper()))
                            isValid = false;
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
                assert.SkipProperty(m => m.RecommendationSettings, "RecommendationSettings is empty. Skipping.");
            }


        }
    }

    internal static class WebPartGalleryFileDefinitionValidatorExtensions
    {
        public static string GetWebPartGalleryFileDescription(this SPListItem item)
        {
            return item["WebPartDescription"] as string;
        }

        public static string GetWebPartGalleryFileGroup(this SPListItem item)
        {
            return item["Group"] as string;
        }

        public static SPFieldMultiChoiceValue GetWebPartGalleryFileRecommendationSettings(this SPListItem item)
        {
            if (item["QuickAddGroups"] != null)
                return new SPFieldMultiChoiceValue(item["QuickAddGroups"].ToString());

            return null;
        }
    }
}
