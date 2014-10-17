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
    /// Allows to define and deploy SharePoint Nintex workflow.
    /// </summary>
    public class NintexWorkflowDefinition : DefinitionBase
    {
        public string WorkflowName { get; set; }
        public byte[] WorkflowXml { get; set; }
    }
}
