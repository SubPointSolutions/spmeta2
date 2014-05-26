using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.MinimalModelTests.Base;

namespace SPMeta2.Regression.MinimalModelTests
{
    [TestClass]
    public class SerializationTest : MinimalModelTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanSaveToXml()
        {
            var model = ModelBuilder.GetSiteWithDefaultFields();

            var definitionClasses = AppDomain.CurrentDomain
                                             .GetAssemblies()
                                             .SelectMany(a => a.GetTypes()
                                                               .Where(type => type.IsSubclassOf(typeof(DefinitionBase)) ||
                                                                              type.IsSubclassOf(typeof(ModelNode))));

            var xml = XmlSerializerUtils.SerializeToString(model, definitionClasses);
            XmlSerializerUtils.DeserializeFromString<ModelNode>(xml, definitionClasses);
        }


        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanSaveToJson()
        {
            var model = ModelBuilder.GetSiteWithDefaultFieldsAndFilledContentTypes();
            var json = JsonSerializerUtils.SerializeToJsonString(model);
        }

        #endregion
    }
}
