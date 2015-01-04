using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WebConfigModificationScenariosTest : SPMeta2RegresionScenarioTestBase
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

        [TestMethod]
        [TestCategory("Regression.Scenarios.WebConfigModification")]
        public void CanDeploy_WebConfigModification_As_EnsureChildNode()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var childNodeConfig = ModelGeneratorService.GetRandomDefinition<WebConfigModificationDefinition>(def =>
                {
                    var key = string.Format("key{0}", Rnd.String(8));
                    var value = string.Format("value{0}", Rnd.String(8));

                    def.Path = string.Format("configuration/appSettings");
                    def.Name = string.Format("add[@key='{0}']", key);
                    def.Sequence = Rnd.UInt(100);
                    def.Owner = string.Format("WebConfigModifications{0}", Rnd.String(8));
                    def.Type = BuiltInWebConfigModificationType.EnsureChildNode;
                    def.Value = string.Format(@"<add key=""{0}"" value=""{1}"" />", key, value);
                });

                var model = SPMeta2Model
                    .NewWebApplicationModel(webApp =>
                    {
                        webApp.AddWebConfigModification(childNodeConfig);
                    });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WebConfigModification")]
        public void CanDeploy_WebConfigModification_As_EnsureAttribute()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var attributeNodeConfig =
                    ModelGeneratorService.GetRandomDefinition<WebConfigModificationDefinition>(def =>
                    {
                        var key = string.Format("key{0}", Rnd.String(8));
                        var value = string.Format("value{0}", Rnd.String(8));

                        def.Path = string.Format("configuration/appSettings");
                        def.Name = string.Format("{0}", key);
                        def.Sequence = Rnd.UInt(100);
                        def.Owner = string.Format("WebConfigModifications{0}", Rnd.String(8));
                        def.Type = BuiltInWebConfigModificationType.EnsureAttribute;
                        def.Value = string.Format(@"{0}", value);
                    });

                var model = SPMeta2Model
                    .NewWebApplicationModel(webApp =>
                    {
                        webApp.AddWebConfigModification(attributeNodeConfig);
                    });

                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.WebConfigModification")]
        public void CanDeploy_WebConfigModification_As_EnsureSection()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var ensureSectionNodeConfig =
                    ModelGeneratorService.GetRandomDefinition<WebConfigModificationDefinition>(def =>
                    {
                        def.Path = string.Format("configuration/appSettings");
                        def.Name = string.Format("testSection{0}", Rnd.String(8));
                        def.Sequence = Rnd.UInt(100);
                        def.Owner = string.Format("WebConfigModifications{0}", Rnd.String(8));
                        def.Type = BuiltInWebConfigModificationType.EnsureSection;
                    });

                var model = SPMeta2Model
                    .NewWebApplicationModel(webApp =>
                    {
                        webApp.AddWebConfigModification(ensureSectionNodeConfig);
                    });

                TestModel(model);
            });
        }


        #endregion
    }
}
