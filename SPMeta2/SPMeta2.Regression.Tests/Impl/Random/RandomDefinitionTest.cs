using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Utils;
using SPMeta2.Attributes.Regression;

using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Enumerations;
using SPMeta2.Definitions.Fields;
using System.IO;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Validation.Services;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.Tests.Impl.Random
{
    [TestClass]
    public class RandomDefinitionTest : SPMeta2RegresionTestBase
    {
        public RandomDefinitionTest()
        {
            EnablePropertyUpdateValidation = true;
            PropertyUpdateGenerationCount = 2;

            EnablePropertyNullableValidation = true;
            PropertyNullableGenerationCount = 1;

            RegressionService.ShowOnlyFalseResults = true;
        }

        #region common

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

        [TestMethod]
        [TestCategory("Regression.Rnd")]
        public void SelfDiagnostic_TestShouldHaveAllDefinitions()
        {
            var methods = GetType().GetMethods();

            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            var allDefinitions = ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
            {
                spMetaAssembly,
                spMetaStandardAssembly
            });

            foreach (var def in allDefinitions)
            {
                Trace.WriteLine(def.Name);
            }

            foreach (var definition in allDefinitions)
            {
                var hasTestMethod = RegressionService.HasTestMethod("CanDeployRandom_", definition, methods);

                Assert.IsTrue(hasTestMethod);
            }
        }

        #endregion

        #region farm scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Farm")]
        public void CanDeployRandom_DocumentParserDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<DocumentParserDefinition>();
            });
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Farm")]
        public void CanDeployRandom_FarmDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<FarmDefinition>();
            });
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Farm")]
        public void CanDeployRandom_DiagnosticsServiceBaseDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<DiagnosticsServiceBaseDefinition>();
            });
        }



        [TestMethod]
        [TestCategory("Regression.Rnd.Farm")]
        public void CanDeployRandom_ManagedAccountDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<ManagedAccountDefinition>();
            });
        }

        #endregion

        #region secure store application

        [TestMethod]
        [TestCategory("Regression.Rnd.SecureStore")]
        public void CanDeployRandom_SecureStoreApplicationDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<SecureStoreApplicationDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.SecureStore")]
        public void CanDeployRandom_TargetApplicationDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<TargetApplicationDefinition>();
            });
        }

        #endregion

        #region web app scope

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_AlternateUrlDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<AlternateUrlDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_WebConfigModificationDefinition()
        {
            //var _oldProvisionGenerationCount = 0;

            //try
            //{
            //    _oldProvisionGenerationCount = RegressionService.ProvisionGenerationCount;
            //    RegressionService.ProvisionGenerationCount = 1;

            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<WebConfigModificationDefinition>();
            });
            //}
            //finally
            //{
            //    RegressionService.ProvisionGenerationCount = _oldProvisionGenerationCount;
            //}
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_ContentDatabaseDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<ContentDatabaseDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_WebApplicationDefinition()
        {
            if (TestOptions.EnableWebApplicationDefinitionTest)
            {
                WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
                {
                    TestRandomDefinition<WebApplicationDefinition>();
                });
            }
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_JobDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<JobDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_PrefixDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<PrefixDefinition>();
            });
        }

        #endregion

        #region site scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.CustomDocumentIdProvider")]
        public void CanDeployRandom_CustomDocumentIdProviderDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<CustomDocumentIdProviderDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.Audit")]
        public void CanDeployRandom_AuditSettingsDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<AuditSettingsDefinition>();
            });
        }

        #region IRM

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.IRM")]
        public void CanDeployRandom_InformationRightsManagementSettingsDefinition()
        {
            TestRandomDefinition<InformationRightsManagementSettingsDefinition>();
        }

        #endregion

        #region search

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.Search")]
        public void CanDeployRandom_SearchConfigurationDefinition()
        {
            TestRandomDefinition<SearchConfigurationDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.Search")]
        public void CanDeployRandom_SearchResultDefinition()
        {
            TestRandomDefinition<SearchResultDefinition>();
        }

        #endregion

        #region fields

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_MultiChoiceFieldDefinition()
        {
            TestRandomDefinition<MultiChoiceFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_ChoiceFieldDefinition()
        {
            TestRandomDefinition<ChoiceFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_CurrencyFieldDefinition()
        {
            TestRandomDefinition<CurrencyFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_DateTimeFieldDefinition()
        {
            TestRandomDefinition<DateTimeFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_BooleanFieldDefinition()
        {
            TestRandomDefinition<BooleanFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_UserFieldDefinition()
        {
            TestRandomDefinition<UserFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_LookupFieldDefinition()
        {
            TestRandomDefinition<LookupFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_DependentLookupFieldDefinition()
        {
            TestRandomDefinition<DependentLookupFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_NoteFieldDefinition()
        {
            TestRandomDefinition<NoteFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_TextFieldDefinition()
        {
            TestRandomDefinition<TextFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_CalculatedFieldDefinition()
        {
            TestRandomDefinition<CalculatedFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_ComputedFieldDefinition()
        {
            TestRandomDefinition<ComputedFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_NumberFieldDefinition()
        {
            TestRandomDefinition<NumberFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_GuidFieldDefinition()
        {
            TestRandomDefinition<GuidFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_URLFieldDefinition()
        {
            TestRandomDefinition<URLFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields.Last")]
        public void CanDeployRandom_GeolocationFieldDefinition()
        {
            TestRandomDefinition<GeolocationFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields.Last")]
        public void CanDeployRandom_MediaFieldDefinition()
        {
            TestRandomDefinition<MediaFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields.Last")]
        public void CanDeployRandom_SummaryLinkFieldDefinition()
        {
            TestRandomDefinition<SummaryLinkFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields.Last")]
        public void CanDeployRandom_OutcomeChoiceFieldDefinition()
        {
            TestRandomDefinition<OutcomeChoiceFieldDefinition>();
        }

        #endregion

        #region publishing fields

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.PublishingFields.TypedFields")]
        public void CanDeployRandom_HTMLFieldDefinition()
        {
            TestRandomDefinition<HTMLFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.PublishingFields.TypedFields")]
        public void CanDeployRandom_ImageFieldDefinition()
        {
            TestRandomDefinition<ImageFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.PublishingFields.TypedFields")]
        public void CanDeployRandom_LinkFieldDefinition()
        {
            TestRandomDefinition<LinkFieldDefinition>();
        }

        #endregion



        [TestMethod]
        [TestCategory("Regression.Rnd.EventReceivers")]
        public void CanDeployRandom_EventReceiverDefinition()
        {
            TestRandomDefinition<EventReceiverDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_SiteDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<SiteDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_UserCustomActionDefinition()
        {
            TestRandomDefinition<UserCustomActionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.Fields")]
        public void CanDeployRandom_FieldDefinition()
        {
            TestRandomDefinition<FieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_ImageRenditionDefinition()
        {
            TestRandomDefinition<ImageRenditionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_BusinessDataFieldDefinition()
        {
            TestRandomDefinition<BusinessDataFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.TypedFields")]
        public void CanDeployRandom_TaxonomyFieldDefinition()
        {
            TestRandomDefinition<TaxonomyFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_SandboxSolutionDefinition()
        {
            TestRandomDefinition<SandboxSolutionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_FarmSolutionDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<FarmSolutionDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_AudienceDefinition()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<AudienceDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_ContentTypeDefinition()
        {
            TestRandomDefinition<ContentTypeDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.ContentTypes")]
        public void CanDeployRandom_HideContentTypeFieldLinksDefinition()
        {
            TestRandomDefinition<HideContentTypeFieldLinksDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.ContentTypes")]
        public void CanDeployRandom_RemoveContentTypeFieldLinksDefinition()
        {
            TestRandomDefinition<RemoveContentTypeFieldLinksDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site.ContentTypes")]
        public void CanDeployRandom_UniqueContentTypeFieldsOrderDefinition()
        {
            TestRandomDefinition<UniqueContentTypeFieldsOrderDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_ContentTypeFieldLinkDefinition()
        {
            TestRandomDefinition<ContentTypeFieldLinkDefinition>();
        }

        #endregion

        #region web scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_SearchSettingsDefinition()
        {
            TestRandomDefinition<SearchSettingsDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_TreeViewSettingsDefinition()
        {
            TestRandomDefinition<TreeViewSettingsDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_RegionalSettingsDefinition()
        {
            TestRandomDefinition<RegionalSettingsDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_WebDefinition()
        {
            TestRandomDefinition<WebDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_RootWebDefinition()
        {
            TestRandomDefinition<RootWebDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_QuickLaunchNavigationNodeDefinition()
        {
            TestRandomDefinition<QuickLaunchNavigationNodeDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.SP2013Workflow")]
        public void CanDeployRandom_SP2013WorkflowDefinition()
        {
            TestRandomDefinition<SP2013WorkflowDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_TopNavigationNodeDefinition()
        {
            TestRandomDefinition<TopNavigationNodeDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_PropertyDefinition()
        {
            TestRandomDefinition<PropertyDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_FeatureDefinition()
        {
            TestRandomDefinition<FeatureDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_ListDefinition()
        {
            TestRandomDefinition<ListDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_AppDefinition()
        {
            TestRandomDefinition<AppDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_PageLayoutAndSiteTemplateSettingsDefinition()
        {
            TestRandomDefinition<PageLayoutAndSiteTemplateSettingsDefinition>();
        }

        #endregion

        #region list scope

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ListItemFieldValueDefinition()
        {
            TestRandomDefinition<ListItemFieldValueDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ListItemFieldValuesDefinition()
        {
            TestRandomDefinition<ListItemFieldValuesDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_BreakRoleInheritanceDefinition()
        {
            TestRandomDefinition<BreakRoleInheritanceDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ResetRoleInheritanceDefinition()
        {
            TestRandomDefinition<ResetRoleInheritanceDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ContentTypeLinkDefinition()
        {
            TestRandomDefinition<ContentTypeLinkDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_FolderDefinition()
        {
            TestRandomDefinition<FolderDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ListFieldLinkDefinition()
        {
            TestRandomDefinition<ListFieldLinkDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ModuleFileDefinition()
        {
            TestRandomDefinition<ModuleFileDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.SP2013Workflow")]
        public void CanDeployRandom_SP2013WorkflowSubscriptionDefinition()
        {
            TestRandomDefinition<SP2013WorkflowSubscriptionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ListItemDefinition()
        {
            TestRandomDefinition<ListItemDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List")]
        public void CanDeployRandom_ListViewDefinition()
        {
            TestRandomDefinition<ListViewDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List.ContentTypes")]
        public void CanDeployRandom_UniqueContentTypeOrderDefinition()
        {
            TestRandomDefinition<UniqueContentTypeOrderDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List.ContentTypes")]
        public void CanDeployRandom_HideContentTypeLinksDefinition()
        {
            TestRandomDefinition<HideContentTypeLinksDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.List.ContentTypes")]
        public void CanDeployRandom_RemoveContentTypeLinksDefinition()
        {
            TestRandomDefinition<RemoveContentTypeLinksDefinition>();
        }

        #endregion

        #region list settings



        #endregion


        #region wb part gallery

        [TestMethod]
        [TestCategory("Regression.Rnd.WebPartGallery")]
        public void CanDeployRandom_WebPartGalleryFileDefinition()
        {
            TestRandomDefinition<WebPartGalleryFileDefinition>();
        }

        #endregion

        #region master page gallery

        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_ItemDisplayTemplateDefinition()
        {
            TestRandomDefinition<ItemDisplayTemplateDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_FilterDisplayTemplateDefinition()
        {
            TestRandomDefinition<FilterDisplayTemplateDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_ControlDisplayTemplateDefinition()
        {
            TestRandomDefinition<ControlDisplayTemplateDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_JavaScriptDisplayTemplateDefinition()
        {
            TestRandomDefinition<JavaScriptDisplayTemplateDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_MasterPageDefinition()
        {
            TestRandomDefinition<MasterPageDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_MasterPagePreviewDefinition()
        {
            TestRandomDefinition<MasterPagePreviewDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.MasterPageGallery")]
        public void CanDeployRandom_PublishingPageLayoutDefinition()
        {
            TestRandomDefinition<PublishingPageLayoutDefinition>();
        }

        #endregion

        #region pages scope



        [TestMethod]
        [TestCategory("Regression.Rnd.Pages")]
        public void CanDeployRandom_PublishingPageDefinition()
        {
            TestRandomDefinition<PublishingPageDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Pages")]
        public void CanDeployRandom_DeleteWebPartsDefinition()
        {
            TestRandomDefinition<DeleteWebPartsDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Pages")]
        public void CanDeployRandom_WebPartDefinition()
        {
            TestRandomDefinition<WebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Pages")]
        public void CanDeployRandom_WebPartPageDefinition()
        {
            TestRandomDefinition<WebPartPageDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Pages")]
        public void CanDeployRandom_WikiPageDefinition()
        {
            TestRandomDefinition<WikiPageDefinition>();
        }

        #endregion

        #region security scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Security")]
        public void CanDeployRandom_SecurityGroupDefinition()
        {
            TestRandomDefinition<SecurityGroupDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Security")]
        public void CanDeployRandom_SecurityGroupLinkDefinition()
        {
            TestRandomDefinition<SecurityGroupLinkDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Security")]
        public void CanDeployRandom_SecurityRoleDefinition()
        {
            TestRandomDefinition<SecurityRoleDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Security")]
        public void CanDeployRandom_SecurityRoleLinkDefinition()
        {
            TestRandomDefinition<SecurityRoleLinkDefinition>();
        }

        #endregion

        #region apps

        [TestMethod]
        [TestCategory("Regression.Rnd.Apps")]
        public void CanDeployRandom_AppPrincipalDefinition()
        {
            WithSPMeta2NotSupportedExceptions(() =>
            {
                TestRandomDefinition<AppPrincipalDefinition>();
            });

        }

        #endregion

        #region search

        [TestMethod]
        [TestCategory("Regression.Rnd.Search")]
        public void CanDeployRandom_ManagedPropertyDefinition()
        {
            TestRandomDefinition<ManagedPropertyDefinition>();
        }

        #endregion

        #region webparts

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_SummaryLinkWebPartDefinition()
        {
            TestRandomDefinition<SummaryLinkWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ContentBySearchWebPartDefinition()
        {
            TestRandomDefinition<ContentBySearchWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_RefinementScriptWebPartDefinition()
        {
            TestRandomDefinition<RefinementScriptWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ContentByQueryWebPartDefinition()
        {
            TestRandomDefinition<ContentByQueryWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ResultScriptWebPartDefinition()
        {
            TestRandomDefinition<ResultScriptWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_SiteFeedWebPartDefinition()
        {
            TestRandomDefinition<SiteFeedWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ClientWebPartDefinition()
        {
            TestRandomDefinition<ClientWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ScriptEditorWebPartDefinition()
        {
            TestRandomDefinition<ScriptEditorWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ContentEditorWebPartDefinition()
        {
            TestRandomDefinition<ContentEditorWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_UserCodeWebPartDefinition()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                WithExcpectedExceptions(new[]{
                    typeof(SPMeta2NotSupportedException)
                }, () =>
                {
                    TestRandomDefinition<UserCodeWebPartDefinition>();
                });


            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ContactFieldControlDefinition()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                TestRandomDefinition<ContactFieldControlDefinition>();
            });
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_SilverlightWebPartDefinition()
        {
            TestRandomDefinition<SilverlightWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ListViewWebPartDefinition()
        {
            TestRandomDefinition<ListViewWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_XsltListViewWebPartDefinition()
        {
            TestRandomDefinition<XsltListViewWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_PageViewerWebPartDefinition()
        {
            TestRandomDefinition<PageViewerWebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ProjectSummaryWebPartDefinition()
        {
            TestRandomDefinition<ProjectSummaryWebPartDefinition>();
        }

        #endregion

        #region welcome page

        [TestMethod]
        [TestCategory("Regression.Rnd.WelcomePage")]
        public void CanDeployRandom_WelcomePageDefinition()
        {
            TestRandomDefinition<WelcomePageDefinition>();
        }

        #endregion

        #region taxonomy

        [TestMethod]
        [TestCategory("Regression.Rnd.Taxonomy")]
        public void CanDeployRandom_TaxonomyTermLabelDefinition()
        {
            TestRandomDefinition<TaxonomyTermLabelDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Taxonomy")]
        public void CanDeployRandom_TaxonomyTermGroupDefinition()
        {
            TestRandomDefinition<TaxonomyTermGroupDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Taxonomy")]
        public void CanDeployRandom_TaxonomyTermStoreDefinition()
        {
            TestRandomDefinition<TaxonomyTermStoreDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Taxonomy")]
        public void CanDeployRandom_TaxonomyTermDefinition()
        {
            TestRandomDefinition<TaxonomyTermDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Taxonomy")]
        public void CanDeployRandom_TaxonomyTermSetDefinition()
        {
            TestRandomDefinition<TaxonomyTermSetDefinition>();
        }

        #endregion

        #region web navigation

        [TestMethod]
        [TestCategory("Regression.Rnd.Web.MasterPage")]
        public void CanDeployRandom_MasterPageSettingsDefinition()
        {
            TestRandomDefinition<MasterPageSettingsDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.Web.Navigation")]
        public void CanDeployRandom_WebNavigationSettingsDefinition()
        {
            TestRandomDefinition<WebNavigationSettingsDefinition>();
        }

        #endregion

        #region composed looks

        [TestMethod]
        [TestCategory("Regression.Rnd.ComposedLooks")]
        public void CanDeployRandom_ComposedLookItemDefinition()
        {
            TestRandomDefinition<ComposedLookItemDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.ComposedLooks")]
        public void CanDeployRandom_ComposedLookItemLinkDefinition()
        {
            TestRandomDefinition<ComposedLookItemLinkDefinition>();
        }

        #endregion

        #region reusable content

        [TestMethod]
        [TestCategory("Regression.Rnd.ReusableItems")]
        public void CanDeployRandom_ReusableTextItemDefinition()
        {
            TestRandomDefinition<ReusableTextItemDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.Rnd.ReusableItems")]
        public void CanDeployRandom_ReusableHTMLItemDefinition()
        {
            TestRandomDefinition<ReusableHTMLItemDefinition>();
        }

        #endregion
    }
}
