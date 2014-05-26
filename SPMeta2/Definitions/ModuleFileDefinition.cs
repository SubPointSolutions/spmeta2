using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public class ModuleFileDefinition : DefinitionBase
    {
        #region contructors

        public ModuleFileDefinition()
        {
            Content = new byte[0];
        }

        #endregion

        #region properties

        public string FileName { get; set; }
        public byte[] Content { get; set; }

        #endregion
    }
}
