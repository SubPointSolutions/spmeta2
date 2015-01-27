using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint site features.
    /// </summary>
    public static class BuiltInSiteFeatures
    {
        #region custom added

        /// <summary>
        /// Makes the following Web Parts available in the site collection Web Part catalog: Page Viewer, Content Editor, Image, Form, XML and Site Users list.
        /// </summary>
        public static FeatureDefinition BasicWebParts = new FeatureDefinition
        {
            Title = "BasicWebParts",
            Id = new Guid("{00bfea71-1c5e-4a24-b310-ba51c3eb7a57}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        #endregion

        #region auto generated

        /// <summary>
        /// Content Deployment Source feature enables content deployment specific checks on source site collection and enables setting up content deployment from the site collection to a target site collection.
        /// </summary>
        public static FeatureDefinition ContentDeploymentSourceFeature = new FeatureDefinition
        {
            Title = "Content Deployment Source Feature",
            Id = new Guid("{cd1a49b0-c067-4fdd-adfe-69e6f5022c1a}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provisions a site to be Enterprise Metadata hub site.
        /// </summary>
        public static FeatureDefinition ContentTypeSyndicationHub = new FeatureDefinition
        {
            Title = "Content Type Syndication Hub",
            Id = new Guid("{9a447926-5937-44cb-857a-d3829301c73b}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Use the cross-farm site permissions feature to allow internal SharePoint applications to access websites across farms.
        /// </summary>
        public static FeatureDefinition CrossFarmSitePermissions = new FeatureDefinition
        {
            Title = "Cross-Farm Site Permissions",
            Id = new Guid("{a5aedf1a-12e5-46b4-8348-544386d5312d}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enables site collection to designate lists and document libraries as catalog sources for Cross-Site Collection Publishing.
        /// </summary>
        public static FeatureDefinition CrossSiteCollectionPublishing = new FeatureDefinition
        {
            Title = "Cross-Site Collection Publishing",
            Id = new Guid("{151d22d9-95a8-4904-a0a3-22e4db85d1e0}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Creates a Help library that can be used to store custom help for this site collection.
        /// </summary>
        public static FeatureDefinition CustomSiteCollectionHelp = new FeatureDefinition
        {
            Title = "Custom Site Collection Help",
            Id = new Guid("{57ff23fc-ec05-4dd8-b7ed-d93faa7c795d}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Manages document expiration and retention by allowing participants to decide whether to retain or delete expired documents.
        /// </summary>
        public static FeatureDefinition DispositionApprovalWorkflow = new FeatureDefinition
        {
            Title = "Disposition Approval Workflow",
            Id = new Guid("{c85e5759-f323-4efb-b548-443d2216efb5}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Assigns IDs to documents in the Site Collection, which can be used to retrieve items independent of their current location.
        /// </summary>
        public static FeatureDefinition DocumentIDService = new FeatureDefinition
        {
            Title = "Document ID Service",
            Id = new Guid("{b50e3104-6812-424f-a011-cc90e6327318}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides the content types required for creating and using document sets. Create a document set when you want to manage multiple documents as a single work product.
        /// </summary>
        public static FeatureDefinition DocumentSets = new FeatureDefinition
        {
            Title = "Document Sets",
            Id = new Guid("{3bae86a2-776d-499d-9db8-fa4cdc7884f8}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enable the definition and declaration of records in place.
        /// </summary>
        public static FeatureDefinition InPlaceRecordsManagement = new FeatureDefinition
        {
            Title = "In Place Records Management",
            Id = new Guid("{da2e115b-07e4-49d9-bb2c-35e93bb9fca9}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Allows list administrators to override content type retention schedules and set schedules on libraries and folders.
        /// </summary>
        public static FeatureDefinition LibraryAndFolderBasedRetention = new FeatureDefinition
        {
            Title = "Library and Folder Based Retention",
            Id = new Guid("{063c26fa-3ccc-4180-8a84-b6f98e991df3}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// When this feature is enabled, permissions for users in the "limited access" permissions level (such as Anonymous Users) are reduced, preventing access to Application Pages.
        /// </summary>
        public static FeatureDefinition LimitedAccessUserPermissionLockdownMode = new FeatureDefinition
        {
            Title = "Limited-access user permission lockdown mode",
            Id = new Guid("{7c637b23-06c4-472d-9a9a-7c175762c5c4}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Configures links to documents so they open in client applications instead of Web applications, by default.
        /// </summary>
        public static FeatureDefinition OpenDocumentsInClientApplicationsByDefault = new FeatureDefinition
        {
            Title = "Open Documents in Client Applications by Default",
            Id = new Guid("{8a4b8de2-6fd8-41e9-923c-c7c3c00f8295}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features enabling the PerformancePoint Services site including content types and site definitions for this site collection.
        /// </summary>
        public static FeatureDefinition PerformancePointServicesSiteCollectionFeatures = new FeatureDefinition
        {
            Title = "PerformancePoint Services Site Collection Features",
            Id = new Guid("{a1cb5b7f-e5e9-421b-915f-bf519b0760ef}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Routes a page for approval. Approvers can approve or reject the page, reassign the approval task, or request changes to the page. This workflow can be edited in SharePoint Designer.
        /// </summary>
        public static FeatureDefinition PublishingApprovalWorkflow = new FeatureDefinition
        {
            Title = "Publishing Approval Workflow",
            Id = new Guid("{a44d2aa3-affc-4d58-8db4-f4a3af053188}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Creates reports about information in Microsoft SharePoint Foundation.
        /// </summary>
        public static FeatureDefinition Reporting = new FeatureDefinition
        {
            Title = "Reporting",
            Id = new Guid("{7094bd89-2cfe-490a-8c7e-fbace37b4a34}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides content types, site columns, and library templates required to support Reports and Data Search in the Enterprise Search Center.
        /// </summary>
        public static FeatureDefinition ReportsAndDataSearchSupport = new FeatureDefinition
        {
            Title = "Reports and Data Search Support",
            Id = new Guid("{b435069a-e096-46e0-ae30-899daca4b304}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature improves the search engine optimization of a website by automatically generating a search engine sitemap on a recurring basis that contains all valid URLs in a SharePoint website. Anonymous access must be enabled in order to use this feature.
        /// </summary>
        public static FeatureDefinition SearchEngineSitemap = new FeatureDefinition
        {
            Title = "Search Engine Sitemap",
            Id = new Guid("{77fc9e13-e99a-4bd3-9438-a3f69670ed97}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature will add the Search Server Web Parts and Display Templates to your site. Search will work on most sites without this feature being activated, but if you get a message about missing templates when searching, then activate this feature.
        /// </summary>
        public static FeatureDefinition SearchServerWebPartsAndTemplates = new FeatureDefinition
        {
            Title = "Search Server Web Parts and Templates",
            Id = new Guid("{9c0834e1-ba47-4d49-812b-7d4fb6fea211}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Aggregated set of out-of-box workflow features provided by SharePoint 2007.
        /// </summary>
        public static FeatureDefinition SharePoint2007Workflows = new FeatureDefinition
        {
            Title = "SharePoint 2007 Workflows",
            Id = new Guid("{c845ed8d-9ce5-448c-bd3e-ea71350ce45b}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features such as InfoPath Forms Services, Visio Services, Access Services, and Excel Services Application, included in the SharePoint Server Enterprise License.
        /// </summary>
        public static FeatureDefinition SharePointServerEnterpriseSiteCollectionFeatures = new FeatureDefinition
        {
            Title = "SharePoint Server Enterprise Site Collection features",
            Id = new Guid("{8581a8a7-cf16-4770-ac54-260265ddb0b2}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides centralized libraries, content types, master pages and page layouts and enables page scheduling and other publishing functionality for a site collection.
        /// </summary>
        public static FeatureDefinition SharePointServerPublishingInfrastructure = new FeatureDefinition
        {
            Title = "SharePoint Server Publishing Infrastructure",
            Id = new Guid("{f6924d36-2fa8-4f0b-b16d-06b7250180fa}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features such as user profiles and search, included in the SharePoint Server Standard License.
        /// </summary>
        public static FeatureDefinition SharePointServerStandardSiteCollectionFeatures = new FeatureDefinition
        {
            Title = "SharePoint Server Standard Site Collection features",
            Id = new Guid("{b21b090c-c796-4b0f-ac0f-7ef1659c20ae}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Allows site collection administrators to define retention schedules that apply to a site and all its content.
        /// </summary>
        public static FeatureDefinition SitePolicy = new FeatureDefinition
        {
            Title = "Site Policy",
            Id = new Guid("{2fcd5f8a-26b7-4a6a-9755-918566dba90a}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Use this workflow to track items in a list.
        /// </summary>
        public static FeatureDefinition ThreeStateWorkflow = new FeatureDefinition
        {
            Title = "Three-state workflow",
            Id = new Guid("{fde5d850-671e-4143-950a-87b473922dc7}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides libraries, content types, and web parts for storing, managing, and viewing rich media assets, like images, sound clips, and videos.
        /// </summary>
        public static FeatureDefinition VideoAndRichMedia = new FeatureDefinition
        {
            Title = "Video and Rich Media",
            Id = new Guid("{6e1e5426-2ebd-4871-8027-c5ca86371ead}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Aggregated set of out-of-box workflow features provided by SharePoint.
        /// </summary>
        public static FeatureDefinition Workflows = new FeatureDefinition
        {
            Title = "Workflows",
            Id = new Guid("{0af5989a-3aea-4519-8ab0-85d91abe39ff}"),
            Scope = FeatureDefinitionScope.Site,
            ForceActivate = false,
            Enable = false
        };

        #endregion
    }
}
