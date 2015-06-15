using System;

namespace SPMeta2.Attributes.Identity
{
    /// <summary>
    /// Used by M2 infrastructure to identify definition ID property or properties.
    /// </summary>
    public class IdentityKeyAttribute : Attribute
    {
        #region properties

        /// <summary>
        /// Composed key group name
        /// </summary>
        public string GroupName { get; set; }

        #endregion
    }
}
