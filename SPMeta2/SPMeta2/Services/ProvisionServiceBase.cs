using SPMeta2.Services.Impl;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Utils;
using System;
using SPMeta2.Definitions;

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

            var validationServiceTypes =
                ReflectionUtils.GetTypesFromAssembly<PreDeploymentValidationServiceBase>(
                        typeof(FieldDefinition).Assembly);

            foreach (var validationServiceType in validationServiceTypes)
            {
                var service = Activator.CreateInstance(validationServiceType) as PreDeploymentValidationServiceBase;

                if (service != null)
                {
                    PreDeploymentServices.Add(service);
                }
            }
        }

        private void InitDefaultPreDeploymentServices()
        {

        }

        #endregion
    }
}
