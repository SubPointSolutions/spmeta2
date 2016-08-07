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
using SPMeta2.Containers.Services;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Definitions.Base;

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

        #region tokens

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode.Tokens")]
        public void CanDeploy_TopNavigationNode_WithSiteCollectionToken()
        {
            var nav1Node = GenerateNode(n =>
            {
                n.Url = string.Format("~sitecollection/{0}.html", Rnd.String());
            });

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
        [TestCategory("Regression.Scenarios.TopNavigationNode.Tokens")]
        public void CanDeploy_TopNavigationNode_WithSiteToken()
        {
            var nav1Node = GenerateNode(n =>
            {
                n.Url = string.Format("~site/{0}.html", Rnd.String());
            });

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

        #endregion

        #region localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode.Localization")]
        public void CanDeploy_Localized_TopNavigationNode()
        {
            var definition = GetLocalizedDefinition();
            var subWebDefinition = GetLocalizedDefinition();

            var definitionSecondLevel = GetLocalizedDefinition();
            var subWebDefinitionSecondLevel = GetLocalizedDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddTopNavigationNode(definition, def =>
                {
                    def.AddTopNavigationNode(definitionSecondLevel);
                });

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddTopNavigationNode(subWebDefinition, def =>
                    {
                        def.AddTopNavigationNode(subWebDefinitionSecondLevel);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region utils

        protected TopNavigationNodeDefinition GetLocalizedDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<TopNavigationNodeDefinition>();
            var localeIds = Rnd.LocaleIds();

            foreach (var localeId in localeIds)
            {
                definition.TitleResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedTitle_{0}", localeId)
                });
            }

            return definition;
        }

        #endregion

        #region navigation headers

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode")]
        public void CanDeploy_Simple_TopNavigation_As_Heading()
        {
            var nav1Header = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "Heading"
                });
            });

            var nav2Header = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "Heading"
                });
            });

            var nav3Header = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "Heading"
                });
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddTopNavigationNode(nav1Header);
                    rndWeb.AddTopNavigationNode(nav2Header, n =>
                    {
                        n.AddTopNavigationNode(nav3Header);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode")]
        public void CanDeploy_Simple_TopNavigation_As_AuthoredLinkPlain()
        {
            var nav1Header = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "AuthoredLinkPlain"
                });
            });

            var nav2Header = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "AuthoredLinkPlain"
                });
            });

            var nav3Header = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "AuthoredLinkPlain"
                });
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddTopNavigationNode(nav1Header);
                    rndWeb.AddTopNavigationNode(nav2Header, n =>
                    {
                        n.AddTopNavigationNode(nav3Header);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region special characters

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode.SpecialCharacters")]
        public void CanDeploy_TopNavigationNode_With_Space()
        {
            var node1 = RndDef<TopNavigationNodeDefinition>(def =>
            {
                def.Title = string.Format("1_{0}", Rnd.String());
                def.Url = string.Format("{0} {1}", Rnd.String(), Rnd.String());
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddTopNavigationNode(node1);
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TopNavigationNode.SpecialCharacters")]
        public void CanDeploy_TopNavigationNode_With_PercentTwenty()
        {
            var node1 = RndDef<TopNavigationNodeDefinition>(def =>
            {
                def.Title = string.Format("1_{0}", Rnd.String());
                def.Url = string.Format("{0}%20{1}", Rnd.String(), Rnd.String());
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddTopNavigationNode(node1);
                });
            });

            TestModel(model);
        }

        #endregion
    }
}
