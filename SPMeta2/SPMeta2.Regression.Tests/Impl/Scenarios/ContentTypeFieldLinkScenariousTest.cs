using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ContentTypeFieldLinkScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypeFieldLink")]
        public void CanDeploy_ContentTypeFieldLink_WithProperties()
        {
            var fieldOne = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var fieldTwo = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var contentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(fieldOne);
                site.AddField(fieldTwo);

                site.AddContentType(contentType, c =>
                {
                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        FieldId = fieldOne.Id,
                        DisplayName = Rnd.String(),
                        Hidden = Rnd.Bool(),
                        Required = Rnd.Bool()
                    });

                    c.AddContentTypeFieldLink(new ContentTypeFieldLinkDefinition
                    {
                        DisplayName = Rnd.String(),
                        FieldId = fieldTwo.Id,
                        Hidden = Rnd.Bool(),
                        Required = Rnd.Bool()
                    });
                });
            });

            TestModels(new[] { siteModel });
        }

        #endregion
    }
}
