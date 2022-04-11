using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Threading;
using SPMeta2.Extensions;
using SPMeta2.Exceptions;
using System.Collections.ObjectModel;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FarmSolutionModelHandler : SSOMModelHandlerBase
    {
        #region constructors

        public FarmSolutionModelHandler()
        {

        }

        #endregion

        #region static

        static FarmSolutionModelHandler()
        {
            SolutionDeploymentTimeoutInMillisecond = 1000;
        }

        #endregion

        #region properties

        public static int SolutionDeploymentTimeoutInMillisecond { get; set; }
        protected bool IsQARun { get; set; }

        public override Type TargetType
        {
            get { return typeof(FarmSolutionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
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

            var solutionModel = model.WithAssertAndCast<FarmSolutionDefinition>("model", value => value.RequireNotNull());

            DeploySolutionDefinition(modelHost, farm, webApp, solutionModel);
        }

        protected SPSolution FindExistingSolution(object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                "Resolving farm solution by SolutionId: [{0}] and Name: [{1}]",
                 new object[]
                    {
                        definition.SolutionId,
                        definition.FileName
                    });

            // always get anew instance of the farm
            // that would refresh the .Solution colleciton with the right state of the solutions
            farm = SPFarm.Local;

            return farm.Solutions.FirstOrDefault(s =>
                s.Name.ToUpper() == definition.FileName.ToUpper() ||
                definition.SolutionId != Guid.Empty && s.SolutionId == definition.SolutionId);
        }

        private void DeploySolutionDefinition(object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition)
        {
            var existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingSolution,
                ObjectType = typeof(SPSolution),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            // should retract?
            if (existingSolution != null && definition.ShouldRetract == true)
            {
                RetractSolution(modelHost, farm, webApplication, definition, existingSolution);
                existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
            }
            else if (existingSolution == null && definition.ShouldRetract == true)
            {
                // set up flag for nn existing solution
                if (IsQARun)
                    definition.SetPropertyBagValue("HadRetractHit");
            }


            // should delete?
            if (existingSolution != null && definition.ShouldDelete == true)
            {
                DeleteSolution(modelHost, farm, webApplication, definition, existingSolution);
                existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
            }

            // should add?
            if (definition.ShouldAdd == true)
            {
                // add solution to the farm
                existingSolution = AddSolution(modelHost, farm, webApplication, definition);
            }

            if (existingSolution != null && definition.ShouldUpgrade == true)
            {
                // should upgrade?
                UpgradeSolution(modelHost, farm, webApplication, definition, existingSolution);
            }
            else if (existingSolution != null && definition.ShouldDeploy == true)
            {
                // should deploy?
                DeploySolution(modelHost, farm, webApplication, definition, existingSolution);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingSolution,
                ObjectType = typeof(SPSolution),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        protected virtual void DeploySolution(object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition,
            SPSolution existingSolution)
        {
            definition.SetPropertyBagValue("HadDeploymentHit");

            var webAppCollection = new Collection<SPWebApplication>();

            if (webApplication != null)
                webAppCollection.Add(webApplication);

            // either not deploed or not deployed to a particular web app
            if (!existingSolution.Deployed ||
                (webAppCollection.Any() && !existingSolution.DeployedWebApplications.Contains(webAppCollection.First())))
            {
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying farm solution:[{0}]", existingSolution.Name));

                var isNowDeployment = false;

                if (definition.DeploymentDate.HasValue)
                {

                    if (webAppCollection.Any())
                    {
                        TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying solution to web app on date [{0}]", definition.DeploymentDate.Value));


                        existingSolution.Deploy(definition.DeploymentDate.Value,
                                             definition.DeploymentGlobalInstallWPPackDlls,
                                             webAppCollection,
                                             definition.DeploymentForce);
                    }
                    else
                    {
                        TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying solution globally on date [{0}]", definition.DeploymentDate.Value));

                        existingSolution.Deploy(definition.DeploymentDate.Value,
                                                definition.DeploymentGlobalInstallWPPackDlls,
                                                definition.DeploymentForce);
                    }
                }
                else
                {
                    if (webAppCollection.Any())
                    {
                        TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying solution to web app NOW."));

                        existingSolution.Deploy(DateTime.Now,
                                                  definition.DeploymentGlobalInstallWPPackDlls,
                                                   webAppCollection,
                                                  definition.DeploymentForce);
                    }
                    else
                    {
                        TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying solution globaly NOW."));

                        existingSolution.Deploy(DateTime.Now,
                                                  definition.DeploymentGlobalInstallWPPackDlls,
                                                  definition.DeploymentForce);
                    }

                    isNowDeployment = true;
                }

                var deployed = existingSolution.DeploymentState != SPSolutionDeploymentState.NotDeployed;

                if (isNowDeployment)
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .Deployed status to be true"));

                    while (!deployed)
                    {
                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                        Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);

                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Checkin .Deployed for solution [{0}] in [{1}] milliseconds...",
                            existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                        existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                        deployed = existingSolution.DeploymentState != SPSolutionDeploymentState.NotDeployed;
                    }

                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .Deployed status to be false"));
                    var jobExists = existingSolution.JobExists;

                    while (jobExists)
                    {
                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                        Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);


                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Checkin .JobExists for solution [{0}] in [{1}] milliseconds...",
                            existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                        existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                        jobExists = existingSolution.JobExists;
                    }

                    TraceService.Information((int)LogEventId.CoreCalls, string.Format(".Deployed is true AND .JobExists is false"));
                }
                else
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Future deployment. Passing wait."));
                }
            }
            else
            {
                if (webAppCollection.Any())
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Farm solution:[{0}] was already deployed to web app", existingSolution.Name));
                }   
                else
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Farm solution:[{0}] was already deployed.", existingSolution.Name));
                }
            }
        }

        protected virtual void UpgradeSolution(object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition,
            SPSolution existingSolution)
        {
            definition.SetPropertyBagValue("HadUpgradetHit");

            // ensure deployment state first
            TraceService.Information((int)LogEventId.CoreCalls, string.Format("Ensuring deployment state. Solution must be deployed before upgrading."));
            DeploySolution(modelHost, farm, webApplication, definition, existingSolution);

            // upgrade
            var tmpWspDirectory = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(definition.FileName), Guid.NewGuid().ToString("N"));
            var tmpWspDirectoryPath = Path.Combine(Path.GetTempPath(), tmpWspDirectory);

            Directory.CreateDirectory(tmpWspDirectoryPath);

            var tmpWspFilPath = Path.Combine(tmpWspDirectoryPath, definition.FileName);
            File.WriteAllBytes(tmpWspFilPath, definition.Content);

            var isNowDeployment = false;

            if (definition.UpgradeDate.HasValue)
            {
                existingSolution.Upgrade(tmpWspFilPath, definition.UpgradeDate.Value);
            }
            else
            {
                existingSolution.Upgrade(tmpWspFilPath, DateTime.Now);
                isNowDeployment = true;
            }

            var deployed = existingSolution.Deployed;

            if (isNowDeployment)
            {
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .Deployed status to be true"));

                while (!deployed)
                {
                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                    Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);

                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Checkin .Deployed for solution [{0}] in [{1}] milliseconds...",
                        existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                    existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                    deployed = existingSolution.DeploymentState != SPSolutionDeploymentState.NotDeployed;
                }

                existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .Deployed status to be false"));
                var jobExists = existingSolution.JobExists;

                while (jobExists)
                {
                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                    Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);


                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Checkin .JobExists for solution [{0}] in [{1}] milliseconds...",
                        existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                    existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                    jobExists = existingSolution.JobExists;
                }

                TraceService.Information((int)LogEventId.CoreCalls, string.Format(".Deployed is true AND .JobExists is false"));
            }
            else
            {
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Future upgrade. Passing wait."));
            }
        }

        protected virtual void DeleteSolution(object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition,
            SPSolution existingSolution)
        {
            if (IsQARun)
                definition.SetPropertyBagValue("HadDeleteHit");

            TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deleting solution [{0}]", existingSolution.Name));
            existingSolution.Delete();
        }

        protected virtual void RetractSolution(
            object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition,
            SPSolution existingSolution)
        {
            var webAppCollection = new Collection<SPWebApplication>();

            if (webApplication != null)
                webAppCollection.Add(webApplication);

            if (IsQARun)
                definition.SetPropertyBagValue("HadRetractHit");

            TraceService.Information((int)LogEventId.CoreCalls, string.Format("Retracting solution [{0}]", existingSolution.Name));

            if (existingSolution.Deployed)
            {
                var retracted = existingSolution.DeploymentState == SPSolutionDeploymentState.NotDeployed;

                if (webAppCollection.Any())
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Retracting solution from web application [{0}]", existingSolution.Name));
                    existingSolution.Retract(DateTime.Now, webAppCollection);
                }
                else
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Retracting solution from the farm [{0}]", existingSolution.Name));
                    existingSolution.Retract(DateTime.Now);
                }

                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .Deployed status to be false"));

                if (webAppCollection.Any())
                {
                    // this is bad but we don't expext more than one web app here
                    var webApp = webAppCollection.First();

                    while (existingSolution.DeployedWebApplications.Contains(webApp))
                    {
                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                        Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);

                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Checkin .Deployed for solution [{0}] in [{1}] milliseconds...",
                                existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                        existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                        retracted = existingSolution.DeploymentState == SPSolutionDeploymentState.NotDeployed;
                    }
                }
                else
                {
                    while (!retracted)
                    {
                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                        Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);

                        TraceService.Information((int)LogEventId.CoreCalls,
                            string.Format("Checkin .Deployed for solution [{0}] in [{1}] milliseconds...",
                                existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                        existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                        retracted = existingSolution.DeploymentState == SPSolutionDeploymentState.NotDeployed;
                    }
                }

                existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);

                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .JobExists status to be false"));
                var jobExists = existingSolution.JobExists;

                while (jobExists)
                {
                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                    Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);


                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Checkin .JobExists for solution [{0}] in [{1}] milliseconds...",
                            existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                    existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);
                    jobExists = existingSolution.JobExists;
                }

                existingSolution = FindExistingSolution(modelHost, farm, webApplication, definition);

                TraceService.Information((int)LogEventId.CoreCalls, string.Format(".Deployed and .JobExists are false"));
            }
            else
            {
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Solution was already retracted."));
            }
        }

        protected virtual SPSolution AddSolution(
            object modelHost,
            SPFarm farm,
            SPWebApplication webApplication,
            FarmSolutionDefinition definition)
        {
            if (IsQARun)
                definition.SetPropertyBagValue("HadAddHit");

            TraceService.Information((int)LogEventId.CoreCalls, string.Format("Adding solution [{0}]", definition.FileName));

            var existringSolution = FindExistingSolution(modelHost, farm, webApplication, definition);

            var tmpWspDirectory = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(definition.FileName), Guid.NewGuid().ToString("N"));
            var tmpWspDirectoryPath = Path.Combine(Path.GetTempPath(), tmpWspDirectory);

            Directory.CreateDirectory(tmpWspDirectoryPath);

            var tmpWspFilPath = Path.Combine(tmpWspDirectoryPath, definition.FileName);
            File.WriteAllBytes(tmpWspFilPath, definition.Content);

            if (existringSolution == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new farm solution");

                if (definition.LCID.HasValue)
                {
                    existringSolution = farm.Solutions.Add(tmpWspFilPath, (uint)definition.LCID);
                }
                else
                {
                    existringSolution = farm.Solutions.Add(tmpWspFilPath);
                }
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Farm solution exists. No need to add.");
            }

            return existringSolution;
        }

        #endregion
    }
}
