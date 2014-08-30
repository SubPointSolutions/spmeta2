using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Exceptions;
using Microsoft.SharePoint;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class JobModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(JobDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var jobDefinition = model.WithAssertAndCast<JobDefinition>("model", value => value.RequireNotNull());

            DeployTimerJob(modelHost, jobDefinition);
        }

        private void DeployTimerJob(object modelHost, JobDefinition jobDefinition)
        {
            if (modelHost is WebApplicationModelHost)
            {
                var webAppModelHost = modelHost as WebApplicationModelHost;
                DeployWebApplicationJob(modelHost, webAppModelHost.HostWebApplication, jobDefinition);
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("JobDefiniton deployment is not supported under host of type:[{0}]", modelHost.GetType()));
            }
        }

        private void DeployWebApplicationJob(object modelHost, SPWebApplication webApp, JobDefinition jobDefinition)
        {
            var jobDefinitions = webApp.JobDefinitions;

            var jobNameInUpperCase = jobDefinition.Name.ToUpper();
            var currentJobInstance = jobDefinitions.FirstOrDefault(j => !string.IsNullOrEmpty(j.Name) && j.Name.ToUpper() == jobNameInUpperCase);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentJobInstance,
                ObjectType = typeof(SPJobDefinition),
                ObjectDefinition = jobDefinition,
                ModelHost = modelHost
            });

            if (currentJobInstance == null)
            {
                Type jobType = null;

                // install one
                var subType = jobDefinition.JobType.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (subType.Length == 2)
                {
                    var typeName = subType[0].Trim();
                    var assemblyName = subType[1].Trim();

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                    var targetAssembly = assemblies.FirstOrDefault(a => a.FullName.Split(',')[0].ToUpper() == assemblyName.ToUpper());
                    jobType = targetAssembly.GetType(typeName);
                }
                else
                {
                    jobType = Type.GetType(jobDefinition.JobType);
                }

                // TODO
                // sort out job host - SPWebApp, SPServer
                currentJobInstance = (SPJobDefinition)Activator.CreateInstance(jobType);
                //, jobDefinition.Name, webApp);

                if (!string.IsNullOrEmpty(jobDefinition.ScheduleString))
                {
                    currentJobInstance.Schedule = SPSchedule.FromString(jobDefinition.ScheduleString);
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentJobInstance,
                    ObjectType = typeof(SPJobDefinition),
                    ObjectDefinition = jobDefinition,
                    ModelHost = modelHost
                });

                currentJobInstance.Update();
            }
            else
            {
                if (!string.IsNullOrEmpty(jobDefinition.ScheduleString))
                {
                    currentJobInstance.Schedule = SPSchedule.FromString(jobDefinition.ScheduleString);
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentJobInstance,
                    ObjectType = typeof(SPJobDefinition),
                    ObjectDefinition = jobDefinition,
                    ModelHost = modelHost
                });

                currentJobInstance.Update();
            }
        }

        #endregion

    }
}
