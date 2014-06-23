using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.Model.Templates;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Model.Definitions
{
    public class RegSP2013Workflows
    {
        public static SP2013WorkflowDefinition WriteToHistoryList = new SP2013WorkflowDefinition
        {
            DisplayName = "Write to history list",
            Xaml = WorkflowXAMLDefinitions.WriteToHistoryListWorkflow,
            Override = true
        };
    }
}
