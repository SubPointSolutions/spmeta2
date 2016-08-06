using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client.Utilities;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientTopNavigationNodeDefinitionValidator : TopNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (modelHost is WebModelHost)
                CurrentClientContext = (modelHost as WebModelHost).HostClientContext;

            CurrentModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var definition = model.WithAssertAndCast<TopNavigationNodeDefinition>("model", value => value.RequireNotNull());

            var spObject = LookupNodeForHost(modelHost, definition);
            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                  .ShouldNotBeNull(spObject)
                //.ShouldBeEndOf(m => m.Url, o => o.Url)
                  .ShouldBeEqual(m => m.IsExternal, o => o.IsExternal)
                  .ShouldBeEqual(m => m.IsVisible, o => o.IsVisible)
                  .ShouldBeEqual(m => m.Title, o => o.Title);

            var context = spObject.Context;

            assert.SkipProperty(m => m.Properties, "Skipping. Not supported by CSOM API");

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = ResolveTokenizedUrl(CurrentModelHost, definition);
                var dstUrl = d.Url;

                srcUrl = HttpUtility.UrlKeyValueDecode(srcUrl);
                dstUrl = HttpUtility.UrlKeyValueDecode(dstUrl);

                var isValid = dstUrl.ToUpper().EndsWith(srcUrl.ToUpper());

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            var supportsLocalization = ReflectionUtils.HasProperties(spObject, new[]
            {
                "TitleResource"
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
            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.TitleResource and Web.DescriptionResource() methods support. Skipping validation.");

                assert.SkipProperty(m => m.TitleResource, "TitleResource is null or empty. Skipping.");
            }
        }
    }
}
