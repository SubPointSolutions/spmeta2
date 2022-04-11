using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class TopNavigationNodeDefinitionValidator : TopNavigationNodeModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // TMP
            if (modelHost is WebModelHost)
                CurrentWebModelHost = modelHost as WebModelHost;

            var definition = model.WithAssertAndCast<TopNavigationNodeDefinition>("model", value => value.RequireNotNull());

            var spObject = LookupNodeForHost(modelHost, definition);
            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                  .ShouldNotBeNull(spObject)
                //.ShouldBeEndOf(m => m.Url, o => o.Url)
                  .ShouldBeEqual(m => m.IsExternal, o => o.IsExternal)
                  .ShouldBeEqual(m => m.IsVisible, o => o.IsVisible)
                  .ShouldBeEqual(m => m.Title, o => o.Title);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Properties);
                var dstProp = d.GetExpressionValue(ct => ct.Properties);

                var isValid = true;

                var srcValues = s.Properties;
                var dstValues = d.Properties;

                foreach (var srcValue in srcValues)
                {
                    if (!dstValues.ContainsKey(srcValue.Key))
                    {
                        isValid = false;
                        break;
                    }

                    if (ConvertUtils.ToString(dstValues[srcValue.Key]) != srcValue.Value)
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Url);
                var dstProp = d.GetExpressionValue(ct => ct.Url);

                var srcUrl = s.Url;
                var dstUrl = d.Url;

                srcUrl = ResolveTokenizedUrl(CurrentWebModelHost, definition);

                srcUrl = HttpUtility.UrlDecode(srcUrl);
                dstUrl = HttpUtility.UrlDecode(dstUrl);

                var isValid = dstUrl.ToUpper().EndsWith(srcUrl.ToUpper());

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            /// localization
            if (definition.TitleResource.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.TitleResource);
                    var isValid = true;

                    foreach (var userResource in s.TitleResource)
                    {
                        var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                        var value = d.TitleResource.GetValueForUICulture(culture);

                        isValid = userResource.Value == value;

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
    }
}
