using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ReusableItemScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.ReusableItems")]
        public void CanDeploy_ReusableItemsToList()
        {
            var reusableHtml = ModelGeneratorService.GetRandomDefinition<ReusableHTMLItemDefinition>();
            var reusableText = ModelGeneratorService.GetRandomDefinition<ReusableTextItemDefinition>();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.ReusableContent, list =>
                {
                    list.AddReusableHTMLItem(reusableHtml);
                    list.AddReusableTextItem(reusableText);
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ReusableItems")]
        public void CanDeploy_ReusableItemsToFolder()
        {
            var reusableHtml = ModelGeneratorService.GetRandomDefinition<ReusableHTMLItemDefinition>();
            var reusableText = ModelGeneratorService.GetRandomDefinition<ReusableTextItemDefinition>();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.ReusableContent, list =>
                {
                    list.AddRandomFolder(folder =>
                    {
                        folder.AddReusableHTMLItem(reusableHtml);
                        folder.AddReusableTextItem(reusableText);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

    }
}
