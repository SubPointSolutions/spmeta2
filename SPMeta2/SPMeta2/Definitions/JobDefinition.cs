using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Collections.Generic;

namespace SPMeta2.Definitions
{
    [Serializable]
    [DataContract]
    public class JobDefinitionProperty
    {
        [DataMember]
        public object Key { get; set; }

        [DataMember]
        public object Value { get; set; }
    }

    /// <summary>
    /// Allows to define and deploy SharePoint timer job.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPJobDefinition", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(WebApplicationDefinition))]

    [ExpectManyInstances]
    public class JobDefinition : DefinitionBase
    {
        #region constructors

        public JobDefinition()
        {
            ConstructorParams = new Collection<JobDefinitionCtorParams>();
            Properties = new List<JobDefinitionProperty>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target timer job.
        /// </summary>
        /// 

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [ExpectValidation]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Type of the target timer job.
        /// </summary>
        /// 

        [ExpectValidation]
        [DataMember]
        public string JobType { get; set; }

        /// <summary>
        /// Schedule string for the target timer job.
        /// 
        /// SPSchedule.FromString is used to get instance of the target Schedule object.
        /// </summary>
        /// 

        [ExpectValidation]
        [DataMember]
        public string ScheduleString { get; set; }

        [DataMember]
        public Collection<JobDefinitionCtorParams> ConstructorParams { get; set; }

        /// <summary>
        /// Corresponds to SPJobDefinition.Properties
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public List<JobDefinitionProperty> Properties { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("JobType", JobType)
                          .AddRawPropertyValue("ScheduleString", ScheduleString)
                          .ToString();
        }

        #endregion
    }

    [DataContract]
    [Serializable]

    public enum JobDefinitionCtorParams
    {
        [EnumMember]
        WebApplication,
        [EnumMember]
        JobName
    }
}
