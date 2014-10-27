using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

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

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ModuleFileDefinition>(this)
                          .AddPropertyValue(p => p.FileName)
                          .AddPropertyValue(p => p.Overwrite)

                          .ToString();
        }

        #endregion
    }
}
