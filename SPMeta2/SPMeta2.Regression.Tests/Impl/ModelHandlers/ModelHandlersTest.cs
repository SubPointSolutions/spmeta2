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
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Exceptions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Extensions
{
    [TestClass]
    public class ModelHandlersTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        [TestMethod]
        [TestCategory("Regression.ModelHandlers")]
        public void Model_Handlers_Should_Pass_On_NoNChanged_Definitions()
        {
            var fieldDef = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {

            });

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(fieldDef, field =>
                {
                });
            });

            var hasException = false;

            try
            {
                TestModel(model);
            }
            catch (Exception e)
            {

            }

            Assert.IsFalse(hasException);
        }

        [TestMethod]
        [TestCategory("Regression.ModelHandlers")]
        public void Model_Handlers_Should_Fail_On_Changed_Definitions()
        {
            if (RegressionService.EnableDefinitionImmutabilityValidation)
            {
                var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
                {

                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddContentType(contentTypeDef, field =>
                    {
                        field.OnProvisioned<object>(context =>
                        {
                            (context.ObjectDefinition as ContentTypeDefinition).ParentContentTypeName = BuiltInContentTypeNames.UDCDocument;
                        });
                    });
                });

                var hasException = false;

                try
                {
                    TestModel(model);
                }
                catch (Exception e)
                {
                    if (e is SPMeta2Exception)
                    {
                        if (e.Message.Contains("was changed"))
                        {
                            hasException = true;
                        }
                    }
                }

                Assert.IsTrue(hasException);
            }
        }
    }
}
