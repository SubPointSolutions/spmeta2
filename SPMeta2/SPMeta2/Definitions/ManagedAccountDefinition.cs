using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint managed account.
    /// </summary>
    public class ManagedAccountDefinition : DefinitionBase
    {
        #region properties

        public string LoginName { get; set; }

        #endregion
    }
}
