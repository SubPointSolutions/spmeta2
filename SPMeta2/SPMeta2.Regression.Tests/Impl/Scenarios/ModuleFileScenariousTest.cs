using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ModuleFileScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public ModuleFileScenariousTest()
        {
            RegressionService.ProvisionGenerationCount = 2;
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

        #region size

        protected void DeployModuleFile(long lenght)
        {
            TestRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Content = Rnd.Content(lenght);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Size")]
        public void CanDeploy_2Mb_ModuleFile()
        {
            DeployModuleFile(1024 * 1024 * 2);
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

        #region hosts


        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToLibrary()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomModuleFile();
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToLibraryFolder()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomFolder(folder =>
                        {
                            folder.AddRandomModuleFile();
                        });
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToWeb()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomModuleFile();
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToContentType()
        {
            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddRandomContentType(contentType =>
                    {
                        contentType.AddRandomModuleFile();
                    });
                });

            TestModel(model);
        }

        #endregion
    }
}
