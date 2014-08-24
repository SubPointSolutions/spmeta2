using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions
{
    public class BreakRoleInheritanceDefinition : DefinitionBase
    {
        #region properties

        public bool CopyRoleAssignments { get; set; }
        public bool ClearSubscopes { get; set; }

        public bool ForceClearSubscopes { get; set; }

        #endregion
    }
}
