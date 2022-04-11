using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services.Impl;
using System;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultTryRetryServiceTests
    {
        #region constructors

        public DefaultTryRetryServiceTests()
        {
            Rnd = new DefaultRandomService();
        }

        #endregion

        #region properties

        public RandomService Rnd { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.DefaultTryRetryService")]
        [TestCategory("CI.Core")]
        public void CanCreate_DefaultTryRetryService()
        {
            var service = new DefaultTryRetryService();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultTryRetryService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_OnMoreAttempts()
        {
            var service = new DefaultTryRetryService();
            var currentTryCount = 0;

            ExpectException<SPMeta2Exception>(() =>
            {
                service.TryWithRetry(() =>
                {
                    currentTryCount++;
                    return false;
                });
            });

            Assert.AreEqual(service.MaxRetryCount, currentTryCount);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultTryRetryService")]
        [TestCategory("CI.Core")]
        public void ShouldWork_On_FirstAttempt()
        {
            var service = new DefaultTryRetryService();

            var currentTryCount = 0;
            var maxTryCount = Rnd.Int(5) + 1;

            service.TryWithRetry(() =>
            {
                currentTryCount++;
                return true;
            }, maxTryCount, 50);

            Assert.AreEqual(1, currentTryCount);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultTryRetryService")]
        [TestCategory("CI.Core")]
        public void ShouldWork_On_RandomAttempt()
        {
            var service = new DefaultTryRetryService();

            var currentTryCount = 0;

            var maxTryCount = Rnd.Int(5) + 2;
            var positiveTryCount = maxTryCount / 2 + 1;

            service.TryWithRetry(() =>
            {
                currentTryCount++;

                if (positiveTryCount == currentTryCount)
                    return true;

                return false;

            }, maxTryCount, 50);

            Assert.AreEqual(positiveTryCount, currentTryCount);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultTryRetryService")]
        [TestCategory("CI.Core")]
        public void CanOverride_MaxTryAttempts()
        {
            var service = new DefaultTryRetryService();

            var currentTryCount = 0;
            var maxTryCount = Rnd.Int(5) + 1;

            ExpectException<SPMeta2Exception>(() =>
            {
                service.TryWithRetry(() =>
                {
                    currentTryCount++;
                    return false;
                }, maxTryCount, 50);
            });

            Assert.AreEqual(maxTryCount, currentTryCount);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultTryRetryService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_OnWrongParameters()
        {
            var service = new DefaultTryRetryService();

            ExpectException<ArgumentNullException>(() =>
            {
                service.TryWithRetry(null, 1, 50);
            });

            ExpectException<ArgumentNullException>(() =>
            {
                service.TryWithRetry(() =>
                {
                    return true;
                }, 1, 50, null);
            });

            ExpectException<ArgumentOutOfRangeException>(() =>
            {
                service.TryWithRetry(() =>
                {
                    return false;
                }, -1, 50);
            });

            ExpectException<ArgumentOutOfRangeException>(() =>
            {
                service.TryWithRetry(() =>
                {
                    return false;
                }, 1, -50);
            });
        }

        protected void ExpectException<TException>(Action action)
            where TException : Exception
        {
            var hasValidException = false;
            var expectedExceptionType = typeof(TException);

            try
            {
                action();
            }
            catch (Exception e)
            {
                hasValidException = e.GetType() == expectedExceptionType;
            }

            Assert.IsTrue(hasValidException);
        }

        #endregion
    }
}
