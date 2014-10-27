using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SPMeta2.Models;
using SPMeta2.Regression.Runners;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
using SPMeta2.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestBase
    {
        public TestContext TestContext { get; set; }

        #region constructors

        public SPMeta2RegresionTestBase()
        {
            ProvisionGenerationCount = 1;

            ProvisionRunners = new List<ProvisionRunnerBase>();
            ProvisionRunnerAssemblies = new List<string>();

            EnableDefinitionValidation = true;

            InitConfig();
        }

        #endregion

        #region properties

        public int ProvisionGenerationCount { get; set; }

        protected void InitLazyRunnerConnection()
        {
            InitRunnerImplementations();

            foreach (var runner in ProvisionRunners)
                runner.InitLazyRunnerConnection();
        }

        protected void DisposeLazyRunnerConnection()
        {
            foreach (var runner in ProvisionRunners)
                runner.DisposeLazyRunnerConnection();
        }

        protected bool EnableDefinitionValidation { get; set; }

        public List<ProvisionRunnerBase> ProvisionRunners { get; set; }
        public List<string> ProvisionRunnerAssemblies { get; set; }

        #endregion

        protected virtual void InitConfig()
        {
            InitRunnerTypes();
            InitRunnerImplementations();
        }

        private bool _hasInit = false;

        protected virtual void InitRunnerImplementations()
        {
            if (_hasInit) return;

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

            _hasInit = true;
        }

        protected virtual void InitRunnerTypes()
        {
            var runnerLibraries = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.RunnerLibraries);

            Trace.WriteLine(string.Format("Testing with runner libraries: [{0}]", runnerLibraries));

            if (!string.IsNullOrEmpty(runnerLibraries))
            {
                var libs = runnerLibraries.Split(',');

                foreach (var lib in libs)
                    ProvisionRunnerAssemblies.Add(lib);
            }

            if (ProvisionRunnerAssemblies.Count == 0)
                throw new ArgumentException("Cannot find any test runners. Please configure test runners via SPMeta2.Regression.Environment.ps1 script.");

            // Test runners should be managed via SPMeta2.Regression.Environment.ps1
            // Manual adding is for internal use only.

            //  ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.O365.dll");
            //  ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.CSOM.dll");
            //  ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.SSOM.dll");
        }

        protected ProvisionRunnerBase CurrentProvisionRunner;

        protected void WithProvisionRunnerContext(Action<ProvisionRunnerContext> action)
        {
            foreach (var provisionRunner in ProvisionRunners)
            {
                var type = provisionRunner.GetType().FullName;

                provisionRunner.ProvisionGenerationCount = ProvisionGenerationCount;
                provisionRunner.EnableDefinitionValidation = EnableDefinitionValidation;

                CurrentProvisionRunner = provisionRunner;

                Trace.WriteLine(string.Format("[INF]    Testing with runner impl: [{0}]", type));
                Trace.WriteLine(string.Format("[INF]        - ProvisionGenerationCount: [{0}]", ProvisionGenerationCount));
                Trace.WriteLine(string.Format("[INF]        - EnableDefinitionValidation: [{0}]", EnableDefinitionValidation));

                action(new ProvisionRunnerContext
                {
                    Runner = provisionRunner
                });
            }
        }
    }


    public class ProvisionRunnerContext
    {
        public ProvisionRunnerBase Runner { get; set; }

    }
}
