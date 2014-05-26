using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppWebPartPageModels
    {
        #region properties

        public static class RootWeb
        {
            public static class SitePages
            {
                public static WebPartPageDefinition AboutFounationApp = new WebPartPageDefinition
                {
                    Title = "About Foundation App Sample",
                    FileName = "About Foundation App.aspx",
                    PageLayoutTemplate = 2
                };
            }
        }

        #endregion
    }
}
