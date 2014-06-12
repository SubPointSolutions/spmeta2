using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Regression.Runners;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionTestBase()
        {
            ProvisionRunners = new List<ProvisionRunnerBase>();
            ProvisionRunnerAssemblies = new List<string>();

            InitConfig();
        }

        #endregion

        #region properties

        public List<ProvisionRunnerBase> ProvisionRunners { get; set; }
        public List<string> ProvisionRunnerAssemblies { get; set; }

        #endregion

        private void InitConfig()
        {
            InitRunnerTypes();
            InitRunnerImplementations();
        }

        private void InitRunnerImplementations()
        {
            foreach (var asmFileName in ProvisionRunnerAssemblies)
            {
                var asmImpl = Assembly.LoadFrom(asmFileName);

                var types = ReflectionUtils.GetTypesFromAssembly<ProvisionRunnerBase>(asmImpl);

                foreach (var type in types)
                {
                    var runnerImpl = Activator.CreateInstance(type) as ProvisionRunnerBase;

                    ProvisionRunners.Add(runnerImpl);
                }
            }
        }

        private void InitRunnerTypes()
        {
            ProvisionRunnerAssemblies.Add("SPMeta2.Regression.CSOMRunner.dll");
        }

        protected void WithProvisionRunners(Action<ProvisionRunnerBase> action)
        {
            foreach (var provisionRunner in ProvisionRunners)
                action(provisionRunner);
        }
    }
}
