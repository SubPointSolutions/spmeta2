using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class UniqueContentTypeFieldsScenariousTest : SPMeta2RegresionScenarioTestBase
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
        
        #region tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.UniqueContentTypeFields")]
        public void CanDeploy_UniqueContentTypeFieldsOrder_ForIssueContentType()
        {
            var ctDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Issue;
            });

            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddContentType(ctDef, contentType => 
                    {
                        contentType
                            .AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                            {
                                Fields = new List<FieldLinkValue>
                                {
                                    new FieldLinkValue{ InternalName = BuiltInInternalFieldNames.TaskDueDate },
                                    new FieldLinkValue{ InternalName = BuiltInInternalFieldNames.Title }
                                }
                            });

                    });
                });

            TestModel(model);
        }

        #endregion
    }
}
