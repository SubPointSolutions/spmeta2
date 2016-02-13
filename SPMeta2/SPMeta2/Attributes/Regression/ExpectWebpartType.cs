using System;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing to ensure that the target web part has an expected class type.
    /// </summary>
    public class ExpectWebpartType : Attribute
    {
        public string WebPartType { get; set; }
    }
}
