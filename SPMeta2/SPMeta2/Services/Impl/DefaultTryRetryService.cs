using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace SPMeta2.Services.Impl
{
    public class DefaultTryRetryService : TryRetryServiceBase
    {
        #region constructors

        public DefaultTryRetryService()
        {
            MaxRetryCount = 10;
            RetryTimeoutInMilliseconds = 1000;

            DefaultRetryWaiter = (currentRetryIndex, maxRetryIndex, defaultTimeoutInMilliseconds) =>
            {
                Thread.Sleep(defaultTimeoutInMilliseconds);
            };
        }

        #endregion

        #region properties

        public int MaxRetryCount { get; set; }
        public int RetryTimeoutInMilliseconds { get; set; }

        public Action<int, int, int> DefaultRetryWaiter { get; set; }
        protected TraceServiceBase TraceService
        {
            get
            {
                return ServiceContainer.Instance.GetService<TraceServiceBase>();
            }
        }

        #endregion

        public override void TryWithRetry(Func<bool> action)
        {
            TryWithRetry(action, MaxRetryCount, RetryTimeoutInMilliseconds);
        }

        public override void TryWithRetry(Func<bool> action, int maxTryCount, int retryTimeoutInMilliseconds)
        {
            TryWithRetry(action, maxTryCount, retryTimeoutInMilliseconds, this.DefaultRetryWaiter);
        }

        public override void TryWithRetry(Func<bool> action,
            int maxTryCount, int retryTimeoutInMilliseconds,
            Action<int, int, int> waiter)
        {
            var currentTryIndex = 0;

            while (action() != true)
            {
                TraceService.Warning((int)LogEventId.ModelProvision,
                    string.Format("Coudn't perform action. Waiting and retrying [{1}/{2}]",
                            retryTimeoutInMilliseconds,
                            currentTryIndex,
                            MaxRetryCount));

                if (currentTryIndex > maxTryCount)
                    break;

                waiter(currentTryIndex, maxTryCount, retryTimeoutInMilliseconds);
                currentTryIndex++;
            }
        }
    }
}
