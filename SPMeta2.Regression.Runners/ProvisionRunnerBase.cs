using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;

namespace SPMeta2.Regression.Runners
{
    public class ProvisionRunnerBase
    {
        #region properties

        public bool EnableDefinitionValidation { get; set; }

        #endregion

        #region methods

        public virtual void DeploySiteModel(ModelNode model)
        {

        }

        public virtual void DeployWebModel(ModelNode model)
        {

        }

        #endregion
    }
}
