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

        public string Name { get; set; }
        public bool EnableDefinitionValidation { get; set; }

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

        }

        public virtual void DeployWebApplicationModel(ModelNode model)
        {

        }

        public virtual void DeploySiteModel(ModelNode model)
        {

        }

        public virtual void DeployWebModel(ModelNode model)
        {

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
