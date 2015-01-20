using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class TaxonomyFieldScenariousTest : SPMeta2RegresionScenarioTestBase
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

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldAsSingleSelect()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsMulti = false;
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModel(model);
        }


        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField")]
        [TestMethod]
        public void CanDeploy_TaxonomyFieldAsMiltiSelect()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.IsMulti = true;
            });

            var model = SPMeta2Model
                  .NewSiteModel(site =>
                  {
                      site.AddTaxonomyField(taxField);
                  });

            TestModel(model);
        }

        #endregion
    }
}
