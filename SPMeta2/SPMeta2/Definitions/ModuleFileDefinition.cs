using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy module file.
    /// </summary>

    public class ModuleFileDefinition : DefinitionBase
    {
        #region contructors

        public ModuleFileDefinition()
        {
            Content = new byte[0];
        }

        #endregion

        #region properties

        /// <summary>
        /// Target file name,
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Target file content.
        /// </summary>
        public byte[] Content { get; set; }

        #endregion
    }
}
