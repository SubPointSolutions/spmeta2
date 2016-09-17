using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SubPointSolutions.Docs.Views.Views.SPMeta2.reference
{
    [TestClass]
    public class Utils : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.Utils")]
        public void UrlConcatenation()
        {
            // fast on two params
            var smQueryUrl = UrlUtility.CombineUrl("http://goole.com", "?q=spmeta2");

            // a bigger one
            var bgQueryUrl = UrlUtility.CombineUrl(new string[]{ 
                "http://goole.com",
                "?",
                "q=1",
                "&p1=3",
                "&p2=tmp"
            });
        }

        [TestMethod]
        [TestCategory("Docs.Utils")]
        public void ClientWebPartSetup(ClientWebPartDefinition wpModel, string webId)
        {
            var wpXml = WebpartXmlExtensions
                           .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ClientWebPart)
                           .SetOrUpdateProperty("FeatureId", wpModel.FeatureId.ToString())
                           .SetOrUpdateProperty("ProductId", wpModel.ProductId.ToString())
                           .SetOrUpdateProperty("WebPartName", wpModel.WebPartName)
                           .SetOrUpdateProperty("ProductWebId", webId)
                           .ToString();
        }

        [TestMethod]
        [TestCategory("Docs.Utils")]
        public void ContentEditorWebPartSetup(ContentEditorWebPartDefinition typedModel,
            string content, string contentLink)
        {
            var wpXml = WebpartXmlExtensions
                           .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ContentEditorWebPart)
                           .SetOrUpdateContentEditorWebPartProperty("Content", content, true)
                           .SetOrUpdateContentEditorWebPartProperty("ContentLink", contentLink)
                           .ToString();
        }

        [TestMethod]
        [TestCategory("Docs.Utils")]
        public void XsltListViewWebPartSetup(XsltListViewWebPartDefinition typedModel,
            string listName, string listId, string titleUrl, string jsLink)
        {
            var wpXml = WebpartXmlExtensions
                            .LoadWebpartXmlDocument(BuiltInWebPartTemplates.XsltListViewWebPart)
                            .SetListName(listName)
                            .SetListId(listId)
                            .SetTitleUrl(titleUrl)
                            .SetOrUpdateProperty("JSLink", jsLink)
                            .ToString();
        }


        #endregion
    }
}