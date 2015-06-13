using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Utils;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.RegressionAPI
{
    [TestClass]
    public class RegressionAPITests : SPMeta2RegresionTestBase
    {
        public RegressionAPITests()
        {

        }

        #region common

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        [TestMethod]
        [TestCategory("Regression.RegressionAPI")]
        public void ExpectUpdateAttributes_ShouldHave_Services()
        {
            var updateAttrTypes = ReflectionUtils.GetTypesFromAssembly<ExpectUpdate>(typeof(ExpectUpdate).Assembly);

            var updateAttrServices = new List<ExpectUpdateValueServiceBase>();
            updateAttrServices.AddRange(ReflectionUtils.GetTypesFromAssembly<ExpectUpdateValueServiceBase>(typeof(ExpectUpdateValueServiceBase).Assembly)
                                                         .Select(t => Activator.CreateInstance(t) as ExpectUpdateValueServiceBase));

            TraceUtils.WithScope(trace =>
            {
                foreach (var attr in updateAttrTypes)
                {
                    var targetServices = updateAttrServices.FirstOrDefault(s => s.TargetType == attr);

                    if (targetServices != null)
                    {
                        trace.WriteLine(string.Format("ExpectUpdate: [{0}] has service: [{1}]", attr,
                            targetServices.GetType()));
                    }
                    else
                    {
                        trace.WriteLine(string.Format("ExpectUpdate: [{0}] misses  service impl", attr));
                        Assert.Fail();
                    }
                }
            });

        }

        #endregion
    }
}
