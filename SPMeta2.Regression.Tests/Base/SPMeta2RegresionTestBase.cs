using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SPMeta2.Regression.Runners;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
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

            EnableDefinitionValidation = true;

            InitConfig();
        }

        #endregion

        #region properties

        public bool EnableDefinitionValidation { get; set; }

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
            var runnerLibraries = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.RunnerLibraries);

            Trace.WriteLine(string.Format("Testing with runner libraries: [{0}]", runnerLibraries));

            //if (!string.IsNullOrEmpty(runnerLibraries))
            //{
            //    var libs = runnerLibraries.Split(',');

            //    foreach (var lib in libs)
            //        ProvisionRunnerAssemblies.Add(lib);
            //}

            ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.O365.dll");
            //ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.CSOM.dll");
            //ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.SSOM.dll");
        }

        protected void WithProvisionRunners(Action<ProvisionRunnerBase> action)
        {
            foreach (var provisionRunner in ProvisionRunners)
            {
                var type = provisionRunner.GetType().FullName;

                provisionRunner.EnableDefinitionValidation = EnableDefinitionValidation;

                Trace.WriteLine(string.Format("Testing with runner impl: [{0}]", type));
                action(provisionRunner);
            }
        }
    }
}
