using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public class UserCustomActionDefinition : DefinitionBase
    {
        #region properties

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Group { get; set; }
        public string Location { get; set; }

        public string ScriptSrc { get; set; }
        public string ScriptBlock { get; set; }

        public int Sequence { get; set; }

        #endregion
    }
}
