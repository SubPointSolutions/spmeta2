using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint list templates.
    /// </summary>
    public static class BuiltInListTemplates
    {
        #region properties

        /// <summary>
        /// Corresponds to built-in list template with Name [Document Library], InternamName [doclib], Description [Use a document library to store, organize, sync, and share documents with people. You can use co-authoring, versioning, and check out to work on documents together. With your documents in one place, everybody can get the latest versions whenever they need them. You can also sync your documents to your local computer for offline access.] and FeatureId: [00bfea71-e717-4e80-aa17-d0c71b360101]'
        /// </summary>
        public static ListTemplate DocumentLibrary = new ListTemplate
        {
            Name = "Document Library",
            InternalName = "doclib",
            Description = "Use a document library to store, organize, sync, and share documents with people. You can use co-authoring, versioning, and check out to work on documents together. With your documents in one place, everybody can get the latest versions whenever they need them. You can also sync your documents to your local computer for offline access.",
            FeatureId = new Guid("00bfea71-e717-4e80-aa17-d0c71b360101"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Form Library], InternamName [xmlform], Description [A place to manage business forms like status reports or purchase orders. Form libraries require a compatible XML editor, such as Microsoft InfoPath] and FeatureId: [00bfea71-1e1d-4562-b56a-f05371bb0115]'
        /// </summary>
        public static ListTemplate FormLibrary = new ListTemplate
        {
            Name = "Form Library",
            InternalName = "xmlform",
            Description = "A place to manage business forms like status reports or purchase orders. Form libraries require a compatible XML editor, such as Microsoft InfoPath",
            FeatureId = new Guid("00bfea71-1e1d-4562-b56a-f05371bb0115"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Wiki Page Library], InternamName [webpagelib], Description [An interconnected set of easily editable web pages, which can contain text, images and web parts.] and FeatureId: [00bfea71-c796-4402-9f2f-0eb9a6e71b18]'
        /// </summary>
        public static ListTemplate WikiPageLibrary = new ListTemplate
        {
            Name = "Wiki Page Library",
            InternalName = "webpagelib",
            Description = "An interconnected set of easily editable web pages, which can contain text, images and web parts.",
            FeatureId = new Guid("00bfea71-c796-4402-9f2f-0eb9a6e71b18"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Picture Library], InternamName [piclib], Description [A place to upload and share pictures.] and FeatureId: [00bfea71-52d4-45b3-b544-b1c71b620109]'
        /// </summary>
        public static ListTemplate PictureLibrary = new ListTemplate
        {
            Name = "Picture Library",
            InternalName = "piclib",
            Description = "A place to upload and share pictures.",
            FeatureId = new Guid("00bfea71-52d4-45b3-b544-b1c71b620109"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Links], InternamName [links], Description [A list of web pages or other resources.] and FeatureId: [00bfea71-2062-426c-90bf-714c59600103]'
        /// </summary>
        public static ListTemplate Links = new ListTemplate
        {
            Name = "Links",
            InternalName = "links",
            Description = "A list of web pages or other resources.",
            FeatureId = new Guid("00bfea71-2062-426c-90bf-714c59600103"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Announcements], InternamName [announce], Description [A list of news items, statuses and other short bits of information.] and FeatureId: [00bfea71-d1ce-42de-9c63-a44004ce0104]'
        /// </summary>
        public static ListTemplate Announcements = new ListTemplate
        {
            Name = "Announcements",
            InternalName = "announce",
            Description = "A list of news items, statuses and other short bits of information.",
            FeatureId = new Guid("00bfea71-d1ce-42de-9c63-a44004ce0104"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Contacts], InternamName [contacts], Description [A list of people your team works with, like customers or partners.  Contacts lists can synchronize with Microsoft Outlook or other compatible programs.] and FeatureId: [00bfea71-7e6d-4186-9ba8-c047ac750105]'
        /// </summary>
        public static ListTemplate Contacts = new ListTemplate
        {
            Name = "Contacts",
            InternalName = "contacts",
            Description = "A list of people your team works with, like customers or partners.  Contacts lists can synchronize with Microsoft Outlook or other compatible programs.",
            FeatureId = new Guid("00bfea71-7e6d-4186-9ba8-c047ac750105"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Calendar], InternamName [events], Description [A calendar of upcoming meetings, deadlines or other events.  Calendar information can be synchronized with Microsoft Outlook or other compatible programs.] and FeatureId: [00bfea71-ec85-4903-972d-ebe475780106]'
        /// </summary>
        public static ListTemplate Calendar = new ListTemplate
        {
            Name = "Calendar",
            InternalName = "events",
            Description = "A calendar of upcoming meetings, deadlines or other events.  Calendar information can be synchronized with Microsoft Outlook or other compatible programs.",
            FeatureId = new Guid("00bfea71-ec85-4903-972d-ebe475780106"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Promoted Links], InternamName [promotedlinks], Description [Use this list to display a set of link actions in a tile based visual layout.] and FeatureId: [192efa95-e50c-475e-87ab-361cede5dd7f]'
        /// </summary>
        public static ListTemplate PromotedLinks = new ListTemplate
        {
            Name = "Promoted Links",
            InternalName = "promotedlinks",
            Description = "Use this list to display a set of link actions in a tile based visual layout.",
            FeatureId = new Guid("192efa95-e50c-475e-87ab-361cede5dd7f"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Discussion Board], InternamName [discuss], Description [A place to have newsgroup-style discussions.  Discussion boards make it easy to manage discussion threads and can be configured to require approval for all posts.] and FeatureId: [00bfea71-6a49-43fa-b535-d15c05500108]'
        /// </summary>
        public static ListTemplate DiscussionBoard = new ListTemplate
        {
            Name = "Discussion Board",
            InternalName = "discuss",
            Description = "A place to have newsgroup-style discussions.  Discussion boards make it easy to manage discussion threads and can be configured to require approval for all posts.",
            FeatureId = new Guid("00bfea71-6a49-43fa-b535-d15c05500108"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Tasks (2010)], InternamName [tasks], Description [A place for team or personal tasks.] and FeatureId: [00bfea71-a83e-497e-9ba0-7a5c597d0107]'
        /// </summary>
        public static ListTemplate Tasks2010 = new ListTemplate
        {
            Name = "Tasks (2010)",
            InternalName = "tasks",
            Description = "A place for team or personal tasks.",
            FeatureId = new Guid("00bfea71-a83e-497e-9ba0-7a5c597d0107"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Project Tasks], InternamName [gantt], Description [A place for team or personal tasks.  Project tasks lists provide a Gantt Chart view and can be opened by Microsoft Project or other compatible programs.] and FeatureId: [00bfea71-513d-4ca0-96c2-6a47775c0119]'
        /// </summary>
        public static ListTemplate ProjectTasks = new ListTemplate
        {
            Name = "Project Tasks",
            InternalName = "gantt",
            Description = "A place for team or personal tasks.  Project tasks lists provide a Gantt Chart view and can be opened by Microsoft Project or other compatible programs.",
            FeatureId = new Guid("00bfea71-513d-4ca0-96c2-6a47775c0119"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Tasks], InternamName [hierarchy], Description [A place for team or personal tasks.] and FeatureId: [f9ce21f8-f437-4f7e-8bc6-946378c850f0]'
        /// </summary>
        public static ListTemplate Tasks = new ListTemplate
        {
            Name = "Tasks",
            InternalName = "hierarchy",
            Description = "A place for team or personal tasks.",
            FeatureId = new Guid("f9ce21f8-f437-4f7e-8bc6-946378c850f0"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Issue Tracking], InternamName [issue], Description [A list of issues or problems associated with a project or item.  You can assign, prioritize and track issue status.] and FeatureId: [00bfea71-5932-4f9c-ad71-1557e5751100]'
        /// </summary>
        public static ListTemplate IssueTracking = new ListTemplate
        {
            Name = "Issue Tracking",
            InternalName = "issue",
            Description = "A list of issues or problems associated with a project or item.  You can assign, prioritize and track issue status.",
            FeatureId = new Guid("00bfea71-5932-4f9c-ad71-1557e5751100"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Custom List], InternamName [custlist], Description [Using a list gives you the power to share information the way you want with your team members. Create your own list from scratch, add any other columns you need, and add items individually, or bulk edit data with Quick Edit.] and FeatureId: [00bfea71-de22-43b2-a848-c05709900100]'
        /// </summary>
        public static ListTemplate CustomList = new ListTemplate
        {
            Name = "Custom List",
            InternalName = "custlist",
            Description = "Using a list gives you the power to share information the way you want with your team members. Create your own list from scratch, add any other columns you need, and add items individually, or bulk edit data with Quick Edit.",
            FeatureId = new Guid("00bfea71-de22-43b2-a848-c05709900100"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Custom List in Datasheet View], InternamName [gridlist], Description [A blank list which is displayed as a spreadsheet in order to allow easy data entry. You can add your own columns and views. This list type requires a compatible list datasheet ActiveX control, such as the one provided in Microsoft Office.] and FeatureId: [00bfea71-3a1d-41d3-a0ee-651d11570120]'
        /// </summary>
        public static ListTemplate CustomListinDatasheetView = new ListTemplate
        {
            Name = "Custom List in Datasheet View",
            InternalName = "gridlist",
            Description = "A blank list which is displayed as a spreadsheet in order to allow easy data entry. You can add your own columns and views. This list type requires a compatible list datasheet ActiveX control, such as the one provided in Microsoft Office.",
            FeatureId = new Guid("00bfea71-3a1d-41d3-a0ee-651d11570120"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [External List], InternamName [extlist], Description [Create an external list to view the data in an External Content Type.] and FeatureId: [00bfea71-9549-43f8-b978-e47e54a10600]'
        /// </summary>
        public static ListTemplate ExternalList = new ListTemplate
        {
            Name = "External List",
            InternalName = "extlist",
            Description = "Create an external list to view the data in an External Content Type.",
            FeatureId = new Guid("00bfea71-9549-43f8-b978-e47e54a10600"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Survey], InternamName [survey], Description [A list of questions which you would like to have people answer.  Surveys allow you to quickly create questions and view graphical summaries of the responses.] and FeatureId: [00bfea71-eb8a-40b1-80c7-506be7590102]'
        /// </summary>
        public static ListTemplate Survey = new ListTemplate
        {
            Name = "Survey",
            InternalName = "survey",
            Description = "A list of questions which you would like to have people answer.  Surveys allow you to quickly create questions and view graphical summaries of the responses.",
            FeatureId = new Guid("00bfea71-eb8a-40b1-80c7-506be7590102"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Data Sources], InternamName [datasrcs], Description [Gallery for storing data source definitions] and FeatureId: [00bfea71-f381-423d-b9d1-da7a54c50110]'
        /// </summary>
        public static ListTemplate DataSources = new ListTemplate
        {
            Name = "Data Sources",
            InternalName = "datasrcs",
            Description = "Gallery for storing data source definitions",
            FeatureId = new Guid("00bfea71-f381-423d-b9d1-da7a54c50110"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Workflow History], InternamName [wrkflhis], Description [This list is used by SharePoint to store the history events for workflow instances.] and FeatureId: [00bfea71-4ea5-48d4-a4ad-305cf7030140]'
        /// </summary>
        public static ListTemplate WorkflowHistory = new ListTemplate
        {
            Name = "Workflow History",
            InternalName = "wrkflhis",
            Description = "This list is used by SharePoint to store the history events for workflow instances.",
            FeatureId = new Guid("00bfea71-4ea5-48d4-a4ad-305cf7030140"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Data Connection Library], InternamName [dcl], Description [A place where you can easily share files that contain information about external data connections.] and FeatureId: [00bfea71-dbd7-4f72-b8cb-da7ac0440130]'
        /// </summary>
        public static ListTemplate DataConnectionLibrary = new ListTemplate
        {
            Name = "Data Connection Library",
            InternalName = "dcl",
            Description = "A place where you can easily share files that contain information about external data connections.",
            FeatureId = new Guid("00bfea71-dbd7-4f72-b8cb-da7ac0440130"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Converted Forms], InternamName [IWConvertedForms], Description [List of user browser-enabled form templates on this site collection.] and FeatureId: [a0e5a010-1329-49d4-9e09-f280cdbed37d]'
        /// </summary>
        public static ListTemplate ConvertedForms = new ListTemplate
        {
            Name = "Converted Forms",
            InternalName = "IWConvertedForms",
            Description = "List of user browser-enabled form templates on this site collection.",
            FeatureId = new Guid("a0e5a010-1329-49d4-9e09-f280cdbed37d"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Status List], InternamName [KPIList], Description [A place to track and display a set of goals.  Colored icons display the degree to which the goals have been achieved.] and FeatureId: [065c78be-5231-477e-a972-14177cc5b3c7]'
        /// </summary>
        public static ListTemplate StatusList = new ListTemplate
        {
            Name = "Status List",
            InternalName = "KPIList",
            Description = "A place to track and display a set of goals.  Colored icons display the degree to which the goals have been achieved.",
            FeatureId = new Guid("065c78be-5231-477e-a972-14177cc5b3c7"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Asset Library], InternamName [AssetLibrary], Description [A place to share, browse and manage rich media assets, like image, audio and video files.] and FeatureId: [4bcccd62-dcaf-46dc-a7d4-e38277ef33f4]'
        /// </summary>
        public static ListTemplate AssetLibrary = new ListTemplate
        {
            Name = "Asset Library",
            InternalName = "AssetLibrary",
            Description = "A place to share, browse and manage rich media assets, like image, audio and video files.",
            FeatureId = new Guid("4bcccd62-dcaf-46dc-a7d4-e38277ef33f4"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Access App], InternamName [AccSrvAddApp], Description [Access web app.] and FeatureId: [d2b9ec23-526b-42c5-87b6-852bd83e0364]'
        /// </summary>
        public static ListTemplate AccessApp = new ListTemplate
        {
            Name = "Access App",
            InternalName = "AccSrvAddApp",
            Description = "Access web app.",
            FeatureId = new Guid("d2b9ec23-526b-42c5-87b6-852bd83e0364"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Persistent Storage List for MySite Published Feed], InternamName [MicroBlogList], Description [MySite MicroFeed Persistent Storage List] and FeatureId: [ea23650b-0340-4708-b465-441a41c37af7]'
        /// </summary>
        public static ListTemplate PersistentStorageListforMySitePublishedFeed = new ListTemplate
        {
            Name = "Persistent Storage List for MySite Published Feed",
            InternalName = "MicroBlogList",
            Description = "MySite MicroFeed Persistent Storage List",
            FeatureId = new Guid("ea23650b-0340-4708-b465-441a41c37af7"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Pages Library], InternamName [Pages], Description [A list template for creating Pages list in Publishing feature] and FeatureId: [22a9ef51-737b-4ff2-9346-694633fe4416]'
        /// </summary>
        public static ListTemplate PagesLibrary = new ListTemplate
        {
            Name = "Pages Library",
            InternalName = "Pages",
            Description = "A list template for creating Pages list in Publishing feature",
            FeatureId = new Guid("22a9ef51-737b-4ff2-9346-694633fe4416"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Custom Workflow Process], InternamName [workflowProcess], Description [Custom Workflow Process tracking list for this web.] and FeatureId: [00bfea71-2d77-4a75-9fca-76516689e21a]'
        /// </summary>
        public static ListTemplate CustomWorkflowProcess = new ListTemplate
        {
            Name = "Custom Workflow Process",
            InternalName = "workflowProcess",
            Description = "Custom Workflow Process tracking list for this web.",
            FeatureId = new Guid("00bfea71-2d77-4a75-9fca-76516689e21a"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [No Code Workflows], InternamName [nocodewf], Description [Gallery for storing No Code Workflows] and FeatureId: [00bfea71-f600-43f6-a895-40c0de7b0117]'
        /// </summary>
        public static ListTemplate NoCodeWorkflows = new ListTemplate
        {
            Name = "No Code Workflows",
            InternalName = "nocodewf",
            Description = "Gallery for storing No Code Workflows",
            FeatureId = new Guid("00bfea71-f600-43f6-a895-40c0de7b0117"),
            Hidden = true
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [Report Library], InternamName [ReportList], Description [A place where you can easily create and manage web pages and documents to track metrics, goals and business intelligence information. ] and FeatureId: [2510d73f-7109-4ccc-8a1c-314894deeb3a]'
        /// </summary>
        public static ListTemplate ReportLibrary = new ListTemplate
        {
            Name = "Report Library",
            InternalName = "ReportList",
            Description = "A place where you can easily create and manage web pages and documents to track metrics, goals and business intelligence information. ",
            FeatureId = new Guid("2510d73f-7109-4ccc-8a1c-314894deeb3a"),
            Hidden = false
        };

        /// <summary>
        /// Corresponds to built-in list template with Name [No Code Public Workflows], InternamName [nocodepublicwf], Description [Gallery for storing No Code Public Workflows] and FeatureId: [00bfea71-f600-43f6-a895-40c0de7b0117]'
        /// </summary>
        public static ListTemplate NoCodePublicWorkflows = new ListTemplate
        {
            Name = "No Code Public Workflows",
            InternalName = "nocodepublicwf",
            Description = "Gallery for storing No Code Public Workflows",
            FeatureId = new Guid("00bfea71-f600-43f6-a895-40c0de7b0117"),
            Hidden = true
        };


        #endregion
    }
}
