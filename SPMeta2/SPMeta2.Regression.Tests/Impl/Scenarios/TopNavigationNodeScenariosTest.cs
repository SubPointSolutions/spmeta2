using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TopNavigationNodeScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region top nav cases

        protected TopNavigationNodeDefinition GenerateNode()
        {
            return GenerateNode(null);
        }

        protected TopNavigationNodeDefinition GenerateNode(Action<TopNavigationNodeDefinition> action)
        {
            var node = ModelGeneratorService.GetRandomDefinition<TopNavigationNodeDefinition>();

            node.IsVisible = true;
            node.IsExternal = true;

            if (action != null) action(node);

            return node;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode")]
        public void CanDeploy_Simple_TopNavigationNode()
        {
            var nav1Node = GenerateNode();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddTopNavigationNode(nav1Node);
                    });
                });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode")]
        public void CanDeploy_TwoLevel_TopNavigationNode()
        {
            var nav1Node = GenerateNode();
            var nav2Node = GenerateNode();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddTopNavigationNode(nav1Node, node =>
                        {
                            node.AddTopNavigationNode(nav2Node);
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode")]
        public void CanDeploy_ThreeLevel_TopNavigationNode()
        {
            var nav1Node = GenerateNode();
            var nav2Node = GenerateNode();
            var nav3Node = GenerateNode();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddTopNavigationNode(nav1Node, node1 =>
                        {
                            node1.AddTopNavigationNode(nav2Node, node2 =>
                            {
                                node2.AddTopNavigationNode(nav3Node);
                            });
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode")]
        public void CanDeploy_Ordered_TopNavigationNodes()
        {
            var wideIndex = 3;

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        for (int firstIndex = 0; firstIndex < wideIndex; firstIndex++)
                        {
                            rndWeb.AddTopNavigationNode(GenerateNode(firstNode => firstNode.Title = string.Format("{0}_node", firstIndex)), node1 =>
                            {
                                for (int secondIndex = 0; secondIndex < wideIndex; secondIndex++)
                                {
                                    node1.AddTopNavigationNode(GenerateNode(secondNode => secondNode.Title = string.Format("{0}_node", secondIndex)), node2 =>
                                    {
                                        for (int thirdIndex = 0; thirdIndex < wideIndex; thirdIndex++)
                                        {
                                            node2.AddTopNavigationNode(GenerateNode(secondNode => secondNode.Title = string.Format("{0}_node", thirdIndex)));
                                        }
                                    });
                                }
                            });
                        }
                    });
                });

            TestModel(model);
        }

        #endregion

    }
}
