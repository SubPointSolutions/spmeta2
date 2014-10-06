using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions.Webparts
{
    public class ContentEditorWebPartDefinition : WebPartDefinitionBase
    {
        #region properties

        public string Content { get; set; }
        public string ContentLink { get; set; }

        #endregion
    }
}
