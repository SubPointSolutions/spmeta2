using SPMeta2.Regression.Assertion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Validation.Services
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
