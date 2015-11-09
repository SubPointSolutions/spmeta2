using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class MetadataNavigationSettingsScenarios : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.MetadataNavigationSettings")]
        public void CanDeploy_MetadataNavigationSettings_With_Hierary()
        {
            var managedNavSettings = ModelGeneratorService.GetRandomDefinition<MetadataNavigationSettingsDefinition>(
                def =>
                {
                    def.Hierarchies.Clear();
                    def.KeyFilters.Clear();

                    def.Hierarchies.Add(new MetadataNavigationHierarchy
                    {
                        FieldId = BuiltInFieldId.ContentTypeId
                    });
                });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.MetadataNavigationAndFiltering.Inherit(f =>
                {
                    f.Enable = true;
                }));

                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddMetadataNavigationSettings(managedNavSettings);
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MetadataNavigationSettings")]
        public void CanDeploy_MetadataNavigationSettings_With_KeyFilters()
        {
            var managedNavSettings = ModelGeneratorService.GetRandomDefinition<MetadataNavigationSettingsDefinition>(
                def =>
                {
                    def.Hierarchies.Clear();
                    def.KeyFilters.Clear();

                    def.KeyFilters.Add(new MetadataNavigationKeyFilter
                    {
                        FieldId = BuiltInFieldId.Created
                    });

                    def.KeyFilters.Add(new MetadataNavigationKeyFilter
                    {
                        FieldId = BuiltInFieldId.Modified
                    });
                });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.MetadataNavigationAndFiltering.Inherit(f =>
                {
                    f.Enable = true;
                }));

                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddMetadataNavigationSettings(managedNavSettings);
                });

            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.MetadataNavigationSettings")]
        public void CanDeploy_MetadataNavigationSettings_With_Hierarchies_And_KeyFilters()
        {
            var managedNavSettings = ModelGeneratorService.GetRandomDefinition<MetadataNavigationSettingsDefinition>(
                def =>
                {
                    def.Hierarchies.Clear();
                    def.KeyFilters.Clear();

                    def.Hierarchies.Add(new MetadataNavigationHierarchy
                    {
                        FieldId = BuiltInFieldId.ContentTypeId
                    });

                    def.KeyFilters.Add(new MetadataNavigationKeyFilter
                    {
                        FieldId = BuiltInFieldId.Created
                    });

                    def.KeyFilters.Add(new MetadataNavigationKeyFilter
                    {
                        FieldId = BuiltInFieldId.Modified
                    });
                });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.MetadataNavigationAndFiltering.Inherit(f =>
                {
                    f.Enable = true;
                }));

                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddMetadataNavigationSettings(managedNavSettings);
                });

            });

            TestModel(model);
        }

        #endregion
    }
}
