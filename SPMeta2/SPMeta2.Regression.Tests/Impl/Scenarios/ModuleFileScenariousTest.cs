using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ModuleFileScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public ModuleFileScenariousTest()
        {
            ProvisionGenerationCount = 2;
        }

        #endregion

        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region default

        protected void DeployModuleFile(long lenght)
        {
            TestRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Content = Rnd.Content(lenght);

            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeploy_1Mb_ModuleFile()
        {
            DeployModuleFile(1024 * 1024 * 1);
        }

        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_5Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 5);
        //}


        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_15Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 15);
        //}

        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_25Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 25);
        //}

        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_50Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 50);
        //}

        #endregion
    }
}
