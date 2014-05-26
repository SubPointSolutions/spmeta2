using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;

namespace SPMeta2.CSOM.Tests
{
    [TestClass]
    public class ModuleFileModelTests : ClientOMSharePointTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            // it is a good place to change TestSetting
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployResourceFilesWithFieldValues()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
               .WithLists(lists =>
               {
                   lists
                       .AddList(ListModels.StyleLibrary, list =>
                       {
                           list
                                .AddModuleFile(ModuleFiles.JQuery, file =>
                                {
                                    file
                                       .AddListItemFieldValue(new ListItemFieldValueDefinition
                                       {
                                           FieldName = "Title",
                                           Value = "Test title - " + Environment.TickCount
                                       });
                                });
                       });
               });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployResourceFiles()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
               .WithLists(lists =>
               {
                   lists
                       .AddList(ListModels.StyleLibrary, list =>
                       {
                           list
                                .AddModuleFile(ModuleFiles.JQuery);
                       });
               });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployResourceFilesInFolder()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
               .WithLists(lists =>
               {
                   lists
                       .AddList(ListModels.StyleLibrary, list =>
                       {
                           list
                               .AddFolder(new FolderDefinition
                               {
                                   Name = "test folder"
                               }, whsFolder =>
                               {
                                   whsFolder
                                         .AddModuleFile(ModuleFiles.JQuery);
                               });


                       });
               });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        #endregion
    }
}
