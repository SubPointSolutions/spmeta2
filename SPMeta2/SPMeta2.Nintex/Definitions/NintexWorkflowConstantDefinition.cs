using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Nintex.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint Nintex workflow constant.
    /// </summary>
    public class NintexWorkflowConstantDefinition : DefinitionBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Value { get; set; }

        public bool Sensitive { get; set; }

        public string Type { get; set; }
        public string Scope { get; set; }

        public bool IsAdminOnly { get; set; }
    }
}
