using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Common;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Regression.Utils;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Utils;
using SPMeta2.Attributes.Regression;

using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Enumerations;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Exceptions;
using System.IO;
using SPMeta2.Validation.Services;
using SPMeta2.Regression.Assertion;
using SPMeta2.Regression.Runners.Config;

namespace SPMeta2.Regression.Tests.Impl.Random
{
    [TestClass]
    public class RandomDefinitionTest : SPMeta2RegresionEventsTestBase
    {
        public RandomDefinitionTest()
        {
            this.ProvisionGenerationCount = 2;
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

            var allDefinitions = ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(spMetaAssembly)
                                                .ToList();

            allDefinitions.AddRange(ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(spMetaStandardAssembly)
                .ToList());

            foreach (var def in allDefinitions)
            {
                Trace.WriteLine(def.Name);
            }

            foreach (var definition in allDefinitions)
            {
                var hasTestMethod = HasTestMethod("CanDeployRandom_", definition, methods);

                Assert.IsTrue(hasTestMethod);
            }
        }

        #endregion

        #region farm scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Farm")]
        public void CanDeployRandom_FarmDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<FarmDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Farm")]
        public void CanDeployRandom_ManagedAccountDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<ManagedAccountDefinition>();
            });
        }

        #endregion

        #region web app scope

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_WebApplicationDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<WebApplicationDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_JobDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<JobDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.WebApplication")]
        public void CanDeployRandom_PrefixDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<PrefixDefinition>();
            });
        }

        #endregion

        #region site scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_SiteDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
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
        [TestCategory("Regression.Rnd.Site")]
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
        [TestCategory("Regression.Rnd.Site")]
        public void CanDeployRandom_BusinessDataFieldDefinition()
        {
            TestRandomDefinition<BusinessDataFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.Rnd.Site")]
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
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<FarmSolutionDefinition>();
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
        public void CanDeployRandom_WebDefinition()
        {
            TestRandomDefinition<WebDefinition>();
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
        public void CanDeployRandom_BreakRoleInheritanceDefinition()
        {
            TestRandomDefinition<BreakRoleInheritanceDefinition>();
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

        #region pages scope

        [TestMethod]
        [TestCategory("Regression.Rnd.Pages")]
        public void CanDeployRandom_PublishingPageDefinition()
        {
            TestRandomDefinition<PublishingPageDefinition>();
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
            TestRandomDefinition<AppPrincipalDefinition>();
        }

        #endregion

        #region webparts

        [TestMethod]
        [TestCategory("Regression.Rnd.Webparts")]
        public void CanDeployRandom_ContentByQueryWebPartDefinition()
        {
            TestRandomDefinition<ContentByQueryWebPartDefinition>();
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
        public void CanDeployRandom_ContactFieldControlDefinition()
        {
            TestRandomDefinition<ContactFieldControlDefinition>();
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
    }
}
