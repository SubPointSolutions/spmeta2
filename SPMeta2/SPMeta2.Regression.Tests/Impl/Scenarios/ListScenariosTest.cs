using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

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

        protected class ContentTypeEnvironment
        {
            public ContentTypeDefinition First { get; set; }
            public ContentTypeDefinition Second { get; set; }
            public ContentTypeDefinition Third { get; set; }

            public ListDefinition List { get; set; }

            public ModelNode FirstLink { get; set; }
            public ModelNode SecondLink { get; set; }
            public ModelNode ThirdLink { get; set; }

            public ModelNode SiteModel { get; set; }
            public ModelNode WebModel { get; set; }
        }

        private ContentTypeEnvironment GetContentTypeLinksSandbox(
            Action<ModelNode, ContentTypeEnvironment> siteModelConfig,
            Action<ModelNode, ContentTypeEnvironment> webModelConfig,
            Action<ModelNode, ContentTypeEnvironment> listModelConfig)
        {
            var result = new ContentTypeEnvironment();

            // site model

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

            result.First = ctFirst;
            result.Second = ctSecond;
            result.Third = ctThird;

            result.SiteModel = siteModel;

            if (siteModelConfig != null)
                siteModelConfig(result.SiteModel, result);

            // web model
            var webModel = SPMeta2Model
                 .NewWebModel(web =>
                 {
                     web
                         .AddRandomList(list =>
                         {
                             list
                                 .AddContentTypeLink(ctFirst, link =>
                                 {
                                     result.FirstLink = link;
                                     link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeLink(ctSecond, link =>
                                 {
                                     result.SecondLink = link;
                                     link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeLink(ctThird, link =>
                                 {
                                     result.ThirdLink = link;
                                     link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                 });

                             result.List = list.Value as ListDefinition;

                             if (listModelConfig != null)
                                 listModelConfig(list, result);
                         });
                 });

            result.WebModel = webModel;

            if (webModelConfig != null)
                webModelConfig(result.WebModel, result);

            return result;
        }

        #region removing content types

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.ContentTypeLinks")]
        public void CanDeploy_CanRemoveContentTypeLinksInLibrary()
        {
            var env = GetContentTypeLinksSandbox(
                (siteModel, e) =>
                {
                    e.First.ParentContentTypeId = BuiltInContentTypeId.Document;
                    e.Second.ParentContentTypeId = BuiltInContentTypeId.Document;
                    e.Third.ParentContentTypeId = BuiltInContentTypeId.Document;
                },
                (webModel, e) =>
                {

                },
                (listModel, e) =>
                {
                    e.List.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    e.List.ContentTypesEnabled = true;

                    listModel
                       .AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition
                       {
                           ContentTypes = new List<ContentTypeLinkValue>
                           {
                                          new ContentTypeLinkValue { ContentTypeName = e.Second.Name },
                                          new ContentTypeLinkValue { ContentTypeName = e.First.Name },
                           }
                       }, m =>
                       {
                           m.OnProvisioned<object>(ctx =>
                           {
                               // disable validation on content type  links as they would be deleted by 'RemoveContentTypeLinksDefinition'

                               e.FirstLink.Options.RequireSelfProcessing = e.FirstLink.Value.RequireSelfProcessing = false;
                               e.SecondLink.Options.RequireSelfProcessing = e.SecondLink.Value.RequireSelfProcessing = false;
                               e.ThirdLink.Options.RequireSelfProcessing = e.ThirdLink.Value.RequireSelfProcessing = false;
                           });
                       });
                });

            TestModel(env.SiteModel);

            // we need to skip ct links validation
            TestModel(env.WebModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.ContentTypeLinks")]
        public void CanDeploy_CanRemoveContentTypeLinksInList()
        {
            var env = GetContentTypeLinksSandbox(
                (siteModel, e) =>
                {
                    e.First.ParentContentTypeId = BuiltInContentTypeId.Item;
                    e.Second.ParentContentTypeId = BuiltInContentTypeId.Item;
                    e.Third.ParentContentTypeId = BuiltInContentTypeId.Item;
                },
                (webModel, e) =>
                {

                },
                (listModel, e) =>
                {
                    e.List.TemplateType = BuiltInListTemplateTypeId.GenericList;
                    e.List.ContentTypesEnabled = true;

                    listModel
                       .AddRemoveContentTypeLinks(new RemoveContentTypeLinksDefinition
                       {
                           ContentTypes = new List<ContentTypeLinkValue>
                           {
                                          new ContentTypeLinkValue { ContentTypeName = e.Second.Name },
                                          new ContentTypeLinkValue { ContentTypeName = e.First.Name },
                           }
                       }, m =>
                       {
                           m.OnProvisioned<object>(ctx =>
                           {
                               // disable validation on content type  links as they would be deleted by 'RemoveContentTypeLinksDefinition'

                               e.FirstLink.Options.RequireSelfProcessing = e.FirstLink.Value.RequireSelfProcessing = false;
                               e.SecondLink.Options.RequireSelfProcessing = e.SecondLink.Value.RequireSelfProcessing = false;
                               e.ThirdLink.Options.RequireSelfProcessing = e.ThirdLink.Value.RequireSelfProcessing = false;
                           });
                       });
                });

            TestModel(env.SiteModel);
            TestModel(env.WebModel);
        }

        #endregion

        #region hiding content types

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.ContentTypeLinks")]
        public void CanDeploy_CanHideContentTypeLinksInLibrary()
        {
            var env = GetContentTypeLinksSandbox(
                (siteModel, e) =>
                {
                    e.First.ParentContentTypeId = BuiltInContentTypeId.Document;
                    e.Second.ParentContentTypeId = BuiltInContentTypeId.Document;
                    e.Third.ParentContentTypeId = BuiltInContentTypeId.Document;
                },
                (webModel, e) =>
                {

                },
                (listModel, e) =>
                {
                    e.List.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    e.List.ContentTypesEnabled = true;

                    listModel
                       .AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                       {
                           ContentTypes = new List<ContentTypeLinkValue>
                           {
                                          new ContentTypeLinkValue { ContentTypeName = e.Second.Name },
                                          new ContentTypeLinkValue { ContentTypeName = e.First.Name },
                           }
                       });
                });

            TestModel(env.SiteModel);
            TestModel(env.WebModel);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.ContentTypeLinks")]
        public void CanDeploy_CanHideContentTypeLinksInList()
        {
            var env = GetContentTypeLinksSandbox(
                (siteModel, e) =>
                {
                    e.First.ParentContentTypeId = BuiltInContentTypeId.Item;
                    e.Second.ParentContentTypeId = BuiltInContentTypeId.Item;
                    e.Third.ParentContentTypeId = BuiltInContentTypeId.Item;
                },
                (webModel, e) =>
                {

                },
                (listModel, e) =>
                {
                    e.List.TemplateType = BuiltInListTemplateTypeId.GenericList;
                    e.List.ContentTypesEnabled = true;

                    listModel
                       .AddHideContentTypeLinks(new HideContentTypeLinksDefinition
                       {
                           ContentTypes = new List<ContentTypeLinkValue>
                           {
                                          new ContentTypeLinkValue { ContentTypeName = e.Second.Name },
                                          new ContentTypeLinkValue { ContentTypeName = e.First.Name },
                           }
                       });
                });

            TestModel(env.SiteModel);
            TestModel(env.WebModel);
        }

        #endregion

        #region content type order


        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.ContentTypeLinks")]
        public void CanDeploy_CanSetupUniqueContentTypeOrderForLibrary()
        {
            var env = GetContentTypeLinksSandbox(
                (siteModel, e) =>
                {
                    e.First.ParentContentTypeId = BuiltInContentTypeId.Document;
                    e.Second.ParentContentTypeId = BuiltInContentTypeId.Document;
                    e.Third.ParentContentTypeId = BuiltInContentTypeId.Document;
                },
                (webModel, e) =>
                {

                },
                (listModel, e) =>
                {
                    e.List.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    e.List.ContentTypesEnabled = true;

                    listModel
                      .AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                      {
                          ContentTypes = new List<ContentTypeLinkValue>
                          {
                                              new ContentTypeLinkValue { ContentTypeName = e.Second.Name },
                                              new ContentTypeLinkValue { ContentTypeName = e.Third.Name },
                                              new ContentTypeLinkValue { ContentTypeName = e.First.Name },
                          }
                      });
                });

            TestModel(env.SiteModel);
            TestModel(env.WebModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.ContentTypeLinks")]
        public void CanDeploy_CanSetupUniqueContentTypeOrderForList()
        {
            var env = GetContentTypeLinksSandbox(
                 (siteModel, e) =>
                 {
                     e.First.ParentContentTypeId = BuiltInContentTypeId.Item;
                     e.Second.ParentContentTypeId = BuiltInContentTypeId.Item;
                     e.Third.ParentContentTypeId = BuiltInContentTypeId.Item;
                 },
                 (webModel, e) =>
                 {

                 },
                 (listModel, e) =>
                 {
                     e.List.TemplateType = BuiltInListTemplateTypeId.GenericList;
                     e.List.ContentTypesEnabled = true;

                     listModel
                       .AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                       {
                           ContentTypes = new List<ContentTypeLinkValue>
                          {
                                              new ContentTypeLinkValue { ContentTypeName = e.Second.Name },
                                              new ContentTypeLinkValue { ContentTypeName = e.Third.Name },
                                              new ContentTypeLinkValue { ContentTypeName = e.First.Name },
                          }
                       });
                 });

            TestModel(env.SiteModel);
            TestModel(env.WebModel);
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
