using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.Base;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.DisplayTemplates
{
    public class JavaScriptDisplayTemplateDefinitionValidator : TemplateDefinitionBaseValidator
    {
        public override string FileExtension
        {
            get { return "js"; }
            set { }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<JavaScriptDisplayTemplateDefinition>("model", value => value.RequireNotNull());
            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);
            var file = spObject.File;

            var assert = ServiceFactory.AssertService
                                  .NewAssert(definition, spObject)
                                  .ShouldNotBeNull(spObject);

            assert.ShouldBeEqual(m => m.Standalone, o => o.GetStandalone());
            assert.ShouldBeEqual(m => m.TargetControlType, o => o.GetTargetControlType());
            assert.ShouldBeEqual(m => m.TargetListTemplateId, o => o.GetTargetListTemplateId());
            assert.ShouldBeEqual(m => m.TargetScope, o => o.GetTargetScope());

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

            #region icon url

            if (!string.IsNullOrEmpty(definition.IconUrl))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IconUrl);
                    var isValid = d.GetDisplayTemplateJSIconUrl().Url == s.IconUrl;

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
                assert.SkipProperty(m => m.IconUrl, "IconUrl is NULL. Skipping");
            }

            if (!string.IsNullOrEmpty(definition.IconDescription))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IconDescription);
                    var isValid = d.GetDisplayTemplateJSIconUrl().Description == s.IconDescription;

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
                assert.SkipProperty(m => m.IconDescription, "IconDescription is NULL. Skipping");
            }

            #endregion
        }

        public override System.Type TargetType
        {
            get { return typeof(JavaScriptDisplayTemplateDefinition); }
        }
    }

    public static class JavaScriptDisplayTemplateDefinitionValidatorHelper
    {
        public static SPFieldUrlValue GetDisplayTemplateJSIconUrl(this SPListItem item)
        {
            if (item["DisplayTemplateJSIconUrl"] != null)
                return new SPFieldUrlValue(item["DisplayTemplateJSIconUrl"].ToString());

            return null;
        }

        public static string GetStandalone(this SPListItem item)
        {
            return item["DisplayTemplateJSTemplateType"] as string;
        }
        public static string GetTargetControlType(this SPListItem item)
        {
            return item["DisplayTemplateJSTargetControlType"] as string;
        }

        public static string GetTargetListTemplateId(this SPListItem item)
        {
            return item["DisplayTemplateJSTargetListTemplate"] as string;
        }

        public static string GetTargetScope(this SPListItem item)
        {
            return item["DisplayTemplateJSTargetScope"] as string;
        }

    }
}
