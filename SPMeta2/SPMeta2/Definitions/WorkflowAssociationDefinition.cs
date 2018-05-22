using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Workflow.SPWorkflowAssociation", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Workflow.WorkflowAssociation", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]


    //[ParentHostCapability(typeof(SiteDefinition))]
    //[ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]
    public class WorkflowAssociationDefinition : DefinitionBase
    {
        #region properties

        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        [ExpectValidation]
        public string Name { get; set; }

        [DataMember]
        [ExpectValidation]

        public string Description { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? Enabled { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectRequired]
        public string HistoryListTitle { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectRequired]
        public string TaskListTitle { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? AllowManual { get; set; }

        [DataMember]
        [ExpectValidation]
        public string AssociationData { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? AutoStartChange { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? AutoStartCreate { get; set; }

        [DataMember]
        [ExpectValidation]
        [ExpectRequired]
        public string WorkflowTemplateName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("WorkflowTemplateName", WorkflowTemplateName)
                          .AddRawPropertyValue("TaskListTitle", TaskListTitle)
                          .AddRawPropertyValue("HistoryListTitle", HistoryListTitle)
                          .ToString();
        }

        #endregion
    }
}
