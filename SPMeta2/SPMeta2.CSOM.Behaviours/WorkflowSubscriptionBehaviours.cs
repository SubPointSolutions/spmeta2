using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace SPMeta2.CSOM.Behaviours
{
    public static class WorkflowSubscriptionBehaviours
    {
        #region common extensions

        public static WorkflowSubscription MakeHistoryListId(this WorkflowSubscription workflowSubscription, Guid historyListId)
        {
            workflowSubscription.SetProperty("HistoryListId", historyListId.ToString());
            return workflowSubscription;
        }

        public static WorkflowSubscription MakeTaskListId(this WorkflowSubscription workflowSubscription, Guid taskListId)
        {
            workflowSubscription.SetProperty("TaskListId", taskListId.ToString());
            return workflowSubscription;
        }

        public static WorkflowSubscription MakeListId(this WorkflowSubscription workflowSubscription, Guid listId)
        {
            workflowSubscription.SetProperty("ListId", listId.ToString());
            workflowSubscription.SetProperty("Microsoft.SharePoint.ActivationProperties.ListId", listId.ToString());

            return workflowSubscription;
        }


        #endregion
    }
}
