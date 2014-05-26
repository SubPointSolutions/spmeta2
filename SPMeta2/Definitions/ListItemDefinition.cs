using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public class ListItemDefinition : DefinitionBase
    {
        #region contructors

        public ListItemDefinition()
        {
            Overwrite = true;
            Content = new byte[0];
        }

        #endregion

        #region properties

        public string Name { get; set; }

        public bool Overwrite { get; set; }

        public bool SystemUpdate { get; set; }
        public bool SystemUpdateIncrementVersionNumber { get; set; }

        public bool UpdateOverwriteVersion { get; set; }

        public byte[] Content { get; set; }

        // TODO, serializable dictionary for propertied such as content type and so on

        #endregion
    }
}
