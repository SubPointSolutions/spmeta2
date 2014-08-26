using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint sandbox solution.
    /// </summary>
    public class SandboxSolutionDefinition : DefinitionBase
    {
        #region constructors

        public SandboxSolutionDefinition()
        {
            Content = new byte[0];
        }

        #endregion

        #region properties

        /// <summary>
        /// Target sandbox solutions file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Target sandbox solution content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Should the solution be activated.
        /// </summary>
        public bool Activate { get; set; }

        #endregion
    }
}
