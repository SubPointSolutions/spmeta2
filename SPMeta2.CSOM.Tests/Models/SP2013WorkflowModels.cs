using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public class SP2013WorkflowModels
    {
        public static SP2013WorkflowDefinition WriteToHistoryList = new SP2013WorkflowDefinition
        {
            DisplayName = "Write to history list",
            Xaml = WorkflowXAMLDefinitions.WriteToHistoryListWorkflow
        };
    }
}
