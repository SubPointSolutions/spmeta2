using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions.Webparts
{
    public class ListViewWebPartDefinition : WebPartDefinitionBase
    {
        #region properties

        public string ListName { get; set; }
        public string ListUrl { get; set; }
        public Guid? ListId { get; set; }

        public string ViewName { get; set; }
        public Guid? ViewId { get; set; }

        #endregion
    }
}
