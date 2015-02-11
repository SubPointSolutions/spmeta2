using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListFieldLinkScenariousTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.Fields.Options")]
        public void CanDeploy_ListFieldLink_AsAddToDefaultView()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            field.AddToDefaultView = true;

            var listFieldLink = new ListFieldLinkDefinition
            {
                FieldId = field.Id,
                AddToDefaultView = true
            };

            var siteModel = SPMeta2Model
                   .NewSiteModel(site =>
                   {
                       site.AddField(field);
                   });

            var webModel = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddRandomList(list =>
                       {
                           list.AddListFieldLink(listFieldLink);
                       });
                   });

            TestModels(new[] { siteModel, webModel });
        }

        // add ootb list field link, TODO
        // add ootb list field links, TODO

        // add custom list field link, TODO
        // add custom list field links, TODO

        // add ootb AND custom list field links, TODO

        #endregion
    }
}
