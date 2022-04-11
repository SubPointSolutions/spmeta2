using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Base
{

    /// <summary>
    /// Base definition for farm and sandbox solution definitions.
    /// </summary>
    [DataContract]
    public abstract class SolutionDefinitionBase : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Target solutions file name.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Target sandbox solution content.
        /// </summary>
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public byte[] Content { get; set; }

        /// <summary>
        /// Target ID of the solution.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        public Guid SolutionId { get; set; }

        #endregion
    }
}
