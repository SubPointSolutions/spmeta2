using System;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Use by validation to allow integer range of allowed propeties.
    /// </summary>
    public class ExpectRequiredIntRange : Attribute
    {
        #region properties

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        #endregion
    }
}
