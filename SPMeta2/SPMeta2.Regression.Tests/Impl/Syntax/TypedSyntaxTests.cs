using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Syntax
{
    [TestClass]
    public class TypedSyntaxTests
    {

        #region basic specs

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        [TestCategory("CI.Core")]
        public void CanPassTypedSyntax_FarmLevel()
        {
            var model = SPMeta2Model.NewFarmModel(farm =>
            {
                farm
                    .AddProperty(new PropertyDefinition())
                    .AddProperty(new PropertyDefinition());

                farm.AddTrustedAccessProvider(new TrustedAccessProviderDefinition());

                farm.AddFeature(new FeatureDefinition());
                farm.AddFarmFeature(new FeatureDefinition());

                farm.AddFarmSolution(new FarmSolutionDefinition());
                farm.AddManagedProperty(new ManagedPropertyDefinition());

                farm.AddDiagnosticsServiceBase(new DiagnosticsServiceBaseDefinition());

                farm.AddDeveloperDashboardSettings(new DeveloperDashboardSettingsDefinition());

                farm.AddWebApplication(new WebApplicationDefinition());
            });
        }

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        [TestCategory("CI.Core")]
        public void CanPassTypedSyntax_WebApplicationLevel()
        {
            var model = SPMeta2Model.NewWebApplicationModel(webApplication =>
            {
                webApplication
                    .AddProperty(new PropertyDefinition())
                    .AddProperty(new PropertyDefinition());

                webApplication.AddFeature(new FeatureDefinition());
                webApplication.AddWebApplicationFeature(new FeatureDefinition());

                webApplication.AddAlternateUrl(new AlternateUrlDefinition());

                webApplication.AddSite(new SiteDefinition());

                webApplication.AddPrefix(new PrefixDefinition());

                webApplication.AddContentDatabase(new ContentDatabaseDefinition());

                webApplication.AddOfficialFileHost(new OfficialFileHostDefinition());

                webApplication.AddSuiteBar(new SuiteBarDefinition());

                webApplication.AddFarmSolution(new FarmSolutionDefinition());

                webApplication.AddPeoplePickerSettings(new PeoplePickerSettingsDefinition());
            });
        }

        [TestMethod]
        [TestCategory("Regression.Syntax")]
        [TestCategory("CI.Core")]
        public void CanPassTypedSyntax_SiteLevel()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSharePointDesignerSettings(new SharePointDesignerSettingsDefinition());

                site.AddRootWeb(new RootWebDefinition { }, rootWeb =>
                {
                    rootWeb.AddTopNavigationNode(new TopNavigationNodeDefinition());
                });

                site.AddCoreProperty(new CorePropertyDefinition())
                    .AddCoreProperty(new CorePropertyDefinition(), property =>
                    {
                        property.AddProfileTypeProperty(new ProfileTypePropertyDefinition());
                    });


                site.AddAuditSettings(new AuditSettingsDefinition());
                site.AddImageRendition(new ImageRenditionDefinition());

                site.AddRootWeb(new RootWebDefinition());

                site.AddEventReceiver(new EventReceiverDefinition());

                site
                    .AddProperty(new PropertyDefinition())
                    .AddProperty(new PropertyDefinition());

                site.AddFeature(new FeatureDefinition());
                site.AddSiteFeature(new FeatureDefinition());

                site.AddSecurityGroup(new SecurityGroupDefinition(), group =>
                {
                    group
                        .AddUser(new UserDefinition())
                        .AddUser(new UserDefinition());

                    // TODO

                    // .AddSecurityRoleLink() is missed on SecurityGroup #601
                    // https://github.com/SubPointSolutions/spmeta2/issues/601 
                    group.AddSecurityRoleLink(new SecurityRoleLinkDefinition());
                    group.AddSecurityRoleLink(new SecurityRoleLinkDefinition());
                });

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

                                // .AddTaxonomyTermLabel() is missed on TaxonomyTermDefinition #602
                                // https://github.com/SubPointSolutions/spmeta2/issues/602
                                term.AddTaxonomyTermLabel(new TaxonomyTermLabelDefinition());
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
        [TestCategory("CI.Core")]
        public void CanPassTypedSyntax_WebLevel()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddClearRecycleBin(new ClearRecycleBinDefinition());

                web
                    .AddAnonymousAccessSettings(new AnonymousAccessSettingsDefinition())
                    .AddAnonymousAccessSettings(new AnonymousAccessSettingsDefinition());

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

                web
                  .AddProperty(new PropertyDefinition())
                  .AddProperty(new PropertyDefinition());

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
                    list.AddDiscussionItem(new DiscussionItemDefinition(), item =>
                    {
                        item.AddDiscussionReplyItem(new DiscussionReplyItemDefinition());
                    });

                    list.AddDocumentSet(new DocumentSetDefinition());

                    list.AddAuditSettings(new AuditSettingsDefinition());

                    list.AddMasterPage(new MasterPageDefinition());
                    list.AddHtmlMasterPage(new HtmlMasterPageDefinition());

                    list
                      .AddProperty(new PropertyDefinition())
                      .AddProperty(new PropertyDefinition());


                    list.AddContentTypeLink(new ContentTypeLinkDefinition(), contentTypeLink =>
                    {
                        contentTypeLink.AddWorkflowAssociation(new WorkflowAssociationDefinition());

                    });
                    list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition());
                    list.AddHideContentTypeLinks(new HideContentTypeLinksDefinition());
                    list.AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition());

                    list.AddModuleFile(new ModuleFileDefinition(), moduleFile =>
                    {
                        moduleFile.AddSecurityGroupLink(new SecurityGroupDefinition(), group =>
                       {
                           group
                               .AddSecurityRoleLink(BuiltInSecurityRoleNames.Edit)
                               .AddSecurityRoleLink(BuiltInSecurityRoleNames.Design)
                               .AddSecurityRoleLink(BuiltInSecurityRoleNames.Approve);
                       });
                    });

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

                    list.AddListView(new ListViewDefinition(), listView =>
                    {
                        // Enhance 'WebPartDefinition' provision - enabne privision under list views #590
                        // https://github.com/SubPointSolutions/spmeta2/issues/590

                        listView.AddDeleteWebParts(new DeleteWebPartsDefinition());
                        listView.AddWebPart(new WebPartDefinition());
                    });

                    list.AddListItem(new ListItemDefinition(), listItem =>
                    {
                        listItem
                            .AddListItemFieldValue(new ListItemFieldValueDefinition())
                            .AddListItemFieldValues(new ListItemFieldValuesDefinition());
                    });

                    list.AddFolder(new FolderDefinition(), folder =>
                    {
                        folder
                            .AddProperty(new PropertyDefinition())
                            .AddProperty(new PropertyDefinition())

                            .AddDocumentSet(new DocumentSetDefinition())

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


        [TestMethod]
        [TestCategory("Regression.Syntax.Extensions")]
        [TestCategory("CI.Core")]
        public void CanPassTypedSyntax_Extensions()
        {
            var tmpDir = Path.Combine(
                                Path.GetTempPath(),
                                string.Format("m2_regression_test_CanPassTypedSyntax_Extensions_{0}", Guid.NewGuid().ToString("N")));

            Directory.CreateDirectory(tmpDir);

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    ModuleFileUtils.LoadModuleFilesFromLocalFolder(list, tmpDir);

                    list.AddRandomFolder(folder =>
                    {
                        ModuleFileUtils.LoadModuleFilesFromLocalFolder(folder, tmpDir);
                    });
                });
            });
        }

        #endregion
    }
}
