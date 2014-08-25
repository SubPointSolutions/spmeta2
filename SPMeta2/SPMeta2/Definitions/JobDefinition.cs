using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint timer job.
    /// </summary>
    public class JobDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target timer job.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the target timer job.
        /// </summary>
        public string JobType { get; set; }

        #endregion
    }
}
