using System;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Base definition for all SharePoint artifacts to be defined and deployed.
    /// </summary>
    /// 
    [Serializable]
    public abstract class DefinitionBase : ICloneable
    {
        #region constructors

        protected DefinitionBase()
        {
            RequireSelfProcessing = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Internal usage only. Will be removed in future versions of SPMeta2 library.
        /// </summary>
        [Obsolete("Please use AddHostXXX syntax to setup RequireSelfProcessing on the particular model node. RequireSelfProcessing property will be removed from the future releases of SPMeta2 library.")]
        public virtual bool RequireSelfProcessing { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Create a clone of the current model definition.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
