using System;
using SPMeta2.Definitions;
using SPMeta2.Regression.Const;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public static class DynamicListModels
    {
        #region properties

        public static ListDefinition Pages = new ListDefinition
        {
            Title = "Pages",
            Url = "Pages",
            TemplateType = 119,
            ContentTypesEnabled = true
        };

        public static ListDefinition SitePages = new ListDefinition
        {
            Title = "Site Pages",
            Url = "SitePages",
            TemplateType = 119,
            Description = "Use this library to create and store pages on this site.",
            ContentTypesEnabled = true
        };

        public static ListDefinition DocumentLibrary = GetListTestTemplate(SPListTemplateType.DocumentLibrary);
        public static ListDefinition AnnouncementsList = GetListTestTemplate(SPListTemplateType.Announcements);
        public static ListDefinition ContactsList = GetListTestTemplate(SPListTemplateType.Contacts);
        public static ListDefinition EventsList = GetListTestTemplate(SPListTemplateType.Events);
        public static ListDefinition GenericList = GetListTestTemplate(SPListTemplateType.GenericList);
        public static ListDefinition LinksList = GetListTestTemplate(SPListTemplateType.Links);
        public static ListDefinition TasksList = GetListTestTemplate(SPListTemplateType.Tasks);

        #endregion

        #region methods

        public static ListDefinition GetListTestTemplate(SPListTemplateType listTemplateType)
        {
            return GetListTestTemplate(listTemplateType, null);
        }

        public static ListDefinition GetListTestTemplate(SPListTemplateType listTemplateType, Action<ListDefinition> action)
        {
            var result = new ListDefinition
            {
                Title = string.Format("{0} test list", listTemplateType.ToString()),
                Url = string.Format("{0}testlist", listTemplateType.ToString()),
                TemplateType = (int)listTemplateType,
                Description = Guid.NewGuid().ToString(),
                ContentTypesEnabled = true
            };

            if (action != null) action(result);

            return result;
        }

        #endregion
    }
}