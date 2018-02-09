using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using System.Collections.Generic;
using SPMeta2.Regression.Utils;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class HashCodeServiceTests : SPMeta2DefinitionRegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.HashCodeService")]
        [TestCategory("CI.Core")]
        public void CanGetHashForAllDefinitions()
        {
            var service = ServiceContainer.Instance.GetService<HashCodeServiceBase>();

            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            var allDefinitions = ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
            {
                spMetaAssembly,
                spMetaStandardAssembly
            });

            var hashes = new Dictionary<string, string>();

            RegressionUtils.WriteLine("Checking hashes for definitions...");

            foreach (var defType in allDefinitions)
            {
                var defInstance = ModelGeneratorService.GetRandomDefinition(defType);
                RegressionUtils.WriteLine(string.Format("Definition:[{0}] - [{1}]", defType, defInstance));

                IndentableTrace.WithScope(trace =>
                {
                    var hash1 = service.GetHashCode(defInstance);
                    var hash2 = service.GetHashCode(defInstance);

                    trace.WriteLine(string.Format("HASH1:[{0}]", hash1));
                    trace.WriteLine(string.Format("HASH1:[{0}]", hash2));

                    // just curious, would it ever blow up on duplicate?
                    hashes.Add(hash1, hash1);

                    Assert.IsNotNull(hash1);
                    Assert.IsNotNull(hash2);

                    Assert.IsTrue(hash1 == hash2);

                });
            }
        }

        #endregion
    }
}
