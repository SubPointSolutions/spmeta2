using System;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Use by validation to check allowed boolean values.
    /// </summary>
    public class ExpectRequiredBoolRange : Attribute
    {
        public ExpectRequiredBoolRange()
        {
            
        }

        public ExpectRequiredBoolRange(bool expectedValue)
        {
            ExpectedValue = expectedValue;
        }

        public bool? ExpectedValue { get; set; }
    }
}
