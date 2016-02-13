using System;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing to ensure that random generated model will have several random instances of the definition.
    /// </summary>
    public class ExpectManyInstances : Attribute
    {
        #region constructors

        public ExpectManyInstances()
        {
            MinInstancesCount = 2;
            MaxInstancesCount = 3;
        }

        #endregion

        #region properties

        public int MinInstancesCount { get; set; }
        public int MaxInstancesCount { get; set; }

        #endregion
    }
}
