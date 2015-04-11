using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions.Base
{
    [DataContract]
    /// <summary>
    /// Base definition for farm and sandbox solution definitions.
    /// </summary>
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
        public Guid SolutionId { get; set; }

        #endregion
    }
}
