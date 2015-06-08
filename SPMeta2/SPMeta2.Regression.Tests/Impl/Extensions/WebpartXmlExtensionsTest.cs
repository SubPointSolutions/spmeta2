using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.Tests.Consts;
using SPMeta2.Regression.Tests.Utils;
using SPMeta2.Standard.Definitions.Webparts;
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



        #endregion

        #region loading API

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void LoadDefinitionFromV2WebpartFile()
        {
            var webpartXml = ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V2.NewsFeed);

            var def = WebpartXmlExtensions.LoadDefinitionFromWebpartFile<SiteFeedWebPartDefinition>(webpartXml);
            Assert.IsNotNull(def);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void LoadDefinitionFromV3WebpartFile()
        {
            var webpartXml = ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V3.TeamTasks);

            var def = WebpartXmlExtensions.LoadDefinitionFromWebpartFile<XsltListViewWebPartDefinition>(webpartXml);
            Assert.IsNotNull(def);
        }

        #endregion

        #region base tests

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanLoadWebpartDefinitionV2()
        {
            var def = WebpartXmlExtensions
                            .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V2.NewsFeed))
                            .ToString();

        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanLoadWebpartDefinitionV3()
        {
            var def = WebpartXmlExtensions
                            .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V3.TeamTasks))
                            .ToString();
        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanSetV2PlainProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V2.NewsFeed))
                                .SetOrUpdateProperty(propName, propValue)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanSetV3PlainProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V3.TeamTasks))
                                .SetOrUpdateProperty(propName, propValue)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanSetV2CDataProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V2.NewsFeed))
                                .SetOrUpdateProperty(propName, propValue, true)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanSetCDataV3PlainProperty()
        {
            var propName = string.Format("prop_{0}", Guid.NewGuid().ToString("N"));
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.V3.TeamTasks))
                                .SetOrUpdateProperty(propName, propValue, true)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        #endregion

        #region content editor tests

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanSetContentEditor_PlainProperty()
        {
            var propName = "ContentLink";
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.Base.ContentEditor))
                                .SetOrUpdateContentEditorWebPartProperty(propName, propValue)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetContentEditorWebPartProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }

        [TestMethod]
        [TestCategory("Regression.Extensions.WebpartXmlExtensions")]
        public void CanSetContentEditor_CDataProperty()
        {
            var propName = "Content";
            var propValue = string.Format("{0} {1} {2}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

            var updatedDef = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(ResourceReaderUtils.ReadFromResourceName(GetType().Assembly, RegWebparts.Base.ContentEditor))
                                .SetOrUpdateContentEditorWebPartProperty(propName, propValue, true)
                                .ToString();

            var updatedProp = WebpartXmlExtensions
                                .LoadWebpartXmlDocument(updatedDef)
                                .GetContentEditorWebPartProperty(propName);

            Assert.AreEqual(propValue, updatedProp);
        }


        #endregion
    }
}
