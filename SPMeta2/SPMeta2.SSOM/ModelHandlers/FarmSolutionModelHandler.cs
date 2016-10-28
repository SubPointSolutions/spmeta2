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

        public override Type TargetType
        {
            get { return typeof(FarmSolutionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var solutionModel = model.WithAssertAndCast<FarmSolutionDefinition>("model", value => value.RequireNotNull());

            DeploySolution(farmModelHost, solutionModel);
        }

        protected SPSolution FindExistingSolution(FarmModelHost modelHost, FarmSolutionDefinition definition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                "Resolving farm solution by SolutionId: [{0}] and Name: [{1}]",
                 new object[]
                    {
                        definition.SolutionId,
                        definition.FileName
                    });

            var farm = modelHost.HostFarm;

            return farm.Solutions.FirstOrDefault(s =>
                s.Name.ToUpper() == definition.FileName.ToUpper() ||
                definition.SolutionId != Guid.Empty && s.SolutionId == definition.SolutionId);
        }

        private void DeploySolution(FarmModelHost modelHost, FarmSolutionDefinition definition)
        {
            var farm = modelHost.HostFarm;
            var existingSolution = FindExistingSolution(modelHost, definition);

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
                RetractSolution(modelHost, definition, existingSolution);
            }

            // should delete?
            if (existingSolution != null && definition.ShouldDelete == true)
            {
                DeleteSolution(modelHost, definition, existingSolution);
            }

            // add solution to the farm
            existingSolution = AddSolution(modelHost, definition);

            // should update?
            if (existingSolution != null && definition.ShouldUpdate == true)
            {
                UpdateSolution(modelHost, definition, existingSolution);
            }

            // should deploy?
            if (existingSolution != null && definition.ShouldDeploy == true)
            {
                DeploySolution(modelHost, definition, existingSolution);
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

        protected virtual void DeploySolution(FarmModelHost modelHost, FarmSolutionDefinition definition, SPSolution existingSolution)
        {
            if (!existingSolution.Deployed)
            {
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying farm solution:[{0}]", existingSolution.Name));

                var isNowDeployment = false;

                if (definition.DeploymentDate.HasValue)
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying solution on date [{0}]", definition.DeploymentDate.Value));

                    existingSolution.Deploy(definition.DeploymentDate.Value,
                                            definition.DeploymentGlobalInstallWPPackDlls,
                                            definition.DeploymentForce);
                }
                else
                {
                    TraceService.Information((int)LogEventId.CoreCalls, string.Format("Deploying solution NOW."));

                    existingSolution.Deploy(DateTime.Now,
                                              definition.DeploymentGlobalInstallWPPackDlls,
                                              definition.DeploymentForce);

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

                        deployed = existingSolution.Deployed;
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
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Farm solution:[{0}] was already deployed.", existingSolution.Name));
            }
        }

        protected virtual void UpdateSolution(FarmModelHost modelHost, FarmSolutionDefinition definition, SPSolution existingSolution)
        {

        }

        protected virtual void DeleteSolution(FarmModelHost modelHost, FarmSolutionDefinition definition, SPSolution existringSolution)
        {
            existringSolution.Delete();
        }

        protected virtual void RetractSolution(FarmModelHost modelHost, FarmSolutionDefinition definition, SPSolution existingSolution)
        {
            TraceService.Information((int)LogEventId.CoreCalls, string.Format("Retracting solution [{0}]", existingSolution.Name));

            if (existingSolution.Deployed)
            {
                var deployed = existingSolution.Deployed;
                existingSolution.Retract(DateTime.Now);

                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Checking .Deployed status to be false"));

                while (deployed)
                {
                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Sleeping [{0}] milliseconds...", SolutionDeploymentTimeoutInMillisecond));
                    Thread.Sleep(SolutionDeploymentTimeoutInMillisecond);

                    TraceService.Information((int)LogEventId.CoreCalls,
                        string.Format("Checkin .Deployed for solution [{0}] in [{1}] milliseconds...",
                            existingSolution.Name, SolutionDeploymentTimeoutInMillisecond));

                    deployed = existingSolution.Deployed;
                }

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

                    jobExists = existingSolution.JobExists;
                }

                TraceService.Information((int)LogEventId.CoreCalls, string.Format(".Deployed and .JobExists are false"));
            }
            else
            {
                TraceService.Information((int)LogEventId.CoreCalls, string.Format("Solution was already retracted."));
            }
        }

        protected virtual SPSolution AddSolution(FarmModelHost modelHost, FarmSolutionDefinition definition)
        {
            var farm = modelHost.HostFarm;

            var existringSolution = FindExistingSolution(modelHost, definition);

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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Farm solution exists");
            }

            return existringSolution;
        }

        #endregion
    }
}
