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
    public class DeveloperDashboardSettingsDefinitionValidator : DeveloperDashboardSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var ddsDefinition = model.WithAssertAndCast<DeveloperDashboardSettingsDefinition>("model", value => value.RequireNotNull());

            ValidateDefinition(farmModelHost, farmModelHost.HostFarm, ddsDefinition);
        }

        private void ValidateDefinition(FarmModelHost farmModelHost,
            SPFarm farm,
            DeveloperDashboardSettingsDefinition definition)
        {
            var spObject = GetCurrentSettings();
            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.DisplayLevel))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.DisplayLevel);
                    var dstProp = d.GetExpressionValue(ct => ct.DisplayLevel);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = dstProp.Value.ToString().ToUpper() == (srcProp.Value.ToString().ToUpper())
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.DisplayLevel);
            }
        }
    }
}
