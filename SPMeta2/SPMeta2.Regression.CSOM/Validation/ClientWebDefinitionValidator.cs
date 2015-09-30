using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebDefinitionValidator : WebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var hostClientContext = ExtractHostClientContext(modelHost);

            var parentWeb = ExtractWeb(modelHost);
            var definition = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            var currentWebUrl = GetCurrentWebUrl(parentWeb.Context, parentWeb, definition);
            var spObject = GetExistingWeb(hostClientContext.Site, parentWeb, currentWebUrl);
            var context = spObject.Context;

            context.Load(spObject,
                            w => w.HasUniqueRoleAssignments,
                            w => w.Description,
                            w => w.Url,
                            w => w.Language,
                            w => w.WebTemplate,
                            w => w.Configuration,
                            w => w.Title,
                            w => w.Id,
                            w => w.Url
                        );



            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.LCID, o => o.GetLCID())
                .ShouldBeEqual(m => m.UseUniquePermission, o => o.HasUniqueRoleAssignments);

            if (!string.IsNullOrEmpty(definition.WebTemplate))
            {
                assert.ShouldBeEqual(m => m.WebTemplate, o => o.GetWebTemplate());
                assert.SkipProperty(m => m.CustomWebTemplate);
            }
            else
            {
                assert.SkipProperty(m => m.WebTemplate);
                assert.SkipProperty(m => m.CustomWebTemplate);
            }

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Description is null or empty. Skipping.");

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Description is null or empty. Skipping.");


            assert.ShouldBeEqual((p, s, d) =>
            {
                if (!parentWeb.IsObjectPropertyInstantiated("Url"))
                {
                    parentWeb.Context.Load(parentWeb, o => o.Url);
                    parentWeb.Context.ExecuteQueryWithTrace();
                }

                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = s.Url;
                var dstUrl = d.Url;

                srcUrl = UrlUtility.RemoveStartingSlash(srcUrl);

                var dstSubUrl = dstUrl.Replace(parentWeb.Url + "/", string.Empty);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = srcUrl == dstSubUrl
                };
            });

            var supportsAlternateCssAndSiteImageUrl = ReflectionUtils.HasProperties(spObject, new[]
            {
                "AlternateCssUrl", 
                "SiteLogoUrl"
            });

            if (supportsAlternateCssAndSiteImageUrl)
            {
                if (!string.IsNullOrEmpty(definition.AlternateCssUrl))
                {
                    var alternateCssUrl = ReflectionUtils.GetPropertyValue(spObject, "AlternateCssUrl");

                    assert.ShouldBeEqual((p, s, d) =>
                   {
                       var srcProp = s.GetExpressionValue(def => def.AlternateCssUrl);
                       var isValid = true;

                       isValid = s.AlternateCssUrl.ToUpper().EndsWith(alternateCssUrl.ToString().ToUpper());

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
                    assert.SkipProperty(m => m.AlternateCssUrl);
                }

                if (!string.IsNullOrEmpty(definition.SiteLogoUrl))
                {
                    var siteLogoUrl = ReflectionUtils.GetPropertyValue(spObject, "SiteLogoUrl");

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.SiteLogoUrl);
                        var isValid = true;

                        isValid = s.SiteLogoUrl.ToUpper().EndsWith(siteLogoUrl.ToString().ToUpper());

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
                    assert.SkipProperty(m => m.SiteLogoUrl);
                }
            }

            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.AlternateCssUrl and Web.SiteLogoUrl methods support. Skipping validation.");

                assert.SkipProperty(m => m.AlternateCssUrl, "AlternateCssUrl is null or empty. Skipping.");
                assert.SkipProperty(m => m.SiteLogoUrl, "SiteLogoUrl is null or empty. Skipping.");
            }


            var supportsLocalization = ReflectionUtils.HasProperties(spObject, new[]
            {
                "TitleResource", "DescriptionResource"
            });

            if (supportsLocalization)
            {
                if (definition.TitleResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.TitleResource);
                        var isValid = true;

                        foreach (var userResource in s.TitleResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "TitleResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                    .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

                            if (!isValid)
                                break;
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
                    assert.SkipProperty(m => m.TitleResource, "TitleResource is NULL or empty. Skipping.");
                }

                if (definition.DescriptionResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.DescriptionResource);
                        var isValid = true;

                        foreach (var userResource in s.DescriptionResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "DescriptionResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                       .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

                            if (!isValid)
                                break;
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
                    assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is NULL or empty. Skipping.");
                }

            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.TitleResource and Web.DescriptionResource() methods support. Skipping validation.");

                assert.SkipProperty(m => m.TitleResource, "TitleResource is null or empty. Skipping.");
                assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is null or empty. Skipping.");
            }
        }
    }



    internal static class WebExtensions
    {
        public static uint GetLCID(this Web web)
        {
            return (uint)web.Language;
        }

        public static string GetWebTemplate(this Web web)
        {
            return string.Format("{0}#{1}", web.WebTemplate, web.Configuration);
        }
    }
}
