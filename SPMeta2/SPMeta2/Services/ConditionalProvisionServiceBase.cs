using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.ModelHosts;
using SPMeta2.Models;

namespace SPMeta2.Services
{
    public class IncrementalProvisionServiceBase : PreDeploymentServiceBase
    {
        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            // before model provision
            if (IsRegisteredModel(model))
            {
                AttachToOnProvisionedEvents(model);
                CalculateIncrementalModel(model);
            }
        }

        private void AttachToOnProvisionedEvents(ModelNode model)
        {
            throw new NotImplementedException();
        }

        protected virtual bool IsRegisteredModel(ModelNode model)
        {
            throw new NotImplementedException();
        }

        public void RegisterModel(string key, ModelNode model)
        {


            // save internally

            // try to restore
            // found - make 'diff' AddHost marks

            // attach to events, save per every provision end event
        }

        protected virtual ModelNode RestoreLastProvisionedModelByKey(string key)
        {

        }

        protected virtual ModelNode RestoreLastProvisionedModelByKey(string key)
        {
            throw new NotImplementedException();
        }

        protected virtual void SaveLastProvisionedModelByKey(string key, ModelNode model)
        {
            throw new NotImplementedException();
        }

    }
}
