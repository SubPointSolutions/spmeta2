using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SPMeta2.Enumerations;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint 2013 workflow to the target list or library.
    /// </summary>
    public class SP2013WorkflowSubscriptionDefinition : DefinitionBase
    {
        #region contructors

        public SP2013WorkflowSubscriptionDefinition()
        {
            EventTypes = new Collection<string> { BuiltInSP2013WorkflowEventTypes.WorkflowStart };
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target SharePoint 2013 workflow.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the target SharePoint 2013 workflow in the list or library.
        /// </summary>
        public string WorkflowDisplayName { get; set; }

        /// <summary>
        /// URL of the workflow history list for the target workflow.
        /// </summary>
        public string HistoryListUrl { get; set; }

        /// <summary>
        /// URL of the workflow task list for the target workflow. 
        /// </summary>
        public string TaskListUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid EventSourceId { get; set; }

        /// <summary>
        /// Set of the events to trigger on.
        /// BuiltInSP2013WorkflowEventTypes class can be used to utilize out of the box SharePoint events.
        /// </summary>
        public Collection<string> EventTypes { get; set; }

        #endregion
    }
}
