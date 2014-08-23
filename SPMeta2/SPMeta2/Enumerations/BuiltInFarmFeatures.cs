using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint farm features.
    /// </summary>
    public static class BuiltInFarmFeatures
    {
        /// <summary>
        /// Enables the Access Services Add Access Application feature in each site that is created.
        /// </summary>
        public static FeatureDefinition AccessServiceAddAccessApplicationFeatureStapler = new FeatureDefinition
        {
            Title = "Access Service Add Access Application Feature Stapler",
            Id = new Guid("{3d7415e4-61ba-4669-8d78-213d374d9825}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds farm-level Access Services 2010 Features to the Microsoft SharePoint Foundation framework
        /// </summary>
        public static FeatureDefinition AccessServices2010FarmFeature = new FeatureDefinition
        {
            Title = "Access Services 2010 Farm Feature",
            Id = new Guid("{1cc4b32c-299b-41aa-9770-67715ea05f25}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds farm-level Access Services Features to the Microsoft SharePoint Foundation framework
        /// </summary>
        public static FeatureDefinition AccessServicesFarmFeature = new FeatureDefinition
        {
            Title = "Access Services Farm Feature",
            Id = new Guid("{5094e988-524b-446c-b2f6-040b5be46297}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds entry points in the ribbon user interface for creating library shortcuts in the user's SharePoint Sites list if they have a recent version of Office installed.  Office will periodically cache templates available in those libraries on the user's local machine.
        /// </summary>
        public static FeatureDefinition ConnecttoOfficeRibbonControls = new FeatureDefinition
        {
            Title = "Connect to Office Ribbon Controls",
            Id = new Guid("{ff48f7e6-2fa1-428d-9a15-ab154762043d}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds Data Connection Library feature
        /// </summary>
        public static FeatureDefinition DataConnectionLibrary = new FeatureDefinition
        {
            Title = "Data Connection Library",
            Id = new Guid("{cdfa39c6-6413-4508-bccf-bf30368472b3}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds farm-level Excel Services Application viewing features to the Microsoft SharePoint Foundation framework
        /// </summary>
        public static FeatureDefinition ExcelServicesApplicationViewFarmFeature = new FeatureDefinition
        {
            Title = "Excel Services Application View Farm Feature",
            Id = new Guid("{e4e6a041-bc5b-45cb-beab-885a27079f74}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds farm-level Excel Services Application web part features to the Microsoft SharePoint Foundation framework
        /// </summary>
        public static FeatureDefinition ExcelServicesApplicationWebPartFarmFeature = new FeatureDefinition
        {
            Title = "Excel Services Application Web Part Farm Feature",
            Id = new Guid("{c6ac73de-1936-47a4-bdff-19a6fc3ba490}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enables the Exchange Sync Timer Job for Work Management Service Application. Customizes the ribbon for SharePoint Server Tasks lists, enabling users to opt in and out of the Work Management synchronization with Exchange Server.
        /// </summary>
        public static FeatureDefinition FarmLevelExchangeTasksSync = new FeatureDefinition
        {
            Title = "Farm Level Exchange Tasks Sync",
            Id = new Guid("{5f68444a-0131-4bb0-b013-454d925681a2}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Installs additional web parts common to all types of sites.
        /// </summary>
        public static FeatureDefinition GlobalWebParts = new FeatureDefinition
        {
            Title = "Global Web Parts",
            Id = new Guid("{319d8f70-eb3a-4b44-9c79-2087a87799d6}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature enables entry points from the SharePoint user interface that allow users to browse SharePoint solutions from Office.com
        /// </summary>
        public static FeatureDefinition OfficeAtComEntryPointsfromSharePoint = new FeatureDefinition
        {
            Title = "Office.com Entry Points from SharePoint",
            Id = new Guid("{a140a1ac-e757-465d-94d4-2ca25ab2c662}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enables offline synchronization for external lists with Outlook and SharePoint Workspace.
        /// </summary>
        public static FeatureDefinition OfflineSynchronizationforExternalLists = new FeatureDefinition
        {
            Title = "Offline Synchronization for External Lists",
            Id = new Guid("{f9cb1a2a-d285-465a-a160-7e3e95af1fdd}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature provides the server to server authentication capabilities.
        /// </summary>
        public static FeatureDefinition SharePointServertoServerAuthentication = new FeatureDefinition
        {
            Title = "SharePoint Server to Server Authentication",
            Id = new Guid("{5709886f-13cc-4ffc-bfdc-ec8ab7f77191}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enables the provisioning of an Exchange mailbox for sites on this farm and connects the documents from the site to Outlook.
        /// </summary>
        public static FeatureDefinition SiteMailboxes = new FeatureDefinition
        {
            Title = "Site Mailboxes",
            Id = new Guid("{3a11d8ef-641e-4c79-b4d9-be3b17f9607c}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Adds entry points for social tagging and note board commenting to the ribbon user interface.
        /// </summary>
        public static FeatureDefinition SocialTagsandNoteBoardRibbonControls = new FeatureDefinition
        {
            Title = "Social Tags and Note Board Ribbon Controls",
            Id = new Guid("{756d8a58-4e24-4288-b981-65dc93f9c4e5}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Enable the Spell Checking in list-item edit forms.
        /// </summary>
        public static FeatureDefinition SpellChecking = new FeatureDefinition
        {
            Title = "Spell Checking",
            Id = new Guid("{612d671e-f53d-4701-96da-c3a4ee00fdc5}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// This feature provisions the User Profile User Settings Provider.
        /// </summary>
        public static FeatureDefinition UserProfileUserSettingsProvider = new FeatureDefinition
        {
            Title = "User Profile User Settings Provider",
            Id = new Guid("{0867298a-70e0-425f-85df-7f8bd9e06f15}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Visio Process Repository document library feature
        /// </summary>
        public static FeatureDefinition VisioProcessRepository = new FeatureDefinition
        {
            Title = "Visio Process Repository",
            Id = new Guid("{7e0aabee-b92b-4368-8742-21ab16453d00}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// View Visio Web Drawings in the browser
        /// </summary>
        public static FeatureDefinition VisioWebAccess = new FeatureDefinition
        {
            Title = "Visio Web Access",
            Id = new Guid("{5fe8e789-d1b7-44b3-b634-419c531cfdca}"),
            Scope = FeatureDefinitionScope.Farm,
            ForceActivate = false,
            Enable = false
        };
    }
}
