using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Regression.Tests.Impl.Syntax
{
    [TestClass]
    public class TypedSyntaxTests
    {

        #region basic specs

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        public void CanPassTypedSyntax_FarmLevel()
        {
            var model = SPMeta2Model.NewFarmModel(farm =>
            {
                farm.AddProperty(new PropertyDefinition());

                farm.AddFeature(new FeatureDefinition());
                farm.AddFarmFeature(new FeatureDefinition());

                farm.AddFarmSolution(new FarmSolutionDefinition());
                farm.AddManagedProperty(new ManagedPropertyDefinition());

                farm.AddDiagnosticsServiceBase(new DiagnosticsServiceBaseDefinition());
            });
        }

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        public void CanPassTypedSyntax_WebApplicationLevel()
        {
            var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
            {
                webApplication.AddProperty(new PropertyDefinition());

                webApplication.AddFeature(new FeatureDefinition());
                webApplication.AddWebApplicationFeature(new FeatureDefinition());

                webApplication.AddAlternateUrl(new AlternateUrlDefinition());

                webApplication.AddSite(new SiteDefinition());

                webApplication.AddPrefix(new PrefixDefinition());

                webApplication.AddContentDatabase(new ContentDatabaseDefinition());
            });
        }

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        public void CanPassTypedSyntax_SiteLevel()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddAuditSettings(new AuditSettingsDefinition());
                site.AddImageRendition(new ImageRenditionDefinition());

                site.AddRootWeb(new RootWebDefinition());

                site.AddEventReceiver(new EventReceiverDefinition());

                site.AddProperty(new PropertyDefinition());

                site.AddFeature(new FeatureDefinition());
                site.AddSiteFeature(new FeatureDefinition());

                site.AddSecurityGroup(new SecurityGroupDefinition());
                site.AddSecurityRole(new SecurityRoleDefinition());

                site.AddWeb(new WebDefinition());

                site.AddField(new FieldDefinition());
                site.AddContentType(new ContentTypeDefinition(), contentType =>
                {
                    contentType
                        .AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition())
                        .AddModuleFile(new ModuleFileDefinition())

                        .AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition())
                        .AddHideContentTypeFieldLinks(new HideContentTypeFieldLinksDefinition())
                        .AddRemoveContentTypeFieldLinks(new RemoveContentTypeFieldLinksDefinition());
                });

                site.AddSandboxSolution(new SandboxSolutionDefinition());

                site.AddTaxonomyTermStore(new TaxonomyTermStoreDefinition(), store =>
                {
                    store.AddTaxonomyTermGroup(new TaxonomyTermGroupDefinition(), group =>
                    {
                        group.AddTaxonomyTermSet(new TaxonomyTermSetDefinition(), termSet =>
                        {
                            termSet.AddTaxonomyTerm(new TaxonomyTermDefinition(), term =>
                            {
                                term.AddTaxonomyTerm(new TaxonomyTermDefinition());
                            });
                        });
                    });
                });

                site.AddRootWeb(new RootWebDefinition());
                site.AddWeb(new WebDefinition());

                site.AddUserCustomAction(new UserCustomActionDefinition());
            });
        }

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        public void CanPassTypedSyntax_WebLevel()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddAuditSettings(new AuditSettingsDefinition());
                web.AddWebNavigationSettings(new WebNavigationSettingsDefinition());
                web.AddPageLayoutAndSiteTemplateSettings(new PageLayoutAndSiteTemplateSettingsDefinition());

                web.AddRootWeb(new RootWebDefinition());

                web.AddComposedLookItemLink(new ComposedLookItemLinkDefinition());

                web.AddTreeViewSettings(new TreeViewSettingsDefinition());

                web.AddTopNavigationNode(new TopNavigationNodeDefinition());
                web.AddQuickLaunchNavigationNode(new QuickLaunchNavigationNodeDefinition());

                web.AddMasterPageSettings(new MasterPageSettingsDefinition());
                web.AddRegionalSettings(new RegionalSettingsDefinition());

                web.AddSP2013Workflow(new SP2013WorkflowDefinition());
                web.AddWelcomePage(new WelcomePageDefinition());

                web.AddEventReceiver(new EventReceiverDefinition());

                web.AddProperty(new PropertyDefinition());

                web.AddFeature(new FeatureDefinition());
                web.AddWebFeature(new FeatureDefinition());

                //web.AddSecurityGroup(new SecurityGroupDefinition());
                //web.AddSecurityRole(new SecurityRoleDefinition());

                web.AddWeb(new WebDefinition());

                web.AddField(new FieldDefinition());
                web.AddContentType(new ContentTypeDefinition());

                web.AddUserCustomAction(new UserCustomActionDefinition());

                web.AddList(new ListDefinition(), list =>
                {
                    list.AddAuditSettings(new AuditSettingsDefinition());

                    list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition());
                    list.AddHideContentTypeLinks(new HideContentTypeLinksDefinition());
                    list.AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition());

                    list.AddModuleFile(new ModuleFileDefinition());

                    list.AddUserCustomAction(new UserCustomActionDefinition());

                    list.AddReusableHTMLItem(new ReusableHTMLItemDefinition());
                    list.AddReusableTextItem(new ReusableTextItemDefinition());

                    list.AddPublishingPage(new PublishingPageDefinition());
                    list.AddPublishingPageLayout(new PublishingPageLayoutDefinition());

                    list.AddComposedLookItem(new ComposedLookItemDefinition());

                    list.AddWelcomePage(new WelcomePageDefinition());

                    list.AddEventReceiver(new EventReceiverDefinition());

                    list.AddField(new FieldDefinition());
                    list.AddContentTypeLink(new ContentTypeLinkDefinition());

                    list.AddListView(new ListViewDefinition());

                    list.AddListItem(new ListItemDefinition(), listItem =>
                    {
                        listItem
                            .AddListItemFieldValue(new ListItemFieldValueDefinition())
                            .AddListItemFieldValues(new ListItemFieldValuesDefinition());
                    });

                    list.AddFolder(new FolderDefinition(), folder =>
                    {
                        folder
                            .AddWelcomePage(new WelcomePageDefinition())
                            .AddFolder(new FolderDefinition())
                            .AddListItem(new ListItemDefinition());
                    });

                    list.AddWebPartPage(new WebPartPageDefinition(), page =>
                    {
                        page.AddDeleteWebParts(new DeleteWebPartsDefinition());
                        page.AddWebPart(new WebPartDefinition());
                    });

                    list.AddWikiPage(new WikiPageDefinition(), page =>
                    {
                        page.AddDeleteWebParts(new DeleteWebPartsDefinition());
                        page.AddWebPart(new WebPartDefinition());
                    });

                    list.AddPublishingPage(new PublishingPageDefinition(), page =>
                    {
                        page.AddDeleteWebParts(new DeleteWebPartsDefinition());
                        page.AddWebPart(new WebPartDefinition());
                    });
                });
            });
        }

        #endregion
    }
}
