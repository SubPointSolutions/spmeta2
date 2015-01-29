using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Fields
{
    [TestClass]
    public class LookupFieldScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitialize]
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
        [TestCategory("Regression.Scenarios.Fields.LookupField")]
        public void CanDeploy_LookupField_AsSingleSelect()
        {
            var field = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.AllowMultipleValues = false;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField")]
        public void CanDeploy_LookupField_AsMultiSelectSelect()
        {
            var field = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.AllowMultipleValues = true;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

        #endregion
    }
}
