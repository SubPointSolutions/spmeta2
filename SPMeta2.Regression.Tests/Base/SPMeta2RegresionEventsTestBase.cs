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
        #region contructors

        public SPMeta2RegresionEventsTestBase()
        {
            EnableDefinitionValidation = false;
        }

        #endregion

        protected virtual void AssertEventHooks<TObj>(ModelNode modelNode, EventHooks hooks)
        {
            modelNode.OnProvisioning<TObj>(context =>
            {
                hooks.OnProvisioning = true;

                Assert.IsNotNull(context.ObjectDefinition);
            });

            modelNode.OnProvisioned<TObj>(context =>
            {
                hooks.OnProvisioned = true;

                Assert.IsNotNull(context.Object);
                Assert.IsNotNull(context.ObjectDefinition);

                Assert.IsInstanceOfType(context.Object, typeof(TObj));
            });
        }

        protected virtual void WithEventHooks(Action<EventHooks> hooks)
        {
            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validating events..."));

                var eventHooks = new EventHooks();

                hooks(eventHooks);

                traceScope.WriteLine(string.Format("Validating OnProvisioning event hit."));
                Assert.AreEqual(true, eventHooks.OnProvisioning);

                traceScope.WriteLine(string.Format("Validating OnProvisioned event hit."));
                Assert.AreEqual(true, eventHooks.OnProvisioned);
            });
        }
    }
}
