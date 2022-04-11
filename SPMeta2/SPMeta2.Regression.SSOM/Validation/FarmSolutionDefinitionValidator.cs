using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Containers.Assertion;
using SPMeta2.Extensions;
using Microsoft.SharePoint.Administration;
using SPMeta2.Exceptions;

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

            SPFarm farm = null;
            SPWebApplication webApp = null;

            if (modelHost is WebApplicationModelHost)
            {
                farm = (modelHost as WebApplicationModelHost).HostWebApplication.Farm;
                webApp = (modelHost as WebApplicationModelHost).HostWebApplication;
            }
            else if (modelHost is FarmModelHost)
            {
                farm = (modelHost as FarmModelHost).HostFarm;
                webApp = null;
            }
            else
            {
                throw new SPMeta2Exception(
                    string.Format("Unsupported model host type:[{0}]", modelHost.GetType()));
            }

            var solution = FindExistingSolution(modelHost, farm, webApp, definition);

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

                CheckWebApplicationDeployment(modelHost, farm, webApp, solution, definition);

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

                    var isValid = false;

                    if (webApp == null)
                    {
                        isValid = d.Deployed == false
                                       && definition.HasPropertyBagValue("HadRetractHit");
                    }
                    else
                    {
                        isValid = !solution.DeployedWebApplications.Contains(webApp)
                                   && definition.HasPropertyBagValue("HadRetractHit");
                    }

                    if (isValid == false)
                    {

                    }

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

                CheckWebApplicationDeployment(modelHost, farm, webApp, solution, definition);

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

                    if (webApp == null)
                    {
                        isValid = d.Deployed == true
                                       && definition.HasPropertyBagValue("HadDeploymentHit");
                    }
                    else
                    {
                        isValid = solution.DeployedWebApplications.Contains(webApp)
                                   && definition.HasPropertyBagValue("HadDeploymentHit");
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

                CheckWebApplicationDeployment(modelHost, farm, webApp, solution, definition);

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

        private void CheckWebApplicationDeployment(object modelHost, SPFarm farm, SPWebApplication webApp, SPSolution solution, FarmSolutionDefinition definition)
        {
            // might come from the deleting operation
            if (solution != null)
            {
                // web app scope deployment
                if (webApp != null)
                {
                    if (!solution.Deployed)
                        return;

                    if (solution.DeploymentState == SPSolutionDeploymentState.GlobalDeployed
                        || solution.DeploymentState == SPSolutionDeploymentState.NotDeployed)
                    {
                        throw new SPMeta2Exception(
                            string.Format("Solution is not expected to have deployment state:[{0}]",
                            solution.DeploymentState));
                    }

                    if (solution.DeployedWebApplications.Count == 0)
                    {
                        throw new SPMeta2Exception("Web scoped solution is expected to be deployed under at least one web application");
                    }

                    if (!solution.DeployedWebApplications.Contains(webApp))
                    {
                        throw new SPMeta2Exception(
                            string.Format("Web scoped solution is expected to be deployed under web application:[{0}]", webApp));
                    }
                }
                else
                {
                    // farm, global deployment

                    if (!solution.Deployed)
                        return;

                    if (solution.DeploymentState == SPSolutionDeploymentState.WebApplicationDeployed
                        || solution.DeploymentState == SPSolutionDeploymentState.NotDeployed)
                    {
                        throw new SPMeta2Exception(
                            string.Format("Farm scoped solution is not expected to have deployment state:[{0}]",
                            solution.DeploymentState));
                    }
                }
            }
        }
        #endregion
    }
}
