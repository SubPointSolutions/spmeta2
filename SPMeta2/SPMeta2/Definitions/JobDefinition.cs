using System.Collections.ObjectModel;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

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
        public string Name { get; set; }

        [ExpectValidation]
        public string Title { get; set; }

        /// <summary>
        /// Type of the target timer job.
        /// </summary>
        /// 

        [ExpectValidation]
        public string JobType { get; set; }

        /// <summary>
        /// Schedule string for the target timer job.
        /// 
        /// SPSchedule.FromString is used to get instance of the target Schedule object.
        /// </summary>
        /// 

        [ExpectValidation]
        public string ScheduleString { get; set; }

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

    public enum JobDefinitionCtorParams
    {
        WebApplication,
        JobName
    }
}
