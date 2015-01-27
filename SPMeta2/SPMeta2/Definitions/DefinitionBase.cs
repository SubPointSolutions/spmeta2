using SPMeta2.Common;
using System;
using System.Collections.Generic;

namespace SPMeta2.Definitions
{
    public class PropertyBagValue : KeyNameValue
    {
        public PropertyBagValue()
        {

        }

        public PropertyBagValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

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
            PropertyBag = new List<PropertyBagValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Internal usage only. Will be removed in future versions of SPMeta2 library.
        /// </summary>
        [Obsolete("Please use AddHostXXX syntax to setup RequireSelfProcessing on the particular model node. RequireSelfProcessing property will be removed from the future releases of SPMeta2 library.")]
        public virtual bool RequireSelfProcessing { get; set; }

        /// <summary>
        /// A property bag to be used for any 'custom' properties attached to definition.
        /// </summary>
        public List<PropertyBagValue> PropertyBag { get; set; }

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
