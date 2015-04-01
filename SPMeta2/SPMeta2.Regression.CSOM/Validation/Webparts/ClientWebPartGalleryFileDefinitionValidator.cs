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
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class ClientWebPartGalleryFileDefinitionValidator : WebPartGalleryFileModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<WebPartGalleryFileDefinition>("model", value => value.RequireNotNull());

            var file = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = file.ListItemAllFields;

            var context = spObject.Context;

            context.Load(file, f => f.ServerRelativeUrl);
            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                         .NewAssert(definition, spObject)
                                         .ShouldNotBeNull(spObject)
                                         .ShouldBeEqual(m => m.Title, o => o.GetTitle())
                                         .ShouldBeEqual(m => m.FileName, o => o.GetName())

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
