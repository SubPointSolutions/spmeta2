using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    [TestClass]
    public class ListModelTests : ClientOMSharePointTestBase
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
        public void CanDeployListFolders()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                                       .WithLists(lists =>
                                       {
                                           lists
                                               .AddList(ListModels.TestList, list =>
                                               {
                                                   list
                                                       .AddFolder(AppFolders.Year2012, yearFolder =>
                                                       {
                                                           yearFolder
                                                               .AddFolder(AppFolders.January)
                                                               .AddFolder(AppFolders.Febuary)
                                                               .AddFolder(AppFolders.March);
                                                       })
                                                      .AddFolder(AppFolders.Year2013, yearFolder =>
                                                      {
                                                          yearFolder
                                                             .AddFolder(AppFolders.January)
                                                             .AddFolder(AppFolders.Febuary)
                                                             .AddFolder(AppFolders.March);
                                                      })
                                                      .AddFolder(AppFolders.Year2014, yearFolder =>
                                                      {
                                                          yearFolder
                                                             .AddFolder(AppFolders.January)
                                                             .AddFolder(AppFolders.Febuary)
                                                             .AddFolder(AppFolders.March);
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
        public void CanDeployLibraryFolders()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                                       .WithLists(lists =>
                                       {
                                           lists
                                               .AddList(ListModels.TestLibrary, list =>
                                               {
                                                   list
                                                       .AddFolder(AppFolders.Year2012, yearFolder =>
                                                       {
                                                           yearFolder
                                                               .AddFolder(AppFolders.January)
                                                               .AddFolder(AppFolders.Febuary)
                                                               .AddFolder(AppFolders.March);
                                                       })
                                                      .AddFolder(AppFolders.Year2013, yearFolder =>
                                                      {
                                                          yearFolder
                                                             .AddFolder(AppFolders.January)
                                                             .AddFolder(AppFolders.Febuary)
                                                             .AddFolder(AppFolders.March);
                                                      })
                                                      .AddFolder(AppFolders.Year2014, yearFolder =>
                                                      {
                                                          yearFolder
                                                             .AddFolder(AppFolders.January)
                                                             .AddFolder(AppFolders.Febuary)
                                                             .AddFolder(AppFolders.March);
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
        public void CanDeployListModels()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                                       .WithLists(lists =>
                                       {
                                           lists
                                               .AddList(ListModels.TestLibrary)
                                               .AddList(ListModels.TestList)
                                               .AddList(ListModels.TestLinksList);
                                       });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployListViewModels()
        {
            var model = SPMeta2Model
                            .NewModel()
                            .DummyWeb()
                            .WithLists(lists =>
                            {
                                lists
                                    .AddList(ListModels.TestLibrary, list =>
                                    {
                                        list
                                            .AddView(ListViewModels.LastDocs.LastDocuments, view =>
                                            {
                                                //view
                                                //    .On
                                            });
                                    })
                                    .AddList(ListModels.TestList)
                                    .AddList(ListModels.TestLinksList);
                            });


            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        #endregion
    }
}
