using SPMeta2.Definitions;

namespace SubPointSolutions.Docs.Code.Definitions
{
    public static class DocFolders
    {
        #region properties

        public static class WikiPages
        {
            public static FolderDefinition News = new FolderDefinition
            {
                Name = "News"
            };

            public static FolderDefinition Archive = new FolderDefinition
            {
                Name = "Archive"
            };
        }

        public static class Years
        {
            public static FolderDefinition Year2013 = new FolderDefinition
            {
                Name = "2013"
            };

            public static FolderDefinition Year2014 = new FolderDefinition
            {
                Name = "2014"
            };

            public static FolderDefinition Year2015 = new FolderDefinition
            {
                Name = "2015"
            };
        }


        public static class Quarters
        {
            public static FolderDefinition Q1 = new FolderDefinition
            {
                Name = "Q1"
            };


            public static FolderDefinition Q2 = new FolderDefinition
            {
                Name = "Q2"
            };


            public static FolderDefinition Q3 = new FolderDefinition
            {
                Name = "Q3"
            };


            public static FolderDefinition Q4 = new FolderDefinition
            {
                Name = "Q4"
            };

        }

        #endregion
    }
}
