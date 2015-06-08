using System;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Impl.Tests.Scenarios
{
    [TestClass]
    public class TaxonomyFieldScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();

            RegressionService.ProvisionGenerationCount = 1;
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();


        }

        #endregion

        #region deleted taxonomy field

        [TestCategory("Regression.Scenarios.Taxonomy.TaxonomyField.DeletedField")]
        [TestMethod]
        public void CanDeploy_TaxonomyField_AfterDeleting()
        {
            var taxField = ModelGeneratorService.GetRandomDefinition<TaxonomyFieldDefinition>(def =>
            {
                def.Hidden = false;
                def.AllowDeletion = true;
            });

            var fieldModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTaxonomyField(taxField);
            });

            // deploying field 
            TestModel(fieldModel);

            // deleting field
            var deleteFieldModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField(f =>
                {
                    f.OnProvisioned<object>(context =>
                    {
                        DeleteSiteField(context, taxField.Id);
                    });
                });
            });

            TestModel(deleteFieldModel);

            // deplying again
            TestModel(fieldModel);
        }

        private void DeleteSiteField(OnCreatingContext<object, DefinitionBase> context, Guid fieldId)
        {
            if (context.ModelHost is SPMeta2.SSOM.ModelHosts.SiteModelHost)
            {
                var web = (context.ModelHost as SPMeta2.SSOM.ModelHosts.SiteModelHost).HostSite.RootWeb;
                var field = web.Fields[fieldId];

                field.AllowDeletion = true;
                field.ReadOnlyField = false;
                field.Sealed = false;
                field.Update(true);

                field.Delete();
                web.Update();
            }

            if (context.ModelHost is SPMeta2.CSOM.ModelHosts.SiteModelHost)
            {
                var web = (context.ModelHost as SPMeta2.CSOM.ModelHosts.SiteModelHost).HostSite.RootWeb;
                var field = web.Fields.GetById(fieldId);

                field.DeleteObject();
            }
        }


        #endregion
    }
}
