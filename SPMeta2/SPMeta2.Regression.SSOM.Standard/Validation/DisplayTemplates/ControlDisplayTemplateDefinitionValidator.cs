using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.Base;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.DisplayTemplates
{
    public class ControlDisplayTemplateDefinitionValidator : ItemControlTemplateDefinitionBaseValidator
    {
        public override string FileExtension
        {
            get { return "html"; }
            set { }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ControlDisplayTemplateDefinition>("model", value => value.RequireNotNull());
            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);
            var file = spObject.File;

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject);

            #region crawler xslt file

            if (!string.IsNullOrEmpty(definition.CrawlerXSLFileURL))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CrawlerXSLFileURL);
                    var isValid = d.GetCrawlerXSLFile().Url == s.CrawlerXSLFileURL;

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
                assert.SkipProperty(m => m.CrawlerXSLFileURL, "PreviewURL is NULL. Skipping");
            }

            if (!string.IsNullOrEmpty(definition.CrawlerXSLFileDescription))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CrawlerXSLFileDescription);
                    var isValid = d.GetCrawlerXSLFile().Description == s.CrawlerXSLFileDescription;

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
                assert.SkipProperty(m => m.CrawlerXSLFileDescription, "PreviewDescription is NULL. Skipping");
            }

            #endregion

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(file.GetContent());

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

        public override System.Type TargetType
        {
            get { return typeof(ControlDisplayTemplateDefinition); }
        }
    }

    public static class SPListItemHelper
    {


        public static SPFieldUrlValue GetPreviewURL(this SPListItem item)
        {
            if (item["HtmlDesignPreviewUrl"] != null)
                return new SPFieldUrlValue(item["HtmlDesignPreviewUrl"].ToString());

            return null;
        }

        public static SPFieldUrlValue GetCrawlerXSLFile(this SPListItem item)
        {
            if (item["CrawlerXSLFile"] != null)
                return new SPFieldUrlValue(item["CrawlerXSLFile"].ToString());

            return null;
        }


        public static bool GetHiddenTemplate(this SPListItem item)
        {
            var res = ConvertUtils.ToBool(item["TemplateHidden"]);

            return res.HasValue ? res.Value : false;
        }

        public static string GetMasterPageDescription(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        

        public static string GetManagedPropertyMapping(this SPListItem item)
        {
            return item["ManagedPropertyMapping"] as string;
        }

        public static string GetCompatibleManagedProperties(this SPListItem item)
        {
            return item["CompatibleManagedProperties"] as string;
        }
    }
}
