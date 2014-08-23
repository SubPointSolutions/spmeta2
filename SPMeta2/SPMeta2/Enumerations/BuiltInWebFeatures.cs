using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint web features.
    /// </summary>
    public static class BuiltInWebFeatures
    {
        /// <summary>
        /// Access web app.
        /// </summary>
        public static FeatureDefinition AccessApp = new FeatureDefinition
        {
            Title = "Access App",
            Id = new Guid("{d2b9ec23-526b-42c5-87b6-852bd83e0364}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// BICenter Data Connections Feature
        /// </summary>
        public static FeatureDefinition BICenterDataConnectionsFeature = new FeatureDefinition
        {
            Title = "BICenter Data Connections Feature",
            Id = new Guid("{3d8210e9-1e89-4f12-98ef-643995339ed4}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds class and group content to SharePoint My Site Host site collection.
        /// </summary>
        public static FeatureDefinition ClassMySiteHostContent = new FeatureDefinition
        {
            Title = "Class My Site Host Content",
            Id = new Guid("{932f5bb1-e815-4c14-8917-c2bae32f70fe}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds required content types to the SharePoint Class web.
        /// </summary>
        public static FeatureDefinition ClassWebTypes = new FeatureDefinition
        {
            Title = "Class Web Types",
            Id = new Guid("{a16e895c-e61a-11df-8f6e-103edfd72085}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature adds community functionality such as discussion categories, content and people reputation, and the members list. It also provisions community site pages which contain these lists and features.
        /// </summary>
        public static FeatureDefinition CommunitySiteFeature = new FeatureDefinition
        {
            Title = "Community Site Feature",
            Id = new Guid("{961d6a9c-4388-4cf2-9733-38ee8c89afd4}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Make the data stored in this SharePoint site available using the Content Management Interoperability Services (CMIS) standard.  
        /// </summary>
        public static FeatureDefinition ContentManagementInteroperabilityServices_CMISProducer = new FeatureDefinition
        {
            Title = "Content Management Interoperability Services (CMIS) Producer",
            Id = new Guid("{1fce0577-1f58-4fc2-a996-6c4bcf59eeed}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Create metadata based rules that move content submitted to this site to the correct library or folder.
        /// </summary>
        public static FeatureDefinition ContentOrganizer = new FeatureDefinition
        {
            Title = "Content Organizer",
            Id = new Guid("{7ad5272a-2694-4349-953e-ea5ef290e97c}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature enables Alerts and Event Receivers on the External List and External Content Types.
        /// </summary>
        public static FeatureDefinition ExternalSystemEvents = new FeatureDefinition
        {
            Title = "External System Events",
            Id = new Guid("{5b10d113-2d0d-43bd-a2fd-f8bc879f5abd}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enable users to follow documents or sites.
        /// </summary>
        public static FeatureDefinition FollowingContent = new FeatureDefinition
        {
            Title = "Following Content",
            Id = new Guid("{a7a2793e-67cd-4dc1-9fd0-43f61581207a}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// 
        /// </summary>
        public static FeatureDefinition FranchiseeEventHandlers = new FeatureDefinition
        {
            Title = "Franchisee EventHandlers",
            Id = new Guid("{53f0c5eb-b0df-490f-a186-ad0005a945ac}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides a tile view experience for common SharePoint site actions.
        /// </summary>
        public static FeatureDefinition GettingStarted = new FeatureDefinition
        {
            Title = "Getting Started",
            Id = new Guid("{4aec7207-0d02-4f4f-aa07-b370199cd0c7}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature is used to track external actions like litigations, investigations, or audits that require you to suspend the disposition of documents.
        /// </summary>
        public static FeatureDefinition Hold = new FeatureDefinition
        {
            Title = "Hold",
            Id = new Guid("{9e56487c-795a-4077-9425-54a1ecb84282}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides each list in the site with a settings pages for configuring that list to use metadata tree view hierarchies and filter controls to improve navigation and filtering of the contained items.
        /// </summary>
        public static FeatureDefinition MetadataNavigationAndFiltering = new FeatureDefinition
        {
            Title = "Metadata Navigation and Filtering",
            Id = new Guid("{7201d6a4-a5d3-49a1-8c19-19c4bac6e668}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// A technique that delivers a faster and more fluid page navigation experience, in pages and site templates that support it, by downloading and rendering only those portions of a page that are changing.
        /// </summary>
        public static FeatureDefinition MinimalDownloadStrategy = new FeatureDefinition
        {
            Title = "Minimal Download Strategy",
            Id = new Guid("{87294c72-f260-42f3-a41b-981a2ffce37a}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provide document library and other lists in team site with mobile view for smartphone browsers.
        /// </summary>
        public static FeatureDefinition MobileBrowserView = new FeatureDefinition
        {
            Title = "Mobile Browser View",
            Id = new Guid("{d95c97f3-e528-4da2-ae9f-32b3535fbb59}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enables offline synchronization between external lists and Outlook.
        /// </summary>
        public static FeatureDefinition OfflineSynchronizationForExternalLists = new FeatureDefinition
        {
            Title = "Offline Synchronization for External Lists",
            Id = new Guid("{d250636f-0a26-4019-8425-a5232d592c01}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features enabling the PerformancePoint Services list and document library templates.
        /// </summary>
        public static FeatureDefinition PerformancePointServicesSiteFeatures = new FeatureDefinition
        {
            Title = "PerformancePoint Services Site Features",
            Id = new Guid("{0b07a7f4-8bb8-4ec0-a31b-115732b9584d}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature adds project management functionality to a site. It includes tasks, a calendar, and web parts on the home page of the site.
        /// </summary>
        public static FeatureDefinition ProjectFunctionality = new FeatureDefinition
        {
            Title = "Project Functionality",
            Id = new Guid("{e2f2bb18-891d-4812-97df-c265afdba297}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Installs content types designed to manage search config.
        /// </summary>
        public static FeatureDefinition SearchConfigDataContentTypes = new FeatureDefinition
        {
            Title = "Search Config Data Content Types",
            Id = new Guid("{48a243cb-7b16-4b5a-b1b5-07b809b43f47}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Installs columns designed to manage information about search configurations.
        /// </summary>
        public static FeatureDefinition SearchConfigDataSiteColumns = new FeatureDefinition
        {
            Title = "Search Config Data Site Columns",
            Id = new Guid("{41dfb393-9eb6-4fe4-af77-28e4afce8cdc}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Create Search Config List Instance: Provisions a list to enable the import and export of search configurations
        /// </summary>
        public static FeatureDefinition SearchConfigListInstanceFeature = new FeatureDefinition
        {
            Title = "Search Config List Instance Feature",
            Id = new Guid("{acb15743-f07b-4c83-8af3-ffcfdf354965}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Create Search Config Template: Provisions a template for the Search Config List to enable the import and export of search configurations
        /// </summary>
        public static FeatureDefinition SearchConfigTemplateFeature = new FeatureDefinition
        {
            Title = "Search Config Template Feature",
            Id = new Guid("{e47705ec-268d-4c41-aa4e-0d8727985ebc}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features such as Visio Services, Access Services, and Excel Services Application, included in the SharePoint Server Enterprise License.
        /// </summary>
        public static FeatureDefinition SharePointServerEnterpriseSiteFeatures = new FeatureDefinition
        {
            Title = "SharePoint Server Enterprise Site features",
            Id = new Guid("{0806d127-06e6-447a-980e-2e90b03101b8}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Create a Web page library as well as supporting libraries to create and publish pages based on page layouts.
        /// </summary>
        public static FeatureDefinition SharePointServerPublishing = new FeatureDefinition
        {
            Title = "SharePoint Server Publishing",
            Id = new Guid("{94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features such as user profiles and search, included in the SharePoint Server Standard License.
        /// </summary>
        public static FeatureDefinition SharePointServerStandardSiteFeatures = new FeatureDefinition
        {
            Title = "SharePoint Server Standard Site features",
            Id = new Guid("{99fe402e-89a0-45aa-9163-85342e865dc8}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enables the use of site feeds.
        /// </summary>
        public static FeatureDefinition SiteFeed = new FeatureDefinition
        {
            Title = "Site Feed",
            Id = new Guid("{15a572c6-e545-4d32-897a-bab6f5846e18}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// The Site Mailbox app helps you keep email and documents close together by connecting your site to an Exchange mailbox. You can then view your email on SharePoint, and view site documents in Outlook.
        /// </summary>
        public static FeatureDefinition SiteMailbox = new FeatureDefinition
        {
            Title = "Site Mailbox",
            Id = new Guid("{502a2d54-6102-4757-aaa0-a90586106368}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Creates a Microsoft OneNote 2010 notebook in the Shared Documents library and places a link to it on the Quick Launch. This feature requires a properly configured WOPI application server to create OneNote 2010 notebooks.
        /// </summary>
        public static FeatureDefinition SiteNotebook = new FeatureDefinition
        {
            Title = "Site Notebook",
            Id = new Guid("{f151bb39-7c3b-414f-bb36-6bf18872052f}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides team collaboration capabilities for a site by making standard lists, such as document libraries and issues, available.
        /// </summary>
        public static FeatureDefinition TeamCollaborationLists = new FeatureDefinition
        {
            Title = "Team Collaboration Lists",
            Id = new Guid("{00bfea71-4ea5-48d4-a4ad-7ea5c011abe5}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This site feature will create a wiki page and set it as your site home page.
        /// </summary>
        public static FeatureDefinition WikiPageHomePage = new FeatureDefinition
        {
            Title = "Wiki Page Home Page",
            Id = new Guid("{00bfea71-d8fe-4fec-8dad-01c19a6e4053}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Allow workflows to read from and to write to all items in this site.
        /// </summary>
        public static FeatureDefinition WorkflowsCanUseAppPermissions = new FeatureDefinition
        {
            Title = "Workflows can use app permissions",
            Id = new Guid("{ec918931-c874-4033-bd09-4f36b2e31fef}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds the SharePoint 2013 Workflow Task content type to the site.
        /// </summary>
        public static FeatureDefinition WorkflowTaskContentType = new FeatureDefinition
        {
            Title = "Workflow Task Content Type",
            Id = new Guid("{57311b7a-9afd-4ff0-866e-9393ad6647b1}"),
            Scope = FeatureDefinitionScope.Web,
            ForceActivate = false,
            Enable = false
        };

    }
}
