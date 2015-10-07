using System;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Use by validation to ensure minal set of the properties.
    /// </summary>
    public class ExpectRequired : Attribute
    {
        public string GroupName { get; set; }
        public Type GroupType { get; set; }
    }


}
