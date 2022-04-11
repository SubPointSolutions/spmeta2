using SPMeta2.ModelHosts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Interfaces
{
    public interface IProvisionService
    {
        #region methods

        void DeployModel(ModelHostBase modelHost, ModelNode model);

        #endregion

    }
}
