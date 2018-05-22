using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint 2013 workflow to the target list or library.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.WorkflowServices.WorkflowSubscription", "Microsoft.SharePoint.WorkflowServicesBase")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WorkflowServices.WorkflowSubscription", "Microsoft.SharePoint.Client.WorkflowServices")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class SP2013WorkflowSubscriptionDefinition : DefinitionBase
    {
        #region constructors

        public SP2013WorkflowSubscriptionDefinition()
        {
            EventTypes = new Collection<string> { BuiltInSP2013WorkflowEventTypes.WorkflowStart };
            Properties = new List<SP2013WorkflowProperty>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target SharePoint 2013 workflow binding in the list or library.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        /// <summary>
        /// Display name of the target SharePoint 2013 workflow to be bindind to the list.
        /// WorkflowDisplayName is used to lookup workflow from the current web and bind it to the list. 
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string WorkflowDisplayName { get; set; }

        /// <summary>
        /// URL of the workflow history list for the target workflow.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string HistoryListUrl { get; set; }

        /// <summary>
        /// URL of the workflow task list for the target workflow. 
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string TaskListUrl { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// 
        //[ExpectValidation]
        //public Guid EventSourceId { get; set; }

        /// <summary>
        /// Set of the events to trigger on.
        /// BuiltInSP2013WorkflowEventTypes class can be used to utilize out of the box SharePoint events.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public Collection<string> EventTypes { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<SP2013WorkflowProperty> Properties { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("WorkflowDisplayName", WorkflowDisplayName)
                          .AddRawPropertyValue("HistoryListUrl", HistoryListUrl)
                          .AddRawPropertyValue("TaskListUrl", TaskListUrl)
                          .AddRawPropertyValue("EventTypes", EventTypes)

                          .ToString();
        }

        #endregion
    }
}
