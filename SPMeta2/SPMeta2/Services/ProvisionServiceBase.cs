using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Services.Impl;

namespace SPMeta2.Services
{
    public class ProvisionServiceBase : ModelServiceBase
    {
        #region constructors

        public ProvisionServiceBase()
        {
            InitDefaultPreDeploymentServices();
            InitDefaultPostDeploymentServices();
        }

        private void InitDefaultPostDeploymentServices()
        {
            // adding default required prop validation on definitions
            // https://github.com/SubPointSolutions/spmeta2/issues/422

            PreDeploymentServices.Add(new DefaultRequiredPropertiesValidationService());
            PreDeploymentServices.Add(new DefaultXmlBasedPropertiesValidationService());
            PreDeploymentServices.Add(new DefaultVersionBasedPropertiesValidationService());
        }

        private void InitDefaultPreDeploymentServices()
        {

        }

        #endregion
    }
}
