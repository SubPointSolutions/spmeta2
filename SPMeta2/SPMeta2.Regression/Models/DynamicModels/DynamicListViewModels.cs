using System;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public static class DynamicListViewModels
    {
        #region properties

        public static ListViewDefinition AllDocuments = GetListViewTestTemplate("All Documents", view =>
        {
            view.Fields = new System.Collections.ObjectModel.Collection<string>(new[] { "ID", "Title" });
        });

        public static ListViewDefinition AllItems = GetListViewTestTemplate("All Items", view =>
        {
            view.Fields = new System.Collections.ObjectModel.Collection<string>(new[] { "ID", "Title" });
        });

        public static ListViewDefinition AllTasks = GetListViewTestTemplate("All Tasks", view =>
        {
            view.Fields = new System.Collections.ObjectModel.Collection<string>(new[] { "ID", "Title", "Body" });
        });

        #endregion

        #region methods

        public static ListViewDefinition GetListViewTestTemplate(string name, Action<ListViewDefinition> action)
        {
            var result = new ListViewDefinition
            {
                Title = string.Format("{0} test view {1}", name, Environment.TickCount),
                RowLimit = new Random(Environment.TickCount).Next(1, 100),
                IsPaged = Environment.TickCount % 2 == 0,
                IsDefault = Environment.TickCount % 2 == 0
            };

            if (action != null) action(result);

            return result;
        }

        #endregion
    }
}