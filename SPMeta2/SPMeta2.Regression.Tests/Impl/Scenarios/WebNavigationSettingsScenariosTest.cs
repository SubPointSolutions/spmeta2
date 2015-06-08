using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WebNavigationSettingsScenariosTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.WebNavigationSettings")]
        public void CanDeploy_WebNavigationSettings_As_PortalProvider()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));

            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                {
                    def.Enable = true;
                }));

                web.AddWebNavigationSettings(new WebNavigationSettingsDefinition()
                {
                    GlobalNavigationSource = BuiltInStandardNavigationSources.PortalProvider,
                    GlobalNavigationShowSubsites = true,
                    GlobalNavigationShowPages = true,

                    CurrentNavigationSource = BuiltInStandardNavigationSources.PortalProvider,
                    CurrentNavigationShowSubsites = true,
                    CurrentNavigationShowPages = true
                });
            });

            TestModel(siteModel, webModel);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.WebNavigationSettings")]
        public void CanDeploy_WebNavigationSettings_As_PortalProvider_OnSubWeb()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));

            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subweb =>
                {
                    subweb.AddWebFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                    {
                        def.Enable = true;
                    }));

                    subweb.AddWebNavigationSettings(new WebNavigationSettingsDefinition()
                    {
                        GlobalNavigationSource = BuiltInStandardNavigationSources.PortalProvider,
                        GlobalNavigationShowSubsites = true,
                        GlobalNavigationShowPages = true,

                        CurrentNavigationSource = BuiltInStandardNavigationSources.PortalProvider,
                        CurrentNavigationShowSubsites = true,
                        CurrentNavigationShowPages = true
                    });

                });

               
            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WebNavigationSettings")]
        public void CanDeploy_WebNavigationSettings_As_TaxonomyProvider()
        {
            var globalTerm = Rnd.String();
            var currentlTerm = Rnd.String();

            var currentNavigationGroup = new TaxonomyTermGroupDefinition
            {
                Name = string.Format("{0}", globalTerm)
            };

            var currentNavigationTermSet = new TaxonomyTermSetDefinition
            {
                Name = string.Format("{0}", globalTerm)
            };

            var globalNavigationGroup = new TaxonomyTermGroupDefinition
            {
                Name = string.Format("{0}", currentlTerm)
            };

            var globalNavigationTermSet = new TaxonomyTermSetDefinition
            {
                Name = string.Format("{0}", currentlTerm)
            };

            var taxWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));

                site.AddTaxonomyTermStore(new TaxonomyTermStoreDefinition
                {
                    UseDefaultSiteCollectionTermStore = true
                },
                store =>
                {
                    store.AddTaxonomyTermGroup(currentNavigationGroup, group =>
                    {
                        group.AddTaxonomyTermSet(currentNavigationTermSet, termSet =>
                        {
                            termSet.AddRandomTerm();
                            termSet.AddRandomTerm();
                            termSet.AddRandomTerm();
                        });
                    });

                    store.AddTaxonomyTermGroup(globalNavigationGroup, group =>
                    {
                        group.AddTaxonomyTermSet(globalNavigationTermSet, termSet =>
                        {
                            termSet.AddRandomTerm();
                            termSet.AddRandomTerm();
                            termSet.AddRandomTerm();
                        });
                    });
                });
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(taxWeb, subWeb =>
                {
                    subWeb.AddSiteFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                    {
                        def.Enable = true;
                    }));

                    subWeb.AddWebNavigationSettings(new WebNavigationSettingsDefinition()
                    {
                        CurrentNavigationSource = BuiltInStandardNavigationSources.TaxonomyProvider,

                        CurrentNavigationUseDefaultSiteCollectionTermStore = true,
                        CurrentNavigationTermSetName = currentNavigationGroup.Name,

                        GlobalNavigationSource = BuiltInStandardNavigationSources.TaxonomyProvider,

                        GlobalNavigationUseDefaultSiteCollectionTermStore = true,
                        GlobalNavigationTermSetName = globalNavigationGroup.Name,

                        DisplayShowHideRibbonAction = true
                    });
                });
            });

            TestModel(siteModel, webModel);
        }

        #endregion
    }
}
