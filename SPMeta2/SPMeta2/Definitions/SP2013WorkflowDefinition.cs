using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{

    [Serializable]
    [DataContract]
    public class SP2013WorkflowProperty
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// Allows to define and deploy SharePoint 2013 workflow.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.WorkflowServices.WorkflowDefinition", "Microsoft.SharePoint.WorkflowServicesBase")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition", "Microsoft.SharePoint.Client.WorkflowServices")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebDefinition))]

    [ExpectManyInstances]

    public class SP2013WorkflowDefinition : DefinitionBase
    {
        #region constructors

        public SP2013WorkflowDefinition()
        {
            Override = false;

            Properties = new List<SP2013WorkflowProperty>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Display name of the target SharePoint 2013 workflow.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string DisplayName { get; set; }

        /// <summary>
        /// XAML content of the target SharePoint 2013 workflow.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Xaml { get; set; }

        /// <summary>
        /// Should target workflow be overwritten.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public bool Override { get; set; }

        [ExpectValidation]
        [DataMember]
        public string RestrictToType { get; set; }

        [ExpectValidation]
        [DataMember]
        public string RestrictToScope { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<SP2013WorkflowProperty> Properties { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("DisplayName", DisplayName)
                          .AddRawPropertyValue("Override", Override)

                          .ToString();
        }

        #endregion
    }
}
