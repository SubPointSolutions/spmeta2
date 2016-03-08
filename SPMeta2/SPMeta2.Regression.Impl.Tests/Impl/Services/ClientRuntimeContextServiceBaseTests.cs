using System;
using System.Net;
using System.Reflection;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{
    //public class GenericExceptionDefaultClientRuntimeContextService : DefaultClientRuntimeContextService
    //{
    //    protected override void InternalExecuteQuery(ClientRuntimeContext context)
    //    {
    //        throw new Exception("Can't make a ExecuteQuery call");
    //    }
    //}

    [TestClass]
    public class ClientRuntimeContextServiceBaseTests : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public ClientRuntimeContextServiceBaseTests()
        {

        }

        #endregion

        #region init

        [ClassInitialize]
        public static void Init(TestContext context)
        {

        }

        [ClassCleanup]
        public static void Cleanup()
        {

        }

        #endregion

        #region properties

        public ClientRuntimeContextServiceBase Service { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.ClientRuntimeContextServiceBase")]
        [ExpectedException(typeof(SPMeta2NotImplementedException))]
        public void ClientRuntimeContextService_Should_Survive_IISReset()
        {
            // TODO
            // emulate IISReset some day once we don't have anything to do
            throw new SPMeta2NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Impl.ClientRuntimeContextServiceBase")]
        public void Can_Create_ClientRuntimeContextService()
        {
            Assert.IsNotNull(new DefaultClientRuntimeContextService());
        }

        [TestMethod]
        [TestCategory("Regression.Impl.ClientRuntimeContextServiceBase")]
        [ExpectedException(typeof(Exception))]
        public void ClientRuntimeContextService_Should_Rethrow_GenericException()
        {
            // expecting generic exception re-thrown
            var service = new DefaultClientRuntimeContextService();

            service.CustomExecuteQueryHandler += (cnt) =>
            {
                throw new Exception("Can't do anything!");
            };

            service.ExecuteQuery(default(ClientRuntimeContext));
        }

        [TestMethod]
        [TestCategory("Regression.Impl.ClientRuntimeContextServiceBase")]
        public void ClientRuntimeContextService_Should_MakeNAttempts_And_ThrowException()
        {
            // expecting SPMeta2Exception AND currentReconnectCount == ExecuteQueryRetryAttempts
            // tried our best, but failed

            var currentReconnectCount = 0;

            var service = new DefaultClientRuntimeContextService();

            // to run test faster
            service.ExecuteQueryDelayInMilliseconds = 1;

            service.CustomExecuteQueryHandler += (cnt) =>
            {
                currentReconnectCount++;

                var webResponce = Activator.CreateInstance<HttpWebResponse>();

                var statusCodeProp = webResponce.GetType()
                                      .GetField("m_StatusCode", BindingFlags.NonPublic | BindingFlags.Instance);

                statusCodeProp.SetValue(webResponce, 429);

                var ex = new WebException("Can't connect to O365", null,
                    (WebExceptionStatus)402, webResponce);

                throw ex;
            };
            try
            {
                service.ExecuteQuery(default(ClientRuntimeContext));
            }
            catch (Exception e)
            {
                Assert.AreEqual(currentReconnectCount, service.ExecuteQueryRetryAttempts);
                Assert.IsTrue(e is SPMeta2Exception);
            }

        }

        [TestMethod]
        [TestCategory("Regression.Impl.ClientRuntimeContextServiceBase")]
        public void ClientRuntimeContextService_Should_SupportHttpStatuses()
        {
            // expecting SPMeta2Exception on known http statuses

            var currentReconnectCount = 0;
            var service = new DefaultClientRuntimeContextService();

            var rndService = new DefaultRandomService();
            var supportedHttpStatuses = new[]
            {
                429, 503
            };

            // to run test faster
            service.ExecuteQueryDelayInMilliseconds = 1;

            service.CustomExecuteQueryHandler += (cnt) =>
            {
                currentReconnectCount++;

                var webResponce = Activator.CreateInstance<HttpWebResponse>();

                var sttausCode = rndService.RandomFromArray(supportedHttpStatuses);
                SetStatusCode(webResponce, sttausCode);

                var ex = new WebException("Can't connect to O365", null,
                    (WebExceptionStatus)402, webResponce);

                throw ex;
            };
            try
            {
                service.ExecuteQuery(default(ClientRuntimeContext));
            }
            catch (Exception e)
            {
                Assert.AreEqual(currentReconnectCount, service.ExecuteQueryRetryAttempts);
                Assert.IsTrue(e is SPMeta2Exception);
            }

        }

        [TestMethod]
        [TestCategory("Regression.Impl.ClientRuntimeContextServiceBase")]
        [ExpectedException(typeof(WebException))]
        public void ClientRuntimeContextService_Should_ThrowGenericException_On_UnsupportHttpStatuses()
        {
            // expecting web exception on unknown http statuses

            var currentReconnectCount = 0;
            var service = new DefaultClientRuntimeContextService();

            var rndService = new DefaultRandomService();
            var supportedHttpStatuses = new[]
            {
                700, 800, 900
            };

            // to run test faster
            service.ExecuteQueryDelayInMilliseconds = 1;

            service.CustomExecuteQueryHandler += (cnt) =>
            {
                currentReconnectCount++;

                var webResponce = Activator.CreateInstance<HttpWebResponse>();

                var sttausCode = rndService.RandomFromArray(supportedHttpStatuses);
                SetStatusCode(webResponce, sttausCode);

                var ex = new WebException("Can't connect to O365", null,
                    (WebExceptionStatus)402, webResponce);

                throw ex;
            };

            service.ExecuteQuery(default(ClientRuntimeContext));
        }


        #endregion

        #region utils

        private static void SetStatusCode(HttpWebResponse webResponce, int sttausCode)
        {
            var statusCodeProp = webResponce.GetType()
                .GetField("m_StatusCode", BindingFlags.NonPublic | BindingFlags.Instance);

            statusCodeProp.SetValue(webResponce, sttausCode);
        }


        #endregion
    }
}
