using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Extensions
{
    public static class ListDefinitionExtensions
    {
        public static string GetListUrl(this ListDefinition listDefinition)
        {

            if (!string.IsNullOrEmpty(listDefinition.CustomUrl))
                return listDefinition.CustomUrl;

            if (listDefinition.Url.ToUpper().Contains("_CATALOGS"))
                return listDefinition.Url;

            // BIG BIG BIG TODO
            // correct list/doc lib mapping has to be implemented and tested
            // very critical part of the whole provision lib

            var templateType = (ListTemplateType)listDefinition.TemplateType;

            switch (templateType)
            {
                case ListTemplateType.Events:
                case ListTemplateType.Tasks:
                case ListTemplateType.GenericList:
                case ListTemplateType.AdminTasks:
                case ListTemplateType.Agenda:
                case ListTemplateType.Announcements:
                case ListTemplateType.Comments:
                case ListTemplateType.Contacts:
                case ListTemplateType.DiscussionBoard:
                case ListTemplateType.ExternalList:
                case ListTemplateType.Links:
                case ListTemplateType.WorkflowHistory:
                case ListTemplateType.Facility:
                case ListTemplateType.GanttTasks:
                case ListTemplateType.Posts:

                    return "Lists/" + listDefinition.Url;

                case ListTemplateType.CallTrack:
                case ListTemplateType.Categories:
                case ListTemplateType.Circulation:
                case ListTemplateType.CustomGrid:
                case ListTemplateType.DataConnectionLibrary:
                case ListTemplateType.DataSources:
                case ListTemplateType.Decision:
                case ListTemplateType.DocumentLibrary:


                case ListTemplateType.HealthReports:
                case ListTemplateType.HealthRules:
                case ListTemplateType.Holidays:
                case ListTemplateType.HomePageLibrary:
                case ListTemplateType.IMEDic:
                case ListTemplateType.InvalidType:
                case ListTemplateType.IssueTracking:

                case ListTemplateType.ListTemplateCatalog:
                case ListTemplateType.MasterPageCatalog:
                case ListTemplateType.MeetingObjective:
                case ListTemplateType.MeetingUser:
                case ListTemplateType.Meetings:
                case ListTemplateType.NoCodePublic:
                case ListTemplateType.NoCodeWorkflows:
                case ListTemplateType.NoListTemplate:
                case ListTemplateType.PictureLibrary:

                case ListTemplateType.SolutionCatalog:
                case ListTemplateType.Survey:
                case ListTemplateType.TextBox:
                case ListTemplateType.ThemeCatalog:
                case ListTemplateType.ThingsToBring:
                case ListTemplateType.Timecard:
                case ListTemplateType.UserInformation:
                case ListTemplateType.WebPageLibrary:
                case ListTemplateType.WebPartCatalog:
                case ListTemplateType.WebTemplateCatalog:
                case ListTemplateType.Whereabouts:

                case ListTemplateType.WorkflowProcess:
                case ListTemplateType.XMLForm:

                    break;
                default:
                    break;
            }

            return listDefinition.Url;
        }


        private enum ListTemplateType
        {
            InvalidType = -1,
            NoListTemplate = 0,
            GenericList = 100,
            DocumentLibrary = 101,
            Survey = 102,
            Links = 103,
            Announcements = 104,
            Contacts = 105,
            Events = 106,
            Tasks = 107,
            DiscussionBoard = 108,
            PictureLibrary = 109,
            DataSources = 110,
            WebTemplateCatalog = 111,
            UserInformation = 112,
            WebPartCatalog = 113,
            ListTemplateCatalog = 114,
            XMLForm = 115,
            MasterPageCatalog = 116,
            NoCodeWorkflows = 117,
            WorkflowProcess = 118,
            WebPageLibrary = 119,
            CustomGrid = 120,
            SolutionCatalog = 121,
            NoCodePublic = 122,
            ThemeCatalog = 123,
            DesignCatalog = 124,
            AppDataCatalog = 125,
            DataConnectionLibrary = 130,
            WorkflowHistory = 140,
            GanttTasks = 150,
            HelpLibrary = 151,
            AccessRequest = 160,
            TasksWithTimelineAndHierarchy = 171,
            MaintenanceLogs = 175,
            Meetings = 200,
            Agenda = 201,
            MeetingUser = 202,
            Decision = 204,
            MeetingObjective = 207,
            TextBox = 210,
            ThingsToBring = 211,
            HomePageLibrary = 212,
            Posts = 301,
            Comments = 302,
            Categories = 303,
            Facility = 402,
            Whereabouts = 403,
            CallTrack = 404,
            Circulation = 405,
            Timecard = 420,
            Holidays = 421,
            IMEDic = 499,
            ExternalList = 600,
            MySiteDocumentLibrary = 700,
            IssueTracking = 1100,
            AdminTasks = 1200,
            HealthRules = 1220,
            HealthReports = 1221,
            DeveloperSiteDraftApps = 1230,
        }
    }
}
