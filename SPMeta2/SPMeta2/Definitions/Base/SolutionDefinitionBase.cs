using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Base
{
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
        public string FileName { get; set; }

        /// <summary>
        /// Target sandbox solution content.
        /// </summary>
        [ExpectValidation]
        public byte[] Content { get; set; }

        /// <summary>
        /// Target ID of the solution.
        /// </summary>
        [ExpectValidation]
        public Guid SolutionId { get; set; }

        #endregion
    }
}
