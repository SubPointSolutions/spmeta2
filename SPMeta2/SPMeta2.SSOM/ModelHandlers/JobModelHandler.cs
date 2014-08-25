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
                var jobDefinitions = (modelHost as WebApplicationModelHost).HostWebApplication.JobDefinitions;
                DeployJob(jobDefinitions, jobDefinition);
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("JobDefiniton deployment is not supported under host of type:[{0}]", modelHost.GetType()));
            }
        }

        private void DeployJob(SPJobDefinitionCollection jobDefinitions, JobDefinition jobDefinition)
        {
            var jobNameInUpperCase = jobDefinition.Name.ToUpper();
            var currentJob = jobDefinitions.FirstOrDefault(j => !string.IsNullOrEmpty(j.Name) && j.Name.ToUpper() == jobNameInUpperCase);

            if (currentJob == null)
            {
                // install one
            }
            else
            {
                // update one
            }
        }

        #endregion

    }
}
