using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Services;

namespace SPMeta2.Regression.CSOMServices
{
    public class CSOMProvisionService : ModelProvisionServiceBase
    {
        #region constructors

        public CSOMProvisionService(ModelServiceBase modelService, WebModel model)
        {
            Model = model;
            ModelService = modelService;
        }

        #endregion

        #region methods

        protected override void InternalDeploySiteModel(ProvisionServiceContext context)
        {
            throw new NotImplementedException();
        }

        protected override void InternalDeployRootWebModel(ProvisionServiceContext context)
        {
            throw new NotImplementedException();
        }

        protected override void InternalDeployWebModel(ProvisionServiceContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
