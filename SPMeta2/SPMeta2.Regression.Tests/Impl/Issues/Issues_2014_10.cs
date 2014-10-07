using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Services;

namespace SPMeta2.Regression.Tests.Impl.Issues
{
    [TestClass]
    public class Issues_2014_10 : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Issues_2014_10")]
        // #106
        public void Can_PushCalculatedField()
        {
            var genService = ServiceFactory.ModelGeneratorService;

            var calcField = genService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Calculated;
            });

            var parentContentType = genService.GetRandomDefinition<ContentTypeDefinition>();
            var childContentType = genService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = parentContentType.GetContentTypeId();
            });

            var siteMode = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site
                      .AddField(calcField)
                      .AddContentType(parentContentType, ct =>
                      {
                          ct.AddContentTypeFieldLink(calcField);
                      })
                      .AddContentType(childContentType);
                });

            TestModel(siteMode);
        }

        #endregion

    }
}
