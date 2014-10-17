using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Base definition for pages.
    /// </summary>
    /// 
    [Serializable]
    public abstract class PageDefinitionBase : DefinitionBase
    {
        #region constructors

        public PageDefinitionBase()
        {
            NeedOverride = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target page.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Title { get; set; }

        /// <summary>
        /// File name of the target page.
        /// </summary>
        /// 
        [ExpectValidation]
        public string FileName { get; set; }

        /// <summary>
        /// Should page be overwritten during provision.
        /// </summary>
        /// 

        public bool NeedOverride { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] FileName:[{1}]", Title, FileName);
        }

        #endregion
    }
}
