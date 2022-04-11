using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Services.Impl;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class ServiceContainerTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.ServiceContainer")]
        [TestCategory("CI.Core")]
        public void ServiceContainer_Should_Have_DefaultServices()
        {
            var serviceContainer = ServiceContainer.Instance;

            Assert.IsNotNull(serviceContainer);

            var serviceTypes = new[]
            {
                typeof(TraceServiceBase),

                typeof(ModelTreeTraverseServiceBase),
                typeof(ModelWeighServiceBase),

                typeof(DefaultJSONSerializationService),
                typeof(DefaultXMLSerializationService),

                typeof(WebPartChromeTypesConvertService),
                typeof(ListViewScopeTypesConvertService),

                typeof(DefinitionRelationshipServiceBase),

                typeof(ModelCompatibilityServiceBase),
                typeof(DefaultDiagnosticInfoService),

                typeof(ModelPrettyPrintServiceBase),
                typeof(ModelDotGraphPrintServiceBase),

                typeof(TryRetryServiceBase),

                typeof(WebPartPageTemplatesServiceBase),
                typeof(HashCodeServiceBase)
            };

            foreach (var serviceType in serviceTypes)
            {
                Assert.IsNotNull(serviceContainer.GetService<TraceServiceBase>(), serviceType.Name);
            }
        }

        #endregion
    }
}
