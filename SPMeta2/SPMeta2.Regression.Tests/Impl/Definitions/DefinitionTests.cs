using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class DefinitionTests
    {
        #region properties

        protected static List<Type> DefinitionTypes = new List<Type>();

        #endregion

        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            LoadDefinitions();
        }

        private static void LoadDefinitions()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            DefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(spMetaAssembly));
            DefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(spMetaStandardAssembly));
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {

        }

        #endregion

        #region properties



        #endregion

        #region base tests

        [TestMethod]
        [TestCategory("Regression.Definitions")]
        public void DefinitionsShouldHaveToStringOverride()
        {
            foreach (var definitionType in DefinitionTypes)
            {
                var hasToStringOverride = definitionType.GetMethod("ToString").DeclaringType == definitionType;

                if (!hasToStringOverride)
                    Trace.WriteLine(string.Format("Checking definition type:[{0}]. Has override:[{1}]", definitionType, hasToStringOverride));

                Assert.IsTrue(hasToStringOverride);
            }
        }

        #endregion
    }
}
