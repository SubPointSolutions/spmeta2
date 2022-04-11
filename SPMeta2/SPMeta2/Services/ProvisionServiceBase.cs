using SPMeta2.Services.Impl;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Utils;
using System;
using System.Linq;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Interfaces;

namespace SPMeta2.Services
{
    public class ProvisionServiceBase : ModelServiceBase, IProvisionService
    {
        #region constructors

        public ProvisionServiceBase()
        {
            InitDefaultPreDeploymentServices();
            InitDefaultPostDeploymentServices();
        }

        protected void InitDefaultPostDeploymentServices()
        {
            InitDefaultPostDeploymentServices(typeof(FieldDefinition).Assembly);
        }

        protected void InitDefaultPostDeploymentServices(Assembly assembly)
        {
            // TODO
        }

        protected void InitDefaultPreDeploymentServices()
        {
            InitDefaultPreDeploymentServices(typeof(FieldDefinition).Assembly);
        }

        protected void InitDefaultPreDeploymentServices(Assembly assembly)
        {
            // adding default required prop validation on definitions
            // https://github.com/SubPointSolutions/spmeta2/issues/422

            var validationServiceTypes = ReflectionUtils.GetTypesFromAssembly<PreDeploymentValidationServiceBase>(assembly);

            foreach (var validationServiceType in validationServiceTypes)
            {
                var exists = PreDeploymentServices.Any(s => s.GetType() == validationServiceType);

                if (!exists)
                {
                    var service = Activator.CreateInstance(validationServiceType) as PreDeploymentValidationServiceBase;

                    if (service != null)
                    {
                        PreDeploymentServices.Add(service);
                    }
                }
            }
        }

        #endregion
    }
}
