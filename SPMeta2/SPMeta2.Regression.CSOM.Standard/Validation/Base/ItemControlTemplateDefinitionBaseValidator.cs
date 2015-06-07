using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Base
{
    public abstract class ItemControlTemplateDefinitionBaseValidator : TemplateDefinitionBaseValidator
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<ItemControlTemplateDefinitionBase>("model", value => value.RequireNotNull());

            var spFile = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = spFile.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spFile, f => f.ServerRelativeUrl);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            #region preview field

            if (!string.IsNullOrEmpty(definition.PreviewURL))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;
                    var srcProp = s.GetExpressionValue(m => m.PreviewURL);

                    var previewValue = d.GetPreviewURL();
                    isValid = (previewValue != null) &&
                              (d.GetPreviewURL().Url == s.PreviewURL);

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
                assert.SkipProperty(m => m.PreviewURL, "PreviewURL is NULL. Skipping");
            }

            if (!string.IsNullOrEmpty(definition.PreviewDescription))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;
                    var srcProp = s.GetExpressionValue(m => m.PreviewDescription);

                    var previewValue = d.GetPreviewURL();
                    isValid = (previewValue != null) &&
                              (d.GetPreviewURL().Description == s.PreviewDescription);

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
                assert.SkipProperty(m => m.PreviewDescription, "PreviewDescription is NULL. Skipping");
            }

            #endregion

            #region TargetControlTypes

            if (definition.TargetControlTypes.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TargetControlTypes);
                    var isValid = true;

                    var targetControlTypeValues = (d["TargetControlType"] as string[])
                                     .Select(v => v.ToUpper()).ToList();

                    foreach (var v in s.TargetControlTypes)
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
                assert.SkipProperty(m => m.TargetControlTypes, "TargetControlTypes count is 0. Skipping");
            }

            #endregion
        }

        public override string FileExtension
        {
            get { return string.Empty; }
            set
            {

            }
        }


    }

    public static class ListItemHelper
    {
        public static FieldUrlValue GetPreviewURL(this ListItem item)
        {
            if (item["HtmlDesignPreviewUrl"] != null)
                return item["HtmlDesignPreviewUrl"] as FieldUrlValue;

            return null;
        }

        // 
        public static string GetCompatibleManagedProperties(this ListItem item)
        {
            return item["CompatibleManagedProperties"] as string;
        }

        public static string GetManagedPropertyMapping(this ListItem item)
        {
            return item["ManagedPropertyMapping"] as string;
        }

        public static FieldUrlValue GetCrawlerXSLFile(this ListItem item)
        {
            if (item["CrawlerXSLFile"] != null)
                return item["CrawlerXSLFile"] as FieldUrlValue;

            return null;
        }

        public static bool GetHiddenTemplate(this ListItem item)
        {
            var res = ConvertUtils.ToBool(item["TemplateHidden"]);

            return res.HasValue ? res.Value : false;
        }

        public static string GetMasterPageDescription(this ListItem item)
        {
            return item["MasterPageDescription"] as string;
        }


        public static string GetStandalone(this ListItem item)
        {
            return item["DisplayTemplateJSTemplateType"] as string;
        }


        public static string GetTargetControlType(this ListItem item)
        {
            return item["DisplayTemplateJSTargetControlType"] as string;
        }

        public static string GetTargetListTemplateId(this ListItem item)
        {
            return item["DisplayTemplateJSTargetListTemplate"] as string;
        }


        public static string GetTargetScope(this ListItem item)
        {
            return item["DisplayTemplateJSTargetScope"] as string;
        }

        public static FieldUrlValue GetDisplayTemplateJSIconUrl(this ListItem item)
        {
            if (item["DisplayTemplateJSIconUrl"] != null)
                return item["DisplayTemplateJSIconUrl"] as FieldUrlValue;

            return null;
        }
    }
}
