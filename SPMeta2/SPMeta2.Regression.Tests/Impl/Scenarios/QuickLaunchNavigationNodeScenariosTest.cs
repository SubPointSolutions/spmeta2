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
    public class QuickLaunchNavigationNodeScenariosTest : SPMeta2RegresionScenarioTestBase
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

        protected QuickLaunchNavigationNodeDefinition GenerateNode()
        {
            return GenerateNode(null);
        }

        protected QuickLaunchNavigationNodeDefinition GenerateNode(Action<QuickLaunchNavigationNodeDefinition> action)
        {
            var node = ModelGeneratorService.GetRandomDefinition<QuickLaunchNavigationNodeDefinition>();

            node.IsVisible = true;
            node.IsExternal = true;

            if (action != null) action(node);

            return node;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode")]
        public void CanDeploy_Simple_QuickLaunchNavigationNode()
        {
            var nav1Node = GenerateNode();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddQuickLaunchNavigationNode(nav1Node);
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode")]
        public void CanDeploy_Simple_QuickLaunchNavigationNode_As_AuthoredLinkPlain()
        {
            var nav1Node = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "AuthoredLinkPlain"
                });
            });

            var nav2Node = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "AuthoredLinkPlain"
                });
            });

            var nav3Node = GenerateNode(n =>
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
                    rndWeb.AddQuickLaunchNavigationNode(nav1Node);
                    rndWeb.AddQuickLaunchNavigationNode(nav2Node, n =>
                    {
                        n.AddQuickLaunchNavigationNode(nav3Node);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode")]
        public void CanDeploy_Simple_QuickLaunchNavigationNode_As_Heading()
        {
            var nav1Node = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
               {
                   Key = "NodeType",
                   Value = "Heading"
               });
            });

            var nav2Node = GenerateNode(n =>
            {
                n.Properties.Add(new NavigationNodePropertyValue
                {
                    Key = "NodeType",
                    Value = "Heading"
                });
            });

            var nav3Node = GenerateNode(n =>
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
                    rndWeb.AddQuickLaunchNavigationNode(nav1Node);
                    rndWeb.AddQuickLaunchNavigationNode(nav2Node, n =>
                    {
                        n.AddQuickLaunchNavigationNode(nav3Node);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode")]
        public void CanDeploy_TwoLevel_QuickLaunchNavigationNode()
        {
            var nav1Node = GenerateNode();
            var nav2Node = GenerateNode();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddQuickLaunchNavigationNode(nav1Node, node =>
                        {
                            node.AddQuickLaunchNavigationNode(nav2Node);
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode")]
        public void CanDeploy_ThreeLevel_QuickLaunchNavigationNode()
        {
            var nav1Node = GenerateNode();
            var nav2Node = GenerateNode();
            var nav3Node = GenerateNode();

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddQuickLaunchNavigationNode(nav1Node, node1 =>
                        {
                            node1.AddQuickLaunchNavigationNode(nav2Node, node2 =>
                            {
                                node2.AddQuickLaunchNavigationNode(nav3Node);
                            });
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode")]
        public void CanDeploy_Ordered_QuickLaunchNavigationNodes()
        {
            var wideIndex = 3;

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        for (int firstIndex = 0; firstIndex < wideIndex; firstIndex++)
                        {
                            rndWeb.AddQuickLaunchNavigationNode(GenerateNode(firstNode => firstNode.Title = string.Format("{0}_node", firstIndex)), node1 =>
                            {
                                for (int secondIndex = 0; secondIndex < wideIndex; secondIndex++)
                                {
                                    node1.AddQuickLaunchNavigationNode(GenerateNode(secondNode => secondNode.Title = string.Format("{0}_node", secondIndex)), node2 =>
                                    {
                                        for (int thirdIndex = 0; thirdIndex < wideIndex; thirdIndex++)
                                        {
                                            node2.AddQuickLaunchNavigationNode(GenerateNode(secondNode => secondNode.Title = string.Format("{0}_node", thirdIndex)));
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
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode.Tokens")]
        public void CanDeploy_QuickLaunchNavigationNode_WithSiteCollectionToken()
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
                        rndWeb.AddQuickLaunchNavigationNode(nav1Node);
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode.Tokens")]
        public void CanDeploy_QuickLaunchNavigationNode_WithSiteToken()
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
                        rndWeb.AddQuickLaunchNavigationNode(nav1Node);
                    });
                });

            TestModel(model);
        }

        #endregion

        #region localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode.Localization")]
        public void CanDeploy_Localized_QuickLaunchNavigationNode()
        {
            var definition = GetLocalizedDefinition();
            var subWebDefinition = GetLocalizedDefinition();

            var definitionSecondLevel = GetLocalizedDefinition();
            var subWebDefinitionSecondLevel = GetLocalizedDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddQuickLaunchNavigationNode(definition, def =>
                {
                    def.AddQuickLaunchNavigationNode(definitionSecondLevel);
                });

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddQuickLaunchNavigationNode(subWebDefinition, def =>
                    {
                        def.AddQuickLaunchNavigationNode(subWebDefinitionSecondLevel);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region special characters

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode.SpecialCharacters")]
        public void CanDeploy_QuickLaunchNavigationNode_With_Space()
        {
            var node1 = RndDef<QuickLaunchNavigationNodeDefinition>(def =>
            {
                def.Title = string.Format("1_{0}", Rnd.String());
                def.Url = string.Format("{0} {1}", Rnd.String(), Rnd.String());
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddQuickLaunchNavigationNode(node1);
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.QuickLaunchNavigationNode.SpecialCharacters")]
        public void CanDeploy_QuickLaunchNavigationNode_With_PercentTwenty()
        {
            var node1 = RndDef<QuickLaunchNavigationNodeDefinition>(def =>
            {
                def.Title = string.Format("1_{0}", Rnd.String());
                def.Url = string.Format("{0}%20{1}", Rnd.String(), Rnd.String());
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(rndWeb =>
                {
                    rndWeb.AddQuickLaunchNavigationNode(node1);
                });
            });

            TestModel(model);
        }

        #endregion

        #region utils

        protected QuickLaunchNavigationNodeDefinition GetLocalizedDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<QuickLaunchNavigationNodeDefinition>();
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
    }
}
