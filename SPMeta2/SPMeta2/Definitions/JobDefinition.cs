using System.Collections.ObjectModel;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint timer job.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPJobDefinition", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(WebApplicationDefinition))]
    [DefaultParentHostAttribute(typeof(WebApplicationDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    public class JobDefinition : DefinitionBase
    {
        #region constructors

        public JobDefinition()
        {
            ConstructorParams = new Collection<JobDefinitionCtorParams>();
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

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<JobDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.JobType)
                          .AddPropertyValue(p => p.ScheduleString)
                          .ToString();
        }

        #endregion
    }

    [DataContract]

    public enum JobDefinitionCtorParams
    {
        WebApplication,
        JobName
    }
}
