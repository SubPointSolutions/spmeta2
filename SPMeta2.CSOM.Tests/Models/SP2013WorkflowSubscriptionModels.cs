using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class SP2013WorkflowSubscriptionModels
    {
        public static class TestDocumentLibrary
        {
            public static SP2013WorkflowSubscriptionDefinition WriteToHistoryList = new SP2013WorkflowSubscriptionDefinition
            {
                Name = ListModels.TestLibrary.Title + " - " + SP2013WorkflowModels.WriteToHistoryList.DisplayName,
                WorkflowDisplayName = SP2013WorkflowModels.WriteToHistoryList.DisplayName,
                TaskListUrl = "/WorkflowTasks",
                HistoryListUrl = "/Lists/Workflow History"
            };
        }
    }
}
