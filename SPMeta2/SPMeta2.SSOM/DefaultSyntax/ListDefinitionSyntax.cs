using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.SSOM.DefaultSyntax
{
    public static class ListDefinitionSyntax
    {
        #region methods

        #region behavior support

        //public static DefinitionBase OnCreating(this DefinitionBase model, Action<ListDefinition, SPList> action)
        //{
        //    model.RegisterModelUpdatingEvent(action);

        //    return model;
        //}

        //public static DefinitionBase OnCreated(this DefinitionBase model, Action<ListDefinition, SPList> action)
        //{
        //    model.RegisterModelUpdatedEvent(action);

        //    return model;
        //}

        #endregion

        #region add content type

        public static ModelNode AddContentTypeLink(this ModelNode model, SPContentTypeId contentTypeId)
        {
            return ContentTypeLinkDefinitionSyntax.AddContentTypeLink(model, new ContentTypeLinkDefinition
            {
                ContentTypeId = contentTypeId.ToString()
            });
        }

        #endregion

        #endregion

        #region utils

        [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use ListDefinition.CustomUrl property instead.")]
        public static string GetListUrl(this ListDefinition listDefinition)
        {
            if (!string.IsNullOrEmpty(listDefinition.CustomUrl))
                return listDefinition.CustomUrl;

            if (listDefinition.Url.ToUpper().Contains("_CATALOGS"))
                return listDefinition.Url;

            // BIG BIG BIG TODO
            // correct list/doc lib mapping has to be implemented and tested
            // very critical part of the whole provision lib

            var templateType = (SPListTemplateType)listDefinition.TemplateType;

            switch (templateType)
            {
                case SPListTemplateType.Events:
                case SPListTemplateType.Tasks:

#if !NET35
                case SPListTemplateType.TasksWithTimelineAndHierarchy:
#endif
                case SPListTemplateType.GenericList:
                case SPListTemplateType.AdminTasks:
                case SPListTemplateType.Agenda:
                case SPListTemplateType.Announcements:
                case SPListTemplateType.Comments:
                case SPListTemplateType.Contacts:
                case SPListTemplateType.DiscussionBoard:
                case SPListTemplateType.ExternalList:
                case SPListTemplateType.Links:
                case SPListTemplateType.WorkflowHistory:
                case SPListTemplateType.HealthRules:
                case SPListTemplateType.GanttTasks:
                case SPListTemplateType.ListTemplateCatalog:

                    return "Lists/" + listDefinition.Url;

                case SPListTemplateType.CallTrack:
                case SPListTemplateType.Categories:
                case SPListTemplateType.Circulation:
                case SPListTemplateType.CustomGrid:
                case SPListTemplateType.DataConnectionLibrary:
                case SPListTemplateType.DataSources:
                case SPListTemplateType.Decision:
                case SPListTemplateType.DocumentLibrary:

                case SPListTemplateType.Facility:

                case SPListTemplateType.HealthReports:

                case SPListTemplateType.Holidays:
                case SPListTemplateType.HomePageLibrary:
                case SPListTemplateType.IMEDic:
                case SPListTemplateType.InvalidType:
                case SPListTemplateType.IssueTracking:


                case SPListTemplateType.MasterPageCatalog:
                case SPListTemplateType.MeetingObjective:
                case SPListTemplateType.MeetingUser:
                case SPListTemplateType.Meetings:
                case SPListTemplateType.NoCodePublic:
                case SPListTemplateType.NoCodeWorkflows:
                case SPListTemplateType.NoListTemplate:
                case SPListTemplateType.PictureLibrary:
                case SPListTemplateType.Posts:
                case SPListTemplateType.SolutionCatalog:
                case SPListTemplateType.Survey:
                case SPListTemplateType.TextBox:
                case SPListTemplateType.ThemeCatalog:
                case SPListTemplateType.ThingsToBring:
                case SPListTemplateType.Timecard:
                case SPListTemplateType.UserInformation:
                case SPListTemplateType.WebPageLibrary:
                case SPListTemplateType.WebPartCatalog:
                case SPListTemplateType.WebTemplateCatalog:
                case SPListTemplateType.Whereabouts:

                case SPListTemplateType.WorkflowProcess:
                case SPListTemplateType.XMLForm:

                    break;
                default:
                    break;
            }

            return listDefinition.Url;
        }

        #endregion
    }
}
