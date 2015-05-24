using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListDefinitionValidator : ListModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var web = webModelHost.HostWeb;

            var definition = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());
            var spObject = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, definition.GetListUrl()));

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                //.ShouldBeEqual(m => m.Hidden, o => o.Hidden)
                //.ShouldBeEqual(m => m.Description, o => o.Description)
                //.ShouldBeEqual(m => m.IrmEnabled, o => o.IrmEnabled)
                //.ShouldBeEqual(m => m.IrmExpire, o => o.IrmExpire)
                //.ShouldBeEqual(m => m.IrmReject, o => o.IrmReject)
                //.ShouldBeEndOf(m => m.GetListUrl(), m => m.Url, o => o.GetServerRelativeUrl(), o => o.GetServerRelativeUrl())
                .ShouldBeEqual(m => m.ContentTypesEnabled, o => o.ContentTypesEnabled);

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description);


            if (!string.IsNullOrEmpty(definition.DraftVersionVisibility))
            {
                var draftOption = (DraftVisibilityType)Enum.Parse(typeof(DraftVisibilityType), definition.DraftVersionVisibility);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DraftVersionVisibility);
                    var dstProp = d.GetExpressionValue(m => m.DraftVersionVisibility);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = draftOption == (DraftVisibilityType)dstProp.Value
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.DraftVersionVisibility, "Skipping from validation. DraftVersionVisibility IS NULL");
            }

            if (!string.IsNullOrEmpty(definition.Url))
                assert.ShouldBeEndOf(m => m.GetListUrl(), m => m.Url, o => o.GetServerRelativeUrl(), o => o.GetServerRelativeUrl());
            else
                assert.SkipProperty(m => m.Url, "Skipping from validation. Url IS NULL");

            if (!string.IsNullOrEmpty(definition.CustomUrl))
                assert.ShouldBeEndOf(m => m.CustomUrl, o => o.GetServerRelativeUrl());
            else
                assert.SkipProperty(m => m.CustomUrl, "Skipping from validation. CustomUrl IS NULL");

            // common
            if (definition.EnableAttachments.HasValue)
                assert.ShouldBeEqual(m => m.EnableAttachments, o => o.EnableAttachments);
            else
                assert.SkipProperty(m => m.EnableAttachments, "Skipping from validation. EnableAttachments IS NULL");

            if (definition.EnableFolderCreation.HasValue)
                assert.ShouldBeEqual(m => m.EnableFolderCreation, o => o.EnableFolderCreation);
            else
                assert.SkipProperty(m => m.EnableFolderCreation, "Skipping from validation. EnableFolderCreation IS NULL");

            if (definition.EnableMinorVersions.HasValue)
                assert.ShouldBeEqual(m => m.EnableMinorVersions, o => o.EnableMinorVersions);
            else
                assert.SkipProperty(m => m.EnableMinorVersions, "Skipping from validation. EnableMinorVersions IS NULL");

            if (definition.EnableModeration.HasValue)
                assert.ShouldBeEqual(m => m.EnableModeration, o => o.EnableModeration);
            else
                assert.SkipProperty(m => m.EnableModeration, "Skipping from validation. EnableModeration IS NULL");

            if (definition.EnableVersioning.HasValue)
                assert.ShouldBeEqual(m => m.EnableVersioning, o => o.EnableVersioning);
            else
                assert.SkipProperty(m => m.EnableVersioning, "Skipping from validation. EnableVersioning IS NULL");

            if (definition.ForceCheckout.HasValue)
                assert.ShouldBeEqual(m => m.ForceCheckout, o => o.ForceCheckout);
            else
                assert.SkipProperty(m => m.ForceCheckout, "Skipping from validation. ForceCheckout IS NULL");

            if (definition.Hidden.HasValue)
                assert.ShouldBeEqual(m => m.Hidden, o => o.Hidden);
            else
                assert.SkipProperty(m => m.Hidden, "Skipping from validation. Hidden IS NULL");

            if (definition.NoCrawl.HasValue)
                assert.ShouldBeEqual(m => m.NoCrawl, o => o.NoCrawl);
            else
                assert.SkipProperty(m => m.NoCrawl, "Skipping from validation. NoCrawl IS NULL");


            if (definition.OnQuickLaunch.HasValue)
                assert.ShouldBeEqual(m => m.OnQuickLaunch, o => o.OnQuickLaunch);
            else
                assert.SkipProperty(m => m.OnQuickLaunch, "Skipping from validation. OnQuickLaunch IS NULL");

            // IRM
            if (definition.IrmEnabled.HasValue)
                assert.ShouldBeEqual(m => m.IrmEnabled, o => o.IrmEnabled);
            else
                assert.SkipProperty(m => m.IrmEnabled, "Skipping from validation. IrmEnabled IS NULL");

            if (definition.IrmExpire.HasValue)
                assert.ShouldBeEqual(m => m.IrmExpire, o => o.IrmExpire);
            else
                assert.SkipProperty(m => m.IrmExpire, "Skipping from validation. IrmExpire IS NULL");

            if (definition.IrmReject.HasValue)
                assert.ShouldBeEqual(m => m.IrmReject, o => o.IrmReject);
            else
                assert.SkipProperty(m => m.IrmReject, "Skipping from validation. IrmReject IS NULL");

            if (definition.TemplateType > 0)
            {
                assert
                    .ShouldBeEqual(m => m.TemplateType, o => (int)o.BaseTemplate)
                    .SkipProperty(m => m.TemplateName, "Skipping from validation. TemplateType should be == 0");
            }
            else
            {
                assert
                    .SkipProperty(m => m.TemplateType, "Skipping from validation. TemplateName should be empty");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TemplateName);
                    var listTemplate = ResolveListTemplate(web, definition);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid =
                            (spObject.TemplateFeatureId == listTemplate.FeatureId) &&
                            ((int)spObject.BaseTemplate == (int)listTemplate.Type)
                    };
                });
            }


            if (definition.MajorVersionLimit.HasValue)
                assert.ShouldBeEqual(m => m.MajorVersionLimit, o => o.MajorVersionLimit);
            else
                assert.SkipProperty(m => m.MajorVersionLimit, "Skipping from validation. MajorVersionLimit IS NULL");


            if (definition.MajorWithMinorVersionsLimit.HasValue)
                assert.ShouldBeEqual(m => m.MajorWithMinorVersionsLimit, o => o.MajorWithMinorVersionsLimit);
            else
                assert.SkipProperty(m => m.MajorWithMinorVersionsLimit, "Skipping from validation. MajorWithMinorVersionsLimit IS NULL");
        }
    }

    public static class ListExtensions
    {
        public static string GetServerRelativeUrl(this SPList list)
        {
            return list.RootFolder.ServerRelativeUrl;
        }
    }
}
