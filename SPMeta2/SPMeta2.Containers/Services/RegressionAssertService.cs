using System;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Containers.Services
{
    public class RegressionAssertService
    {
        #region method

        public AssertPair<TSrc, TDst> NewAssert<TSrc, TDst>(TSrc src, TDst dst)
        {
            return NewAssert(src, src, dst);
        }

        public AssertPair<TSrc, TDst> NewAssert<TSrc, TDst>(object tag, TSrc src, TDst dst)
        {
            var assert = new AssertPair<TSrc, TDst>(src, dst)
            {
                Tag = tag == null ? src : tag
            };

            assert.OnPropertyValidated += InvokeOnPropertyValidated;

            return assert;
        }

        private void InvokeOnPropertyValidated(object sender, OnPropertyValidatedEventArgs e)
        {
            if (OnPropertyValidated != null)
                OnPropertyValidated(sender, e);
        }

        #endregion

        public static EventHandler<OnPropertyValidatedEventArgs> OnPropertyValidated;
    }
}
