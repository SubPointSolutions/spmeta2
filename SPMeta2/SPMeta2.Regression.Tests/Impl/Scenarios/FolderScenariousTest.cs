using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FolderScenariousTest : SPMeta2RegresionScenarioTestBase
    {
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

        protected void AttachFolderHierarchy(ModelNode node)
        {
            node
                .AddRandomFolder(rootFolder =>
                {
                    rootFolder
                         .AddRandomFolder(subFolder =>
                         {
                             subFolder
                                 .AddRandomFolder()
                                 .AddRandomFolder();
                         })
                        .AddRandomFolder(subFolder =>
                        {
                            subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                        });
                })
                .AddRandomFolder(rootFolder =>
                {
                    rootFolder
                        .AddRandomFolder(subFolder =>
                        {
                            subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                        })
                       .AddRandomFolder(subFolder =>
                       {
                           subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                       });
                });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders")]
        public void CanDeploy_FolderHierarchy_InList()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, list => AttachFolderHierarchy(list));
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders")]
        public void CanDeploy_FolderHierarchy_InLibrary()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, list => AttachFolderHierarchy(list));
                    });
                });

            TestModel(model);
        }

        #endregion
    }
}
