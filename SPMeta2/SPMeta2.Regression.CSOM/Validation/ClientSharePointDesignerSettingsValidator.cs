using System.Linq;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSharePointDesignerSettingsValidator : SharePointDesignerSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SharePointDesignerSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = webHost.HostSite.RootWeb;

            var context = spObject.Context;
            context.Load(spObject, s => s.AllProperties);
            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            if (definition.EnableCustomizingMasterPagesAndPageLayouts.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.EnableCustomizingMasterPagesAndPageLayouts);
                    var isValid = true;

                    var dstValue = ConvertUtils.ToBool(spObject.AllProperties[BuiltInWebPropertyId.AllowMasterpageEditing]);

                    isValid = s.EnableCustomizingMasterPagesAndPageLayouts.Value == dstValue;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid,
                        Message = string.Format("{0} - {1}", BuiltInWebPropertyId.AllowMasterpageEditing, dstValue)
                    };
                });
            }
            else
            {
                assert.SkipProperty(o => o.EnableCustomizingMasterPagesAndPageLayouts, "EnableCustomizingMasterPagesAndPageLayouts is NULL. Skipping");
            }

            if (definition.EnableDetachingPages.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.EnableDetachingPages);
                    var isValid = true;

                    var dstValue = ConvertUtils.ToBool(spObject.AllProperties[BuiltInWebPropertyId.AllowRevertFromTemplate]);

                    isValid = s.EnableDetachingPages.Value == dstValue;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid,
                        Message = string.Format("{0} - {1}", BuiltInWebPropertyId.AllowMasterpageEditing, dstValue)
                    };
                });
            }
            else
            {
                assert.SkipProperty(o => o.EnableDetachingPages, "EnableDetachingPages is NULL. Skipping");
            }

            if (definition.EnableManagingWebSiteUrlStructure.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.EnableManagingWebSiteUrlStructure);
                    var isValid = true;

                    var dstValue = ConvertUtils.ToBool(spObject.AllProperties[BuiltInWebPropertyId.ShowUrlStructure]);

                    isValid = s.EnableManagingWebSiteUrlStructure.Value == dstValue;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid,
                        Message = string.Format("{0} - {1}", BuiltInWebPropertyId.AllowMasterpageEditing, dstValue)
                    };
                });
            }
            else
            {
                assert.SkipProperty(o => o.EnableManagingWebSiteUrlStructure, "EnableManagingWebSiteUrlStructure is NULL. Skipping");
            }

            if (definition.EnableSharePointDesigner.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.EnableSharePointDesigner);
                    var isValid = true;

                    var dstValue = ConvertUtils.ToBool(spObject.AllProperties[BuiltInWebPropertyId.AllowDesigner]);

                    isValid = s.EnableSharePointDesigner.Value == dstValue;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid,
                        Message = string.Format("{0} - {1}", BuiltInWebPropertyId.AllowMasterpageEditing, dstValue)
                    };
                });
            }
            else
            {
                assert.SkipProperty(o => o.EnableSharePointDesigner, "EnableSharePointDesigner is NULL. Skipping");
            }
        }
    }

}
