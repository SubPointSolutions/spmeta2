using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Regression.Model.Templates;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegSP2013WorkflowSubscriptions
    {
        public static SP2013WorkflowSubscriptionDefinition WriteToHistoryList = new SP2013WorkflowSubscriptionDefinition
        {
            Name = RegLists.GenericList.Title + " - " + RegSP2013Workflows.WriteToHistoryList.DisplayName,
            WorkflowDisplayName = RegSP2013Workflows.WriteToHistoryList.DisplayName,
            TaskListUrl = "/WorkflowTasks",
            HistoryListUrl = "/Lists/Workflow History"
        };
    }
}
