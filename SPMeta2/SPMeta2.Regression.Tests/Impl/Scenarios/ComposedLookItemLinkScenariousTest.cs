using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;
using System.IO;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ComposedLookItemLinkScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region default


        [TestMethod]
        [TestCategory("Regression.Scenarios.ComposedLookItemLink")]
        public void CanDeploy_OOTB_ComposedLooks()
        {
            var types = typeof(BuiltInComposedLookItemNames);
            var values = types.GetFields()
                              .Select(p => p.GetValue(null) as string)
                              .OrderBy(s => s);

            foreach (var composedLookItemName in values)
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    // root web
                    web.AddComposedLookItemLink(new ComposedLookItemLinkDefinition
                    {
                        ComposedLookItemName = composedLookItemName
                    });
                    
                    // sun web
                    web.AddRandomWeb(randomWeb =>
                    {
                        randomWeb.AddComposedLookItemLink(new ComposedLookItemLinkDefinition
                        {
                            ComposedLookItemName = composedLookItemName
                        });
                    });
                });

                TestModel(model);
            }
        }


        #endregion
    }
}
