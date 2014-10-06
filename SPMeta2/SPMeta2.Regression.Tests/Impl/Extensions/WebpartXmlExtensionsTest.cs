using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Tests.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Extensions
{
    [TestClass]
    public class WebpartXmlExtensionsTest
    {
        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {

        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {

        }

        #endregion

        #region properties

        public static class Webparts
        {
            public static class V2
            {
                public static string NewsFeed = "SPMeta2.Regression.Tests.Templates.Webparts.v2.News Feed.dwp";
            }

            public static class V3
            {
                public static string TeamTasks = "SPMeta2.Regression.Tests.Templates.Webparts.v3.Team Tasks.webpart";
            }
        }

        #endregion

        #region methods

        [TestMethod]
        [TestCategory("Regression.Extensions")]
        public void CanLoadWebpartDefinitionV2()
        {
            var def = WebpartXmlExtensions
                            .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, Webparts.V2.NewsFeed))
                            .ToString();

        }

        [TestMethod]
        [TestCategory("Regression.Extensions")]
        public void CanLoadWebpartDefinitionV3()
        {
            var def = WebpartXmlExtensions
                            .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, Webparts.V3.TeamTasks))
                            .ToString();
        }

        [TestMethod]
        [TestCategory("Regression.Extensions")]
        public void CanSetV2PlainProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, Webparts.V2.NewsFeed))
                                .SetOrUpdateProperty(propName, propValue)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions")]
        public void CanSetV3PlainProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, Webparts.V3.TeamTasks))
                                .SetOrUpdateProperty(propName, propValue)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions")]
        public void CanSetV2CDataProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, Webparts.V2.NewsFeed))
                                .SetOrUpdateProperty(propName, propValue, true)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions")]
        public void CanSetCDataV3PlainProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, Webparts.V3.TeamTasks))
                                .SetOrUpdateProperty(propName, propValue, true)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        #endregion
    }
}
