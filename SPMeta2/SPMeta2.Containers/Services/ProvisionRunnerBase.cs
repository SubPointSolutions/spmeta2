using System;
using System.Linq;
using SPMeta2.Models;

namespace SPMeta2.Containers.Services
{
    public class ProvisionRunnerBase
    {
        #region constructors

        public ProvisionRunnerBase()
        {
            ProvisionGenerationCount = 1;
        }

        #endregion

        #region properties

        public string Name { get; set; }

        public bool EnableDefinitionProvision { get; set; }
        public bool EnableDefinitionValidation { get; set; }

        public int ProvisionGenerationCount { get; set; }

        #endregion

        #region methods

        public virtual string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var targetAssembly = assemblies.FirstOrDefault(a => a.FullName.Split(',')[0].ToUpper() == assemblyName.ToUpper());
            var targetType = targetAssembly.GetType(typeName);

            return targetType.AssemblyQualifiedName;
        }

        public virtual void DeployFarmModel(ModelNode model)
        {
            throw new NotImplementedException();
        }

        public virtual void DeployWebApplicationModel(ModelNode model)
        {
            throw new NotImplementedException();
        }

        public virtual void DeploySiteModel(ModelNode model)
        {
            throw new NotImplementedException();
        }

        public virtual void DeployWebModel(ModelNode model)
        {
            throw new NotImplementedException();
        }

        public virtual void DeployListModel(ModelNode model)
        {
            throw new NotImplementedException();
        }

        #endregion

        public virtual void InitLazyRunnerConnection()
        {

        }

        public virtual void DisposeLazyRunnerConnection()
        {

        }

        
    }
}
