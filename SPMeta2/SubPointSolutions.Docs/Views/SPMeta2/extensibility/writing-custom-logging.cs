using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Linq;
using System.Collections.Generic;
using SPMeta2.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Common;
using SPMeta2.Services;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    public class CustomLoggingService : TraceServiceBase
    {

        public override void Critical(int id, object message, Exception exception)
        {
            // handle critical event the way you like
        }

        public override void Error(int id, object message, Exception exception)
        {
            // handle error the way you like
        }

        public override void Warning(int id, object message, Exception exception)
        {
            // handle warning the way you like
        }

        public override void Information(int id, object message, Exception exception)
        {
            // handle info the way you like
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            // handle verbose the way you like
        }

        public override void TraceActivityStart(int id, object message)
        {
            // no implementation is required
        }

        public override void TraceActivityStop(int id, object message)
        {
            // no implementation is required
        }

        public override void TraceActivityTransfer(int id, object message, Guid relatedActivityId)
        {
            // no implementation is required
        }

        public override Guid CurrentActivityId
        {
            get { return Guid.Empty; }
            set
            {

            }
        }
    }

    [TestClass]
    public class CustomLogginRegistration : ProvisionTestBase
    {
        [TestMethod]
        [TestCategory("Docs.Models")]
        public void RegisterCustomLogginService()
        {
            // get an instance of ServiceContainer
            // override .Services collection with the right mapping 
            // typeof(TraceServiceBase) -> your instance of TraceServiceBase class

            var serviceInstancies = ServiceContainer.Instance.Services[typeof(TraceServiceBase)];

            serviceInstancies.Clear();
            serviceInstancies.Add(new CustomLoggingService());

            // provision models as usual
            // from now on, all logging calls will go to CustomLoggingService instance
        }

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void GetDefaultTraceServiceInstance()
        {
            // get an instance of ServiceContainer
            // then get an instance of required service such as TraceServiceBase

            var traceService = ServiceContainer.Instance.GetService<TraceServiceBase>();

            // use trace methods as usual
            traceService.Verbose(0, "my verbose message");
        }
    }

}