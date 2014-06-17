using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Models;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.Tests.Common;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionEventsTestBase : SPMeta2RegresionTestBase
    {
        protected virtual void AssertEvents<TObj>(ModelNode modelNode, EventHits hits)
        {
            modelNode.OnProvisioning<TObj>(context =>
            {
                hits.OnProvisioning = true;

                Assert.IsNotNull(context.ObjectDefinition);
            });

            modelNode.OnProvisioned<TObj>(context =>
            {
                hits.OnProvisioned = true;

                Assert.IsNotNull(context.Object);
                Assert.IsNotNull(context.ObjectDefinition);

                Assert.IsInstanceOfType(context.Object, typeof(TObj));
            });
        }

        protected virtual void WithEventHits(Action<EventHits> action)
        {
            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validating events..."));

                var eventHits = new EventHits();

                action(eventHits);

                traceScope.WriteLine(string.Format("Validating OnProvisioning event hit."));
                Assert.AreEqual(true, eventHits.OnProvisioning);

                traceScope.WriteLine(string.Format("Validating OnProvisioned event hit."));
                Assert.AreEqual(true, eventHits.OnProvisioned);
            });
        }
    }
}
