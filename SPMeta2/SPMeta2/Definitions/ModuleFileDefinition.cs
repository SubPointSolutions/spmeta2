using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy module file.
    /// </summary>

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    public class ModuleFileDefinition : DefinitionBase
    {
        #region constructors

        public ModuleFileDefinition()
        {
            Content = new byte[0];
            Overwrite = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Target file name,
        /// </summary>
        /// 
        [ExpectValidation]
        public string FileName { get; set; }

        /// <summary>
        /// Target file content.
        /// </summary>
        /// 
        [ExpectValidation]
        public byte[] Content { get; set; }

        /// <summary>
        /// Overwrite flag
        /// </summary>
        public bool Overwrite { get; set; }

        #endregion
    }
}
