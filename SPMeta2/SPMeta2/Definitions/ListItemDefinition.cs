using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public class ListItemDefinition : DefinitionBase
    {
        #region constructors

        public ListItemDefinition()
        {
            Overwrite = true;
            //Content = new byte[0];
        }

        #endregion

        #region properties

        public string Title { get; set; }

        public bool Overwrite { get; set; }

        public bool SystemUpdate { get; set; }
        public bool SystemUpdateIncrementVersionNumber { get; set; }

        public bool UpdateOverwriteVersion { get; set; }

        // should be collection of attachments later
        //public byte[] Content { get; set; }

        // TODO, serializable dictionary for propertied such as content type and so on

        #endregion
    }
}
