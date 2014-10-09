using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region content type order


        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_CanSetupUniqueContentTypeOrderForLibrary()
        {

        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_CanSetupUniqueContentTypeOrderForList()
        {
            ContentTypeDefinition ctFirst = null;
            ContentTypeDefinition ctSecond = null;
            ContentTypeDefinition ctThird = null;

            var siteModel = SPMeta2Model
                 .NewSiteModel(site =>
                 {
                     site
                         .AddRandomContentType(ct => { ctFirst = ct.Value as ContentTypeDefinition; })
                         .AddRandomContentType(ct => { ctSecond = ct.Value as ContentTypeDefinition; })
                         .AddRandomContentType(ct => { ctThird = ct.Value as ContentTypeDefinition; });
                 });

            ctFirst.Name = "first_" + ctFirst.Name;
            ctSecond.Name = "second_" + ctSecond.Name;
            ctThird.Name = "third_" + ctThird.Name;

            TestModel(siteModel);

            var webModel = SPMeta2Model
                  .NewWebModel(web =>
                  {
                      web
                          .AddRandomList(list =>
                          {
                              list
                                  .AddContentTypeLink(ctFirst)
                                  .AddContentTypeLink(ctSecond)
                                  .AddContentTypeLink(ctThird)
                                  .AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                                  {
                                      ContentTypes = new List<UniqueContentTypeOrderValue>
                                      {
                                          new UniqueContentTypeOrderValue { ContentTypeName = ctSecond.Name },
                                          new UniqueContentTypeOrderValue { ContentTypeName = ctThird.Name },
                                          new UniqueContentTypeOrderValue { ContentTypeName = ctFirst.Name },
                                      }
                                  });
                          });
                  });

            TestModel(webModel);
        }


        #endregion

        #region lists

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_GenericList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_DocumentLibrary()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_CalendarList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Events;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_LinksList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Links;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_TasksList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_TasksWithTimelineAndHierarchyList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.TasksWithTimelineAndHierarchy;
            });
        }

        #endregion
    }
}
