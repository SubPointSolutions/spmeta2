using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Standard.Enumerations
{
    /// <summary>
    /// Predefined ContentClass values for search queries.
    /// Based on the following MSDN blog - http://blogs.msdn.com/b/mvpawardprogram/archive/2015/02/16/sharepoint-power-searching-using-contentclass.aspx
    /// </summary>
    public static class BuiltInContentClass
    {
        public static class URN
        {
            /// <summary>
            /// News Listing
            /// urn:content-class:SPSListing:News
            /// </summary>
            public static string SPSListingNews = "urn:content-class:SPSListing:News";

            /// <summary>
            /// People                                  
            /// urn:content-class:SPSPeople
            /// </summary>
            public static string SPSPeople = "urn:content-class:SPSPeople";

            /// <summary>
            /// Search Query
            /// urn:content-class:SPSSearchQuery
            /// </summary>
            public static string SPSSearchQuery = "urn:content-class:SPSSearchQuery";

            /// <summary>
            /// Category
            /// urn:content-classes:SPSCategory
            /// </summary>
            public static string SPSCategory = "urn:content-classes:SPSCategory";

            /// <summary>
            /// Listing
            /// urn:content-classes:SPSListing
            /// </summary>
            public static string SPSListing = "urn:content-classes:SPSListing";

            /// <summary>
            /// Person Listing
            /// urn:content-classes:SPSPersonListing
            /// </summary>
            public static string SPSPersonListing = "urn:content-classes:SPSPersonListing";

            /// <summary>
            /// Site Listing
            /// urn:content-classes:SPSSiteListing
            /// </summary>
            public static string SPSSiteListing = "urn:content-classes:SPSSiteListing";

            /// <summary>
            /// Site Registry Listing
            /// urn:content-classes:SPSSiteRegistry
            /// </summary>
            public static string SPSSiteRegistry = "urn:content-classes:SPSSiteRegistry";

            /// <summary>
            /// Text Listing
            /// urn:content-classes:SPSTextListing
            /// </summary>
            public static string SPSTextListing = "urn:content-classes:SPSTextListing";
        }

        public static class NonLists
        {
            public static string STS_Site = "STS_Site";
            public static string STS_Web = "STS_Web";
            public static string STS_List = "STS_List";
            public static string STS_Document = "STS_Document";
            public static string STS_ListItem = "STS_ListItem";
        }

        public static class Lists
        {
            /// <summary>
            /// GenericList
            /// STS_List
            /// </summary>
            public static string STS_List = "STS_List";

            /// <summary>
            /// DocumentLibrary
            /// STS_List_DocumentLibrary
            /// </summary>
            public static string STS_List_DocumentLibrary = "STS_List_DocumentLibrary";

            /// <summary>
            /// Survey
            /// STS_List_Survey
            /// </summary>
            public static string STS_List_Survey = "STS_List_Survey";

            /// <summary>
            /// Links
            /// STS_List_Links
            /// </summary>
            public static string STS_List_Links = "STS_List_Links";

            /// <summary>
            /// Announcements
            /// STS_List_Announcements
            /// </summary>
            public static string STS_List_Announcements = "STS_List_Announcements";

            /// <summary>
            /// Contacts
            /// STS_List_Contacts
            /// </summary>
            public static string STS_List_Contacts = "STS_List_Contacts";

            /// <summary>
            /// Events
            /// STS_List_Events
            /// </summary>
            public static string STS_List_Events = "STS_List_Events";

            /// <summary>
            /// DocumentLibrary
            /// Tasks
            /// </summary>
            public static string STS_List_Tasks = "STS_List_Tasks";

            /// <summary>
            /// DiscussionBoard
            /// STS_List_DiscussionBoard
            /// </summary>
            public static string STS_List_DiscussionBoard = "STS_List_DiscussionBoard";

            /// <summary>
            /// PictureLibrary
            /// STS_List_PictureLibrary
            /// </summary>
            public static string STS_List_PictureLibrary = "STS_List_PictureLibrary";

            /// <summary>
            /// XMLForm
            /// STS_List_XMLForm
            /// </summary>
            public static string STS_List_XMLForm = "STS_List_XMLForm";

            /// <summary>
            /// WebPageLibrary
            /// STS_List_WebPageLibrary
            /// </summary>
            public static string STS_List_WebPageLibrary = "STS_List_WebPageLibrary";

            /// <summary>
            /// DataConnectionLibrary
            /// STS_List_DataConnectionLibrary
            /// </summary>
            public static string STS_List_DataConnectionLibrary = "STS_List_DataConnectionLibrary";

            /// <summary>
            /// Preservation Hold Library
            /// STS_List_131
            /// </summary>
            public static string STS_List_131 = "STS_List_131";

            /// <summary>
            /// GanttTasks
            /// STS_List_GanttTasks
            /// </summary>
            public static string STS_List_GanttTasks = "STS_List_GanttTasks";

            /// <summary>
            /// Promoted Links
            /// STS_List_170
            /// </summary>
            public static string STS_List_170 = "STS_List_170";

            /// <summary>
            /// TasksWithTimelineAndHierarchy
            /// STS_List_TasksWithTimelineAndHierarchy
            /// </summary>
            public static string STS_List_TasksWithTimelineAndHierarchy = "STS_List_TasksWithTimelineAndHierarchy";

            /// <summary>
            /// Agenda
            /// STS_List_Agenda
            /// </summary>
            public static string STS_List_Agenda = "STS_List_Agenda";

            /// <summary>
            /// MeetingUser
            /// STS_List_MeetingUser
            /// </summary>
            public static string STS_List_MeetingUser = "STS_List_MeetingUser";

            /// <summary>
            /// Decision
            /// STS_List_Decision
            /// </summary>
            public static string STS_List_Decision = "STS_List_Decision";

            /// <summary>
            /// MeetingObjective
            /// STS_List_MeetingObjective
            /// </summary>
            public static string STS_List_MeetingObjective = "STS_List_MeetingObjective";

            /// <summary>
            /// TextBox
            /// STS_List_TextBox
            /// </summary>
            public static string STS_List_TextBox = "STS_List_TextBox";

            /// <summary>
            /// ThingsToBring
            /// STS_List_ThingsToBring
            /// </summary>
            public static string STS_List_ThingsToBring = "STS_List_ThingsToBring";

            /// <summary>
            /// Posts
            /// STS_List_Posts
            /// </summary>
            public static string STS_List_Posts = "STS_List_Posts";


            /// <summary>
            /// Comments
            /// STS_List_Comments
            /// </summary>
            public static string STS_List_Comments = "STS_List_Comments";

            /// <summary>
            /// Categories
            /// STS_List_Categories
            /// </summary>
            public static string STS_List_Categories = "STS_List_Categories";

            /// <summary>
            /// App Catalog
            /// STS_List_330
            /// </summary>
            public static string STS_List_330 = "STS_List_330";


            /// <summary>
            /// Apps for Office
            /// STS_List_332
            /// </summary>
            public static string STS_List_332 = "STS_List_332";

            /// <summary>
            /// App Requests
            /// STS_List_333
            /// </summary>
            public static string STS_List_333 = "STS_List_333";

            /// <summary>
            /// USysApplicationLog - Access
            /// STS_List_398
            /// </summary>
            public static string STS_List_398 = "STS_List_398";

            /// <summary>
            /// MSysASO - Access
            /// STS_List_399
            /// </summary>
            public static string STS_List_399 = "STS_List_399";

            /// <summary>
            /// Facility
            /// STS_List_Facility
            /// </summary>
            public static string STS_List_Facility = "STS_List_Facility";

            /// <summary>
            /// Whereabouts
            /// STS_List_Whereabouts
            /// </summary>
            public static string STS_List_Whereabouts = "STS_List_Whereabouts";

            /// <summary>
            /// CallTrack
            /// STS_List_CallTrack
            /// </summary>
            public static string STS_List_CallTrack = "STS_List_CallTrack";

            /// <summary>
            /// Circulation
            /// STS_List_Circulation
            /// </summary>
            public static string STS_List_Circulation = "STS_List_Circulation";

            /// <summary>
            /// Timecard
            /// STS_List_TimeCard
            /// </summary>
            public static string STS_List_TimeCard = "STS_List_TimeCard";

            /// <summary>
            /// StatusIndicatorList / KPIs
            /// STS_List_432
            /// </summary>
            public static string STS_List_432 = "STS_List_432";

            /// <summary>
            /// Report Library
            /// STS_List_433
            /// </summary>
            public static string STS_List_433 = "STS_List_433";

            /// <summary>
            /// Dashboard content
            /// STS_List_450
            /// </summary>
            public static string STS_List_450 = "STS_List_450";

            /// <summary>
            /// Data Sources - Perfomance Point
            /// STS_List_460
            /// </summary>
            public static string STS_List_460 = "STS_List_460";

            /// <summary>
            /// Dashboards
            /// STS_List_480
            /// </summary>
            public static string STS_List_480 = "STS_List_480";

            /// <summary>
            /// Categories - community site
            /// STS_List_500
            /// </summary>
            public static string STS_List_500 = "STS_List_500";

            /// <summary>
            /// Visio Repository Site Process Diagrams
            /// STS_List_506
            /// </summary>
            public static string STS_List_506 = "STS_List_506";

            /// <summary>
            /// MicroBlogList (MicroFeed)
            /// STS_List_544
            /// </summary>
            public static string STS_List_544 = "STS_List_544";

            /// <summary>
            /// MySiteDocumentLibrary
            /// STS_List_MySiteDocumentLibrary
            /// </summary>
            public static string STS_List_MySiteDocumentLibrary = "STS_List_MySiteDocumentLibrary";

            /// <summary>
            /// Product Catalog
            /// STS_List_751
            /// </summary>
            public static string STS_List_751 = "STS_List_751";

            /// <summary>
            /// Pages
            /// STS_List_850
            /// </summary>
            public static string STS_List_850 = "STS_List_850";

            /// <summary>
            /// Asset Library / Video Channel
            /// STS_List_851
            /// </summary>
            public static string STS_List_851 = "STS_List_851";

            /// <summary>
            /// Video Channel Settings
            /// STS_List_852
            /// </summary>
            public static string STS_List_852 = "STS_List_852";

            /// <summary>
            /// Hub Settings
            /// STS_List_853
            /// </summary>
            public static string STS_List_853 = "STS_List_853";

            /// <summary>
            /// Members - community site
            /// STS_List_880
            /// </summary>
            public static string STS_List_880 = "STS_List_880";

            /// <summary>
            /// IssueTracking
            /// STS_List_IssueTracking
            /// </summary>
            public static string STS_List_IssueTracking = "STS_List_IssueTracking";

            /// <summary>
            /// DeveloperSiteDraftApps
            /// STS_List_DeveloperSiteDraftApps
            /// </summary>
            public static string STS_List_DeveloperSiteDraftApps = "STS_List_DeveloperSiteDraftApps";

            /// <summary>
            /// EDiscoverySources
            /// STS_List_1305
            /// </summary>
            public static string STS_List_1305 = "STS_List_1305";

            /// <summary>
            /// EDiscoverySourceGroups / Sets
            /// STS_List_1307
            /// </summary>
            public static string STS_List_1307 = "STS_List_1307";

            /// <summary>
            /// EDiscoveryCustodians
            /// STS_List_1308
            /// </summary>
            public static string STS_List_1308 = "STS_List_1308";

            /// <summary>
            /// Custom Lists / Queries
            /// STS_List_1309
            /// </summary>
            public static string STS_List_1309 = "STS_List_1309";

            /// <summary>
            /// EDiscoveryExports
            /// STS_List_1310
            /// </summary>
            public static string STS_List_1310 = "STS_List_1310";

            /// <summary>
            /// Slide Library
            /// STS_List_2100
            /// </summary>
            public static string STS_List_2100 = "STS_List_2100";

            /// <summary>
            /// Acquisition History List
            /// STS_List_10099
            /// </summary>
            public static string STS_List_10099 = "STS_List_10099";
        }
    }
}
