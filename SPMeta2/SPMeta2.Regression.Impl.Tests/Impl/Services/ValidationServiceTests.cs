using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.Services;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{
    [TestClass]
    public class ValidationServiceTests
    {
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

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.ValidationServices")]
        [TestCategory("CI.Core")]
        public void All_Services_Should_Have_Metadata()
        {
            var isValid = true;

            var validationServiceTypes =
                ReflectionUtils.GetTypesFromAssembly<PreDeploymentValidationServiceBase>(
                        typeof(FieldDefinition).Assembly);


            foreach (var validationServiceType in validationServiceTypes)
            {
                var service = Activator.CreateInstance(validationServiceType) as PreDeploymentValidationServiceBase;

                Trace.WriteLine(service.Title);
                Trace.WriteLine(service.Description);

                Assert.IsNotNull(service);

                if (string.IsNullOrEmpty(service.Title))
                {
                    isValid = false;
                    break;
                }

                if (string.IsNullOrEmpty(service.Description))
                {
                    isValid = false;
                    break;
                }
            }

            Assert.IsTrue(isValid);
        }

        #endregion
    }
}
