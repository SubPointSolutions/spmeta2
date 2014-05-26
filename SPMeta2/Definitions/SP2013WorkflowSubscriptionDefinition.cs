using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SPMeta2.Enumerations;

namespace SPMeta2.Definitions
{
    public class SP2013WorkflowSubscriptionDefinition : DefinitionBase
    {
        #region contructors

        public SP2013WorkflowSubscriptionDefinition()
        {
            EventTypes = new Collection<string> { BuiltInSP2013WorkflowEventTypes.WorkflowStart };
        }

        #endregion

        #region properties

        public string Name { get; set; }

        public string WorkflowDisplayName { get; set; }

        public string HistoryListUrl { get; set; }
        public string TaskListUrl { get; set; }

        public Guid EventSourceId { get; set; }

        public Collection<string> EventTypes { get; set; }

        #endregion
    }
}
