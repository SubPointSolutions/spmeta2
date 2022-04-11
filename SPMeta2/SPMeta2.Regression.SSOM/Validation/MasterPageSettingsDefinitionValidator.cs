using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class MasterPageSettingsDefinitionValidator : MasterPageSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<MasterPageSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = webModelHost.HostWeb;

            var assert = ServiceFactory.AssertService
                                      .NewAssert(model, definition, spObject)
                                            .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.SiteMasterPageUrl))
            {
                if (definition.SiteMasterPageUrl.StartsWith("/"))
                {
                    assert.ShouldBeEndOf(m => m.SiteMasterPageUrl, o => o.CustomMasterUrl);
                }
                else
                {
                    // check for ~site/~sitecollection tokens
                    var url = ResolveUrlWithTokens(webModelHost.HostWeb, definition.SiteMasterPageUrl);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.SiteMasterPageUrl);
                        var dstProp = d.GetExpressionValue(def => def.CustomMasterUrl);

                        var isValid = ((string)dstProp.Value).EndsWith((string)url);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = isValid
                        };
                    });
                }
            }
            else
            {
                assert.SkipProperty(m => m.SiteMasterPageUrl, "SiteMasterPageUrl is NULL or empty");
            }

            if (!string.IsNullOrEmpty(definition.SystemMasterPageUrl))
            {
                if (definition.SystemMasterPageUrl.StartsWith("/"))
                {
                    assert.ShouldBeEndOf(m => m.SystemMasterPageUrl, o => o.MasterUrl);
                }
                else
                {
                    // check for ~site/~sitecollection tokens
                    var url = ResolveUrlWithTokens(webModelHost.HostWeb, definition.SystemMasterPageUrl);

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.SystemMasterPageUrl);
                        var dstProp = d.GetExpressionValue(def => def.MasterUrl);

                        var isValid = ((string)dstProp.Value).EndsWith((string)url);

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = isValid
                        };
                    });
                }
            }
            else
            {
                assert.SkipProperty(m => m.SystemMasterPageUrl, "SiteMasterPageUrl is NULL or empty");
            }
        }
    }
}
