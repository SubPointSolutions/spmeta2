using SPMeta2.Exceptions;
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
            TryWithRetry(action, MaxRetryCount);
        }

        public override void TryWithRetry(Func<bool> action, int maxTryCount)
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
            ValidateParameters(action, maxTryCount, retryTimeoutInMilliseconds, waiter);

            var currentTryIndex = 1;
            var actionResult = action();

            while (actionResult != true)
            {
                TraceService.Warning((int)LogEventId.ModelProvision,
                    string.Format("Coudn't perform action. Waiting and retrying [{1}/{2}]",
                            retryTimeoutInMilliseconds,
                            currentTryIndex,
                            MaxRetryCount));

                if (currentTryIndex >= maxTryCount)
                    break;

                waiter(currentTryIndex, maxTryCount, retryTimeoutInMilliseconds);
                currentTryIndex++;

                actionResult = action();
            }

            if (actionResult != true)
            {
                throw new SPMeta2Exception(string.Format(
                    "Error while performing requested action. Tried [{0}] out of [{1}], raising exception",
                    currentTryIndex, maxTryCount));
            }
        }

        private static void ValidateParameters(Func<bool> action, int maxTryCount, int retryTimeoutInMilliseconds, Action<int, int, int> waiter)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (waiter == null)
                throw new ArgumentNullException("waiter");

            if (maxTryCount <= 0)
                throw new ArgumentOutOfRangeException("maxTryCount must be greater than 0");

            if (retryTimeoutInMilliseconds <= 0)
                throw new ArgumentOutOfRangeException("retryTimeoutInMilliseconds must be greater than 0");
        }
    }
}
