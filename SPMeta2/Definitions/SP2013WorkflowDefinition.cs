using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace SPMeta2.Definitions
{
    public class SP2013WorkflowDefinition : DefinitionBase
    {
        #region contructors

        public SP2013WorkflowDefinition()
        {
            Override = false;
        }

        #endregion

        #region properties

        public string DisplayName { get; set; }
        public string Xaml { get; set; }

        public bool Override { get; set; }

        #endregion
    }
}
