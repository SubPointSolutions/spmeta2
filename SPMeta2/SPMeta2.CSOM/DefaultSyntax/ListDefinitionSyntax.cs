using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;

namespace SPMeta2.CSOM.DefaultSyntax
{
    [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ModernSyntax.OnProvisioning/OnProvisioned events.")]
    public static class ListDefinitionSyntax
    {
        #region utils

        [Obsolete("Obsolete, left due to backward compatibility. Use ListDefinition.CustomUrl prop setting web-related list URL instead - http://docs.subpointsolutions.com/spmeta2/kb/m2-methods-GetListUrl")]
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

        #endregion
    }
}
