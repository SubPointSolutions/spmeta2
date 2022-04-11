using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WebApplicationDefinitionValidator : WebApplicationModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            ValidateWebApplication(farmModelHost, farmModelHost.HostFarm, webApplicationDefinition);
        }

        private void ValidateWebApplication(FarmModelHost farmModelHost, Microsoft.SharePoint.Administration.SPFarm sPFarm, WebApplicationDefinition webApplicationDefinition)
        {
            var typedModelHost = farmModelHost;
            var definition = webApplicationDefinition;

            var spObject = LookupWebApplication(definition);

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject);

            // skip all these props
            // they are set only once while web app is created
            // such validation needs to be implemented in two modes
            // 1) web app is created - validate these props
            // 2) web app exists (find via port), and then only a set of props is validated
            // TODO for next iteratins, for now chacking only required BrowserFileHandling related props

            assert.SkipProperty(o => o.AllowAnonymousAccess);
            assert.SkipProperty(o => o.ApplicationPoolId);
            assert.SkipProperty(o => o.ApplicationPoolPassword);
            assert.SkipProperty(o => o.ApplicationPoolUsername);
            assert.SkipProperty(o => o.CreateNewDatabase);
            assert.SkipProperty(o => o.DatabaseName);
            assert.SkipProperty(o => o.DatabaseServer);
            assert.SkipProperty(o => o.HostHeader);
            assert.SkipProperty(o => o.ManagedAccount);
            assert.SkipProperty(o => o.UseNTLMExclusively);

            // validate
            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Port);
                var isValid = d.GetResponseUri(SPUrlZone.Default).Port == s.Port;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.AllowedInlineDownloadedMimeTypes);
                var isValid = true;

                foreach (var value in s.AllowedInlineDownloadedMimeTypes)
                {
                    if (!d.AllowedInlineDownloadedMimeTypes.Contains(value))
                    {
                        isValid = false;
                        break;
                    }
                }

                // only .Count if ShouldOverrideAllowedInlineDownloadedMimeTypes
                if (s.ShouldOverrideAllowedInlineDownloadedMimeTypes == true)
                {
                    isValid = isValid && (d.AllowedInlineDownloadedMimeTypes.Count == s.AllowedInlineDownloadedMimeTypes.Count);
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.ShouldOverrideAllowedInlineDownloadedMimeTypes);
                var isValid = true;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });
        }
    }
}
