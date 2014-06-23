using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegListViews
    {
        #region properties

        public static ListViewDefinition View1 = new ListViewDefinition
        {
            Title = "View1",
            IsDefault = true,
            Fields = new Collection<string>(new string[]
                    {
                        "ID",
                        "Title"
                    }),
            RowLimit = 5
        };

        public static ListViewDefinition View2 = new ListViewDefinition
        {
            Title = "View2",
            Fields = new Collection<string>(new string[]
                    {
                        "ID",
                        "Title"
                    }),
            RowLimit = 5
        };


        #endregion
    }
}
