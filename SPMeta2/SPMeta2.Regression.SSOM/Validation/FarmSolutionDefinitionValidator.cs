using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Containers.Assertion;
using SPMeta2.Extensions;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FarmSolutionDefinitionValidator : FarmSolutionModelHandler
    {
        #region constructors
        public FarmSolutionDefinitionValidator()
        {

        }

        #endregion

        #region methods
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FarmSolutionDefinition>("model", value => value.RequireNotNull());
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());

            var solution = FindExistingSolution(farmModelHost, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, definition, solution);

            if (definition.ShouldDelete == true
                && !definition.ShouldDeploy.HasValue)
            {
                // DELETE OPERATION

                // solution has to be NULL
                assert.ShouldBeNull(solution);

                assert.SkipProperty(m => m.UpgradeDate, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentDate, "ShouldDelete = true");

                assert.SkipProperty(m => m.DeploymentForce, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentGlobalInstallWPPackDlls, "ShouldDelete = true");
                assert.SkipProperty(m => m.LCID, "ShouldDelete = true");

                assert.SkipProperty(m => m.Content, "ShouldDelete = true");
                assert.SkipProperty(m => m.FileName, "ShouldDelete = true");
                assert.SkipProperty(m => m.SolutionId, "ShouldDelete = true");

                assert.SkipProperty(m => m.ShouldAdd, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDelete, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDeploy, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldRetract, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldUpgrade, "ShouldDelete = true");

                return;
            }

            if (definition.ShouldRetract == true)
            {
                // RETRACT OPERATION
                assert.ShouldNotBeNull(solution);

                assert.SkipProperty(m => m.UpgradeDate, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentDate, "ShouldDelete = true");

                assert.SkipProperty(m => m.DeploymentForce, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentGlobalInstallWPPackDlls, "ShouldDelete = true");
                assert.SkipProperty(m => m.LCID, "ShouldDelete = true");

                assert.SkipProperty(m => m.Content, "ShouldDelete = true");
                assert.SkipProperty(m => m.FileName, "ShouldDelete = true");
                assert.SkipProperty(m => m.SolutionId, "ShouldDelete = true");

                assert.SkipProperty(m => m.ShouldAdd, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDelete, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDeploy, "ShouldDelete = true");
                //assert.SkipProperty(m => m.ShouldRetract, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldUpgrade, "ShouldDelete = true");

                // should be deployed state
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ShouldRetract);

                    var isValid = d.Deployed == false
                                  && definition.HasPropertyBagValue("HadRetractHit");

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                return;
            }

            if (definition.ShouldUpgrade == true)
            {
                // UPGRADE OPERATION
                assert.ShouldNotBeNull(solution);

                assert.SkipProperty(m => m.UpgradeDate, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentDate, "ShouldDelete = true");

                assert.SkipProperty(m => m.DeploymentForce, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentGlobalInstallWPPackDlls, "ShouldDelete = true");
                assert.SkipProperty(m => m.LCID, "ShouldDelete = true");

                assert.SkipProperty(m => m.Content, "ShouldDelete = true");
                assert.SkipProperty(m => m.FileName, "ShouldDelete = true");
                assert.SkipProperty(m => m.SolutionId, "ShouldDelete = true");

                assert.SkipProperty(m => m.ShouldAdd, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDelete, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDeploy, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldRetract, "ShouldDelete = true");
                //assert.SkipProperty(m => m.ShouldUpgrade, "ShouldDelete = true");

                // should be deployed state
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ShouldUpgrade);

                    var isValid = definition.HasPropertyBagValue("HadUpgradetHit"); 

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                return;
            }

            if (definition.ShouldDeploy == true)
            {
                // DEPLOY OPERATION
                assert.ShouldNotBeNull(solution);

                assert.SkipProperty(m => m.UpgradeDate, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentDate, "ShouldDelete = true");

                assert.SkipProperty(m => m.DeploymentForce, "ShouldDelete = true");
                assert.SkipProperty(m => m.DeploymentGlobalInstallWPPackDlls, "ShouldDelete = true");
                assert.SkipProperty(m => m.LCID, "ShouldDelete = true");

                assert.SkipProperty(m => m.Content, "ShouldDelete = true");
                assert.SkipProperty(m => m.FileName, "ShouldDelete = true");
                assert.SkipProperty(m => m.SolutionId, "ShouldDelete = true");

                assert.SkipProperty(m => m.ShouldAdd, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldDelete, "ShouldDelete = true");
                //assert.SkipProperty(m => m.ShouldDeploy, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldRetract, "ShouldDelete = true");
                assert.SkipProperty(m => m.ShouldUpgrade, "ShouldDelete = true");

                // should be deployed state
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ShouldDeploy);

                    var isValid = d.Deployed
                                && definition.HasPropertyBagValue("HadDeploymentHit"); 

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                return;
            }

            assert.ShouldNotBeNull(solution)
                //.ShouldBeEqual(m => m.FileName, o => o.Name)
                .SkipProperty(m => m.FileName, "Skipping Name props. It might be different as the same SolutionId could be deployment in the farm.")
                .ShouldBeEqual(m => m.SolutionId, o => o.SolutionId)
                .SkipProperty(m => m.Content, "Skipping Content property.");

            assert.SkipProperty(m => m.ShouldAdd, "ShouldAdd");
            assert.SkipProperty(m => m.ShouldDelete, "ShouldDelete");
            assert.SkipProperty(m => m.ShouldDeploy, "ShouldDeploy");
            assert.SkipProperty(m => m.ShouldRetract, "ShouldRetract");
            assert.SkipProperty(m => m.ShouldUpgrade, "ShouldUpgrade");

            assert.SkipProperty(m => m.DeploymentForce);
            assert.SkipProperty(m => m.DeploymentGlobalInstallWPPackDlls);

            if (definition.LCID.HasValue)
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.LCID, "LCID is NULL");
            }


            if (definition.DeploymentDate.HasValue)
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.DeploymentDate, "DeploymentDate is NULL");
            }

            if (definition.UpgradeDate.HasValue)
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.UpgradeDate, "UpgradeDate is NULL");
            }
        }
        #endregion
    }
}
