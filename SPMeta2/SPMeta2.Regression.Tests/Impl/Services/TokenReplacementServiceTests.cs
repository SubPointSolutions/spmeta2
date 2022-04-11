using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class TokenReplacementServiceTests : SPMeta2RegresionScenarioTestBase
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

        #region tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.TokenReplacementService")]
        public void Can_Replace_Site_Token_On_RootWeb()
        {
            // Incorrect ~site token resolution for CSOM for the subwebs #863
            // https://github.com/SubPointSolutions/spmeta2/issues/863

            var linkUrl = UrlUtility.CombineUrl(new[] { "lib-" + Rnd.String(), "content.html" });

            var tokenUrl = UrlUtility.CombineUrl(new[] { "~site", linkUrl });
            var expectedUrl = UrlUtility.CombineUrl(new[] { "/", linkUrl });

            var cewpDef = ModelGeneratorService.GetRandomDefinition<ContentEditorWebPartDefinition>(def =>
            {
                def.Content = string.Empty;
                def.ContentLink = tokenUrl;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentEditorWebPart(cewpDef);
                    });
                });
            });

            TestModelAndUrl(model, expectedUrl);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TokenReplacementService")]
        public void Can_Replace_Site_Token_On_SubWeb()
        {
            // Incorrect ~site token resolution for CSOM for the subwebs #863
            // https://github.com/SubPointSolutions/spmeta2/issues/863

            var subWebUrl = string.Format("web-{0}", Rnd.String());

            var linkUrl = UrlUtility.CombineUrl(new[] { "lib-" + Rnd.String(), "content.html" });

            // target URL with token and expected URL with the sub site
            var tokenUrl = UrlUtility.CombineUrl(new[] { "~site", linkUrl });
            var expectedUrl = UrlUtility.CombineUrl(new[] { "/", subWebUrl, linkUrl });

            var webDefinition = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.Url = subWebUrl;
            });

            var cewpDef = ModelGeneratorService.GetRandomDefinition<ContentEditorWebPartDefinition>(def =>
            {
                def.Content = string.Empty;
                def.ContentLink = tokenUrl;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(webDefinition, subWeb =>
                {
                    subWeb.AddRandomDocumentLibrary(list =>
                    {
                        list.AddRandomWebPartPage(page =>
                        {
                            page.AddContentEditorWebPart(cewpDef);
                        });
                    });
                });
            });

            TestModelAndUrl(model, expectedUrl);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.TokenReplacementService")]
        public void Can_Replace_Site_Token_On_SubSubWeb()
        {
            // Incorrect ~site token resolution for CSOM for the subwebs #863
            // https://github.com/SubPointSolutions/spmeta2/issues/863

            var subWeb1Url = string.Format("web1-{0}", Rnd.String());
            var subWeb2Url = string.Format("web2-{0}", Rnd.String());

            var linkUrl = UrlUtility.CombineUrl(new[] { "lib-" + Rnd.String(), "content.html" });

            // target URL with token and expected URL with the sub site
            var tokenUrl = UrlUtility.CombineUrl(new[] { "~site", linkUrl });
            var expectedUrl = UrlUtility.CombineUrl(new[] { "/", subWeb2Url, linkUrl });

            var web1Definition = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.Url = subWeb1Url;
            });

            var web2Definition = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.Url = subWeb2Url;
            });

            var cewpDef = ModelGeneratorService.GetRandomDefinition<ContentEditorWebPartDefinition>(def =>
            {
                def.Content = string.Empty;
                def.ContentLink = tokenUrl;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(web1Definition, sub1Web =>
                {
                    web.AddWeb(web2Definition, sub2Web =>
                    {
                        sub2Web.AddRandomDocumentLibrary(list =>
                        {
                            list.AddRandomWebPartPage(page =>
                            {
                                page.AddContentEditorWebPart(cewpDef);
                            });
                        });
                    });
                });
            });

            TestModelAndUrl(model, expectedUrl);
        }

        #endregion

        #region utils

        protected void TestModelAndUrl(ModelNode model, string expectedEndWithUrl)
        {
            // Incorrect ~site token resolution for CSOM for the subwebs #863
            // https://github.com/SubPointSolutions/spmeta2/issues/863

            // handle token replacement event, match with the URL
            // test on the URL structure

            // initial provision would push all the CSOM/SSOM specific services into container
            TestModel(model);

            // get the token replacement service
            var serviceMapping = ServiceContainer.Instance
                                                 .Services
                                                 .FirstOrDefault(m => m.Value.Any(s => s is TokenReplacementServiceBase))
                                                 .Value;

            var tokenReplacementService = serviceMapping.FirstOrDefault(s => s is TokenReplacementServiceBase) as TokenReplacementServiceBase;
            Assert.IsNotNull(tokenReplacementService);

            var hasHit = false;
            var hasCorrectUrl = false;

            tokenReplacementService.OnTokenReplaced += (s, e) =>
            {
                hasHit = true;

                if (e.Result.Value.EndsWith(expectedEndWithUrl))
                {
                    hasCorrectUrl = true;
                }
                else
                {
                    hasCorrectUrl = false;
                }
            };

            TestModel(model);

            Assert.IsTrue(hasHit, string.Format("Has URL hit:[{0}]", expectedEndWithUrl));
            Assert.IsTrue(hasCorrectUrl, string.Format("Has correct URL:[{0}]", expectedEndWithUrl));
        }

        #endregion
    }
}
