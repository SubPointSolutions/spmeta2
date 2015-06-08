using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class WebNavigationSettingsDefinitionValidator : WebNavigationSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebNavigationSettingsDefinition>("model",
                value => value.RequireNotNull());

            var spObject = GetWebNavigationSettings(webModelHost, definition);
            var web = webModelHost.HostWeb;

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            var globalNavIncludeTypes = GetGlobalNavigationIncludeTypes(definition);
            var currentNavIncludeTypes = GetCurrentNavigationIncludeTypes(definition);

            var globalNavIncludeTypesValue =
                ConvertUtils.ToInt(web.AllProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes]);
            var currentNavIncludeTypesValue =
                ConvertUtils.ToInt(web.AllProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes]);

            // global tyepes
            var isGlobalNavIncludeTypesValid = globalNavIncludeTypes == globalNavIncludeTypesValue;

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.GlobalNavigationShowSubsites);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isGlobalNavIncludeTypesValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.GlobalNavigationShowPages);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isGlobalNavIncludeTypesValid
                };
            });

            var isCurrentNavIncludeTypesValid = currentNavIncludeTypes == currentNavIncludeTypesValue;

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.CurrentNavigationShowSubsites);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isCurrentNavIncludeTypesValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.CurrentNavigationShowPages);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isCurrentNavIncludeTypesValid
                };
            });

            // items count
            if (definition.GlobalNavigationMaximumNumberOfDynamicItems.HasValue)
            {
                var globalDynamicChildLimitValue =
                    ConvertUtils.ToInt(web.AllProperties[BuiltInWebPropertyId.GlobalDynamicChildLimit]);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.GlobalNavigationMaximumNumberOfDynamicItems);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = s.GlobalNavigationMaximumNumberOfDynamicItems == globalDynamicChildLimitValue
                    };
                });

            }
            else
            {
                assert.SkipProperty(d => d.GlobalNavigationMaximumNumberOfDynamicItems,
                    "GlobalNavigationMaximumNumberOfDynamicItems is null or empty");
            }

            if (definition.CurrentNavigationMaximumNumberOfDynamicItems.HasValue)
            {
                var currentDynamicChildLimitValue =
                    ConvertUtils.ToInt(web.AllProperties[BuiltInWebPropertyId.CurrentDynamicChildLimit]);

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CurrentNavigationMaximumNumberOfDynamicItems);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = s.CurrentNavigationMaximumNumberOfDynamicItems == currentDynamicChildLimitValue
                    };
                });
            }
            else
            {
                assert.SkipProperty(d => d.CurrentNavigationMaximumNumberOfDynamicItems,
                    "CurrentNavigationMaximumNumberOfDynamicItems is null or empty");
            }

            // nav sources
            if (!string.IsNullOrEmpty(definition.GlobalNavigationSource))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.GlobalNavigationSource);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = s.GlobalNavigationSource == spObject.GlobalNavigation.Source.ToString()
                    };
                });
            }
            else
            {
                assert.SkipProperty(d => d.GlobalNavigationSource,
                   "GlobalNavigationSource is null or empty");
            }

            if (!string.IsNullOrEmpty(definition.CurrentNavigationSource))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CurrentNavigationSource);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = s.CurrentNavigationSource == spObject.CurrentNavigation.Source.ToString()
                    };
                });
            }
            else
            {
                assert.SkipProperty(d => d.CurrentNavigationSource,
                   "CurrentNavigationSource is null or empty");
            }
        }
    }
}
