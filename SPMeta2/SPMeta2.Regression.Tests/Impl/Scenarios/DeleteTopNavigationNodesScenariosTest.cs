using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class DeleteTopNavigationNodesScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region test

        [TestMethod]
        [TestCategory("Regression.Scenarios.DeleteTopNavigationNodes")]
        public void CanDeploy_DeleteTopNavigationNodes_ByTitle()
        {
            // deploy some links, and then delete them by title

            var node1 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "1_" + def.Title);
            var node2 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "2_" + def.Title);
            var node3 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "3_" + def.Title);

            var subWeb = RndDef<WebDefinition>();

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddTopNavigationNode(node1);
                    web.AddTopNavigationNode(node2);
                    web.AddTopNavigationNode(node3);
                });
            });

            TestModel(model);

            var deleteNodesModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddDeleteTopNavigationNodes(new DeleteTopNavigationNodesDefinition
                    {
                        NavigationNodes = new List<NavigationNodeMatch>
                        {
                            new NavigationNodeMatch { Title =  node1.Title },
                            new NavigationNodeMatch { Title =  node3.Title },
                        }
                    });
                });
            });


            TestModel(deleteNodesModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.DeleteTopNavigationNodes")]
        public void CanDeploy_DeleteTopNavigationNodes_ByUrl()
        {
            // deploy some links, and then delete them by title

            var node1 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "1_" + def.Title);
            var node2 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "2_" + def.Title);
            var node3 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "3_" + def.Title);

            var subWeb = RndDef<WebDefinition>();

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddTopNavigationNode(node1);
                    web.AddTopNavigationNode(node2);
                    web.AddTopNavigationNode(node3);
                });
            });

            TestModel(model);

            var deleteNodesModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddDeleteTopNavigationNodes(new DeleteTopNavigationNodesDefinition
                    {
                        NavigationNodes = new List<NavigationNodeMatch>
                        {
                            new NavigationNodeMatch { Url =  node1.Url },
                            new NavigationNodeMatch { Url =  node3.Url },
                        }
                    });
                });
            });


            TestModel(deleteNodesModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.DeleteTopNavigationNodes")]
        public void CanDeploy_DeleteTopNavigationNodes_ByTitleAndUrl()
        {
            // deploy some links, and then delete them by bot title  / url

            // deploy some links, and then delete them by title

            var node1 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "1_" + def.Title);
            var node2 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "2_" + def.Title);
            var node3 = RndDef<TopNavigationNodeDefinition>(def => def.Title = "3_" + def.Title);

            var subWeb = RndDef<WebDefinition>();

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddTopNavigationNode(node1);
                    web.AddTopNavigationNode(node2);
                    web.AddTopNavigationNode(node3);
                });
            });

            TestModel(model);

            var deleteNodesModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddDeleteTopNavigationNodes(new DeleteTopNavigationNodesDefinition
                    {
                        NavigationNodes = new List<NavigationNodeMatch>
                        {
                            new NavigationNodeMatch { Title =  node1.Title },
                            new NavigationNodeMatch { Url =  node3.Url },
                        }
                    });
                });
            });


            TestModel(deleteNodesModel);
        }

        #endregion

        #region special characters

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNodes.SpecialCharacters")]
        public void CanDeploy_DeleteTopNavigationNodes_WithSpaces()
        {
            var node1 = RndDef<TopNavigationNodeDefinition>(def =>
            {
                def.Title = string.Format("1_{0}", Rnd.String());
                def.Url = string.Format("{0} {1}", Rnd.String(), Rnd.String());
            });

            var node2 = RndDef<TopNavigationNodeDefinition>(def =>
            {
                def.Title = string.Format("2_{0}", Rnd.String());
                def.Url = string.Format("{0}%20{1}", Rnd.String(), Rnd.String());
            });

            var subWeb = RndDef<WebDefinition>();

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddTopNavigationNode(node1);
                    web.AddTopNavigationNode(node2);
                });
            });

            TestModel(model);

            var deleteNodesModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddDeleteTopNavigationNodes(new DeleteTopNavigationNodesDefinition
                    {
                        NavigationNodes = new List<NavigationNodeMatch>
                        {
                            new NavigationNodeMatch { Url =  node1.Url },
                            new NavigationNodeMatch { Url =  node2.Url },
                        }
                    });
                });
            });

            TestModel(deleteNodesModel);
        }

        #endregion
    }
}
