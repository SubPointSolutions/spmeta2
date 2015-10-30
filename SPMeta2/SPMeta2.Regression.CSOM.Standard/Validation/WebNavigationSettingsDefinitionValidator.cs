using System;
using System.Net;
using CsQuery;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
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

            var context = web.Context;

            context.Load(spObject);
            context.Load(spObject.GlobalNavigation);
            context.Load(spObject.CurrentNavigation);

            context.Load(web, w => w.AllProperties);

            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            //  web??/_layouts/15/AreaNavigationSettings.aspx
            // extra protection, downbloading HTML page and making sure checkboxes are there :)

            //<input name="ctl00$PlaceHolderMain$globalNavSection$ctl02$globalIncludeSubSites" type="checkbox" id="ctl00_PlaceHolderMain_globalNavSection_ctl02_globalIncludeSubSites" checked="checked">
            //<input name="ctl00$PlaceHolderMain$globalNavSection$ctl02$globalIncludePages" type="checkbox" id="ctl00_PlaceHolderMain_globalNavSection_ctl02_globalIncludePages" disabled="disabled">


            //<input name="ctl00$PlaceHolderMain$currentNavSection$ctl02$currentIncludeSubSites" type="checkbox" id="ctl00_PlaceHolderMain_currentNavSection_ctl02_currentIncludeSubSites">
            //<input name="ctl00$PlaceHolderMain$currentNavSection$ctl02$currentIncludePages" type="checkbox" id="ctl00_PlaceHolderMain_currentNavSection_ctl02_currentIncludePages" disabled="disabled">
            var pageUrl = UrlUtility.CombineUrl(web.Url, "/_layouts/15/AreaNavigationSettings.aspx");

            var client = new WebClient();
            if (context.Credentials != null)
            {
                client.Credentials = context.Credentials;
                client.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
            }
            else
                client.UseDefaultCredentials = true;

            var pageContent = client.DownloadString(new Uri(pageUrl));
            CQ j = pageContent;

            // so not only API, but also real checboxed on browser page check
            var globalSubSites = j.Select("input[id$='globalIncludeSubSites']").First();
            var globalSubSitesValue = globalSubSites.Attr("checked") == "checked";

            var globalIncludePages = j.Select("input[id$='globalIncludePages']").First();
            var globalIncludePagesValue = globalIncludePages.Attr("checked") == "checked";

            var currentIncludeSubSites = j.Select("input[id$='currentIncludeSubSites']").First();
            var currentIncludeSubSitesValue = currentIncludeSubSites.Attr("checked") == "checked";

            var currentIncludePages = j.Select("input[id$='currentIncludePages']").First();
            var currentIncludePagesValue = currentIncludePages.Attr("checked") == "checked";

            // global types
            if (definition.GlobalNavigationShowSubsites.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {

                    var navIncludeTypesValueString = web.AllProperties.FieldValues.ContainsKey(BuiltInWebPropertyId.GlobalNavigationIncludeTypes)
                   ? web.AllProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes]
                   : string.Empty;

                    var globalNavIncludeTypesValue = ConvertUtils.ToInt(navIncludeTypesValueString);

                    var isGlobalNavIncludeTypesValid = false;

                    if (definition.GlobalNavigationShowSubsites.Value)
                    {
                        isGlobalNavIncludeTypesValid =
                            ((globalNavIncludeTypesValue & 1) == 1) && (globalSubSitesValue);
                    }
                    else
                    {
                        isGlobalNavIncludeTypesValid =
                            ((globalNavIncludeTypesValue & 1) != 1) && (!globalSubSitesValue);
                    }


                    var srcProp = s.GetExpressionValue(m => m.GlobalNavigationShowSubsites);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isGlobalNavIncludeTypesValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(d => d.GlobalNavigationShowSubsites,
                   "GlobalNavigationShowSubsites is null or empty");
            }

            if (definition.GlobalNavigationShowPages.HasValue)
            {
                var currentNavIncludeTypesValueString = web.AllProperties.FieldValues.ContainsKey(BuiltInWebPropertyId.GlobalNavigationIncludeTypes)
                    ? web.AllProperties[BuiltInWebPropertyId.GlobalNavigationIncludeTypes]
                    : string.Empty;

                var globalNavIncludeTypesValue =
                       ConvertUtils.ToInt(currentNavIncludeTypesValueString);

                var isGlobalNavIncludeTypesValid = false;

                if (definition.GlobalNavigationShowPages.Value)
                {
                    isGlobalNavIncludeTypesValid =
                        ((globalNavIncludeTypesValue & 2) == 2) && (globalIncludePagesValue);
                }
                else
                {
                    isGlobalNavIncludeTypesValid =
                        ((globalNavIncludeTypesValue & 2) != 2 && (!globalIncludePagesValue));
                }

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
            }
            else
            {
                assert.SkipProperty(d => d.GlobalNavigationShowPages,
                   "GlobalNavigationShowPages is null or empty");
            }



            if (definition.CurrentNavigationShowSubsites.HasValue)
            {
                var currentNavIncludeTypesValueString = web.AllProperties.FieldValues.ContainsKey(BuiltInWebPropertyId.CurrentNavigationIncludeTypes)
                    ? web.AllProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes]
                    : string.Empty;

                var currentNavIncludeTypesValue = ConvertUtils.ToInt(currentNavIncludeTypesValueString);

                var isGlobalNavIncludeTypesValid = false;

                if (definition.CurrentNavigationShowSubsites.Value)
                {
                    isGlobalNavIncludeTypesValid =
                        ((currentNavIncludeTypesValue & 1) == 1) && (currentIncludeSubSitesValue);
                }
                else
                {
                    isGlobalNavIncludeTypesValid =
                        ((currentNavIncludeTypesValue & 1) != 1) && (!currentIncludeSubSitesValue);
                }


                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CurrentNavigationShowSubsites);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isGlobalNavIncludeTypesValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(d => d.CurrentNavigationShowSubsites,
                  "CurrentNavigationShowSubsites is null or empty");
            }

            if (definition.CurrentNavigationShowPages.HasValue)
            {
                var currentNavIncludeTypesValueString = web.AllProperties.FieldValues.ContainsKey(BuiltInWebPropertyId.CurrentNavigationIncludeTypes)
                    ? web.AllProperties[BuiltInWebPropertyId.CurrentNavigationIncludeTypes]
                    : string.Empty;

                var currentNavIncludeTypesValue = ConvertUtils.ToInt(currentNavIncludeTypesValueString);

                var isGlobalNavIncludeTypesValid = false;

                if (definition.CurrentNavigationShowSubsites.Value)
                {
                    isGlobalNavIncludeTypesValid =
                        ((currentNavIncludeTypesValue & 2) == 2) && (currentIncludeSubSitesValue);
                }
                else
                {
                    isGlobalNavIncludeTypesValid =
                        ((currentNavIncludeTypesValue & 2) != 2) && (!currentIncludeSubSitesValue);
                }

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.CurrentNavigationShowPages);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isGlobalNavIncludeTypesValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(d => d.CurrentNavigationShowPages,
                  "CurrentNavigationShowPages is null or empty");
            }

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
