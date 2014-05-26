using System.Collections.ObjectModel;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class ListViewModels
    {
        #region properties

        public static class LastDocs
        {
            public static ListViewDefinition LastDocuments = new ListViewDefinition
            {
                Title = "Last Documents",
                Fields = new Collection<string>(new string[]
                    {
                        "ID",
                        "Title"
                    }),
                RowLimit = 5
            };
        }

        #endregion
    }
}
