using System.Collections.ObjectModel;

namespace SPMeta2.Definitions
{
    public class ListViewDefinition : DefinitionBase
    {
        #region contructors

        public ListViewDefinition()
        {
            Fields = new Collection<string>();
            IsPaged = true;
            RowLimit = 30;
        }

        #endregion

        #region properties

        public string Title { get; set; }
        public int RowLimit { get; set; }

        public string Query { get; set; }

        public bool IsPaged { get; set; }
        public bool IsDefault { get; set; }

        public Collection<string> Fields { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] IsDefault:[{1}] Query:[{2}]", Title, IsDefault, Query);
        }

        #endregion
    }
}
