using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace SPMeta2.Services
{
    public abstract class TryRetryServiceBase
    {
        #region properties

        #endregion

        #region methods

        public abstract void TryWithRetry(Func<bool> action);
        public abstract void TryWithRetry(Func<bool> action, int maxTryCount);
        public abstract void TryWithRetry(Func<bool> action, int maxTryCount, int retryTimeoutInMilliseconds);
        public abstract void TryWithRetry(Func<bool> action, int maxTryCount, int retryTimeoutInMilliseconds, Action<int, int, int> waiter);

        #endregion
    }
}
