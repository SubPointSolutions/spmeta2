using System.Collections.ObjectModel;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppListViewModels
    {
        #region properties

        public static class RootWeb
        {
            public static class Departments
            {
                public static ListViewDefinition AboutUsPageView = new ListViewDefinition
                {
                    Title = "About Us Page View",
                    Fields = new Collection<string>(new string[]
                    {
                        "Title",
                        AppFieldModels.DepartmentCode.InternalName
                    }),
                    RowLimit = 5
                };
            }

            public static class CompanyAnnouncements
            {
                public static ListViewDefinition MainPageView = new ListViewDefinition
                {
                    Title = "Main Page View",
                    Query = "<OrderBy><FieldRef Name=\"ID\" Ascending=\"False\" /></OrderBy>",
                    Fields = new Collection<string>(new string[]
                    {
                        "ID",
                        "Title",
                        "Body",
                        "Expires",
                        AppFieldModels.ShowOnTheMainPage.InternalName
                    }),
                    RowLimit = 5
                };
            }
        }

        #endregion
    }
}
