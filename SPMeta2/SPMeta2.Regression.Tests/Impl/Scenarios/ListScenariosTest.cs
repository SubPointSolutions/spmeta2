﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Syntax.Default.Utils;

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

            public SiteModelNode SiteModel { get; set; }
            public WebModelNode WebModel { get; set; }
        }

        private ContentTypeEnvironment GetContentTypeLinksSandbox(
            Action<SiteModelNode, ContentTypeEnvironment> siteModelConfig,
            Action<WebModelNode, ContentTypeEnvironment> webModelConfig,
            Action<ListModelNode, ContentTypeEnvironment> listModelConfig)
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
                                     //link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                     link.Options.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeLink(ctSecond, link =>
                                 {
                                     result.SecondLink = link;
                                     //link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                     link.Options.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeLink(ctThird, link =>
                                 {
                                     result.ThirdLink = link;
                                     //link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                     link.Options.RequireSelfProcessing = true;
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

                               //e.FirstLink.Options.RequireSelfProcessing = e.FirstLink.Value.RequireSelfProcessing = false;
                               //e.SecondLink.Options.RequireSelfProcessing = e.SecondLink.Value.RequireSelfProcessing = false;
                               //e.ThirdLink.Options.RequireSelfProcessing = e.ThirdLink.Value.RequireSelfProcessing = false;

                               e.FirstLink.Options.RequireSelfProcessing = false;
                               e.SecondLink.Options.RequireSelfProcessing = false;
                               e.ThirdLink.Options.RequireSelfProcessing = false;
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

                               //e.FirstLink.Options.RequireSelfProcessing = e.FirstLink.Value.RequireSelfProcessing = false;
                               //e.SecondLink.Options.RequireSelfProcessing = e.SecondLink.Value.RequireSelfProcessing = false;
                               //e.ThirdLink.Options.RequireSelfProcessing = e.ThirdLink.Value.RequireSelfProcessing = false;

                               e.FirstLink.Options.RequireSelfProcessing = false;
                               e.SecondLink.Options.RequireSelfProcessing = false;
                               e.ThirdLink.Options.RequireSelfProcessing = false;
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
                def.EnableAttachments = false;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_CalendarList()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                TestRandomDefinition<ListDefinition>(def =>
                {
                    def.EnableFolderCreation = false;
                    def.TemplateType = BuiltInListTemplateTypeId.Events;
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_LinksList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.EnableMinorVersions = false;

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

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_ListByTemplateName()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = 0;
                def.TemplateName = BuiltInListTemplates.AssetLibrary.InternalName;
            });
        }



        #endregion

        #region list URLs

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_ListWithDotURLsByTemplateName()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = 0;
                def.TemplateName = BuiltInListTemplates.AssetLibrary.InternalName;
#pragma warning disable 618
                def.Url = string.Format("{0}.{1}.{2}", Rnd.String(5), Rnd.String(5), Rnd.String(5));
#pragma warning restore 618
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_ListWithLibraryPath()
        {
            var testList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.CustomUrl = Rnd.String();
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(testList);
            });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_LibraryWithListPath()
        {
            var testList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                def.CustomUrl = string.Format("Lists/{0}", Rnd.String());
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(testList);
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_GenericListWithDotsInUrl()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
#pragma warning disable 618
                def.Url = string.Format("{0}.{1}.{2}", Rnd.String(5), Rnd.String(5), Rnd.String(5));
#pragma warning restore 618
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_DocumentLibraryWithDotsInUrl()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;

                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = string.Format("{0}.{1}.{2}", Rnd.String(5), Rnd.String(5), Rnd.String(5));
#pragma warning restore 618
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_ListWithCustomUrl()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = string.Empty;
#pragma warning restore 618
                def.CustomUrl = string.Format("Lists/{0}", Rnd.String());
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Urls")]
        public void CanDeploy_LibraryWithCustomUrl()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = string.Empty;
#pragma warning restore 618
                def.CustomUrl = string.Format("{0}", Rnd.String());
            });
        }

        #endregion

        #region custom template URLs

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.DocumentTemplateUrl")]
        public void CanDeploy_DocumentLibrary_With_Custom_DocumentTemplateUrl()
        {
            var templateFileName = Rnd.TxtFileName();

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    var randomList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
                    {
                        def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    });

                    // add first to provision, add a module
                    web.AddList(randomList, list =>
                    {
                        list.AddHostFolder(BuiltInFolderDefinitions.Forms, folder =>
                        {
                            folder.AddModuleFile(new ModuleFileDefinition
                            {
                                FileName = templateFileName,
                                Content = Encoding.UTF8.GetBytes(Rnd.String())
                            });
                        });
                    });

                    // add a clone second time with the template
                    var listWithDocumentTemplate = randomList.Inherit();
#pragma warning disable 618
                    listWithDocumentTemplate.DocumentTemplateUrl = string.Format("/" + randomList.GetListUrl() + "/Forms/" + templateFileName);
#pragma warning restore 618

                    web.AddList(listWithDocumentTemplate);
                });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.DocumentTemplateUrl")]
        public void CanDeploy_DocumentLibrary_With_SiteCollectionToken_DocumentTemplateUrl()
        {
            var templateFileName = Rnd.TxtFileName();

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    var randomList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
                    {
                        def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    });

                    // add first to provision, add a module
                    web.AddList(randomList, list =>
                    {
                        list.AddHostFolder(BuiltInFolderDefinitions.Forms, folder =>
                        {
                            folder.AddModuleFile(new ModuleFileDefinition
                            {
                                FileName = templateFileName,
                                Content = Encoding.UTF8.GetBytes(Rnd.String())
                            });
                        });
                    });

                    // add a clone second time with the template
                    var listWithDocumentTemplate = randomList.Inherit();
#pragma warning disable 618
                    listWithDocumentTemplate.DocumentTemplateUrl = string.Format("~sitecollection/" + randomList.GetListUrl() + "/Forms/" + templateFileName);
#pragma warning restore 618

                    web.AddList(listWithDocumentTemplate);
                });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.DocumentTemplateUrl")]
        public void CanDeploy_DocumentLibrary_With_WebCollectionToken_DocumentTemplateUrl()
        {
            var templateFileName = Rnd.TxtFileName();

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    var randomList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
                    {
                        def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                    });

                    // add first to provision, add a module
                    web.AddList(randomList, list =>
                    {
                        list.AddHostFolder(BuiltInFolderDefinitions.Forms, folder =>
                        {
                            folder.AddModuleFile(new ModuleFileDefinition
                            {
                                FileName = templateFileName,
                                Content = Encoding.UTF8.GetBytes(Rnd.String())
                            });
                        });
                    });

                    // add a clone second time with the template
                    var listWithDocumentTemplate = randomList.Inherit();
#pragma warning disable 618
                    listWithDocumentTemplate.DocumentTemplateUrl = string.Format("~site/" + randomList.GetListUrl() + "/Forms/" + templateFileName);
#pragma warning restore 618

                    web.AddList(listWithDocumentTemplate);
                });

            TestModel(webModel);
        }

        #endregion

        #region custom templates

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.CustomTemplates")]
        public void CanDeploy_List_From_STPListTemplate()
        {
            var stpTemplateContent = ModuleFileUtils.FromResource(
                    GetType().Assembly,
                    "SPMeta2.Regression.Tests.Templates.STP.M2Template.stp");

            var stpTemplateFeatureId = new Guid("{00BFEA71-DE22-43B2-A848-C05709900100}");

            // upload stp to list templates
            var stpModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddHostList(BuiltInListDefinitions.Catalogs.ListTemplates, list =>
                    {
                        list.AddModuleFile(new ModuleFileDefinition
                        {
                            FileName = "M2Template.stp",
                            Content = stpTemplateContent
                        });
                    });
                });

            var customListDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = Rnd.String();
#pragma warning restore 618
                def.CustomUrl = string.Empty;

                def.TemplateType = 0;
                def.TemplateName = "M2Template";
            });

            // deploy template to both current and sub web
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(customListDef.Inherit<ListDefinition>());

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddList(customListDef.Inherit<ListDefinition>());
                });
            });

            TestModel(stpModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.CustomTemplates")]
        public void CanDeploy_List_From_CustomTemplate()
        {
            var sandboxSolution = new SandboxSolutionDefinition()
            {
                FileName = string.Format("{0}.wsp", Rnd.String()),
                Activate = true,

                SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb"),
                Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Regression.Tests.Templates.SandboxSolutions.SPMeta2.Containers.SandboxSolutionContainer.wsp")
            };

            var customListDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = Rnd.String();
#pragma warning restore 618
                def.CustomUrl = string.Empty;

                // would give invalid list template exception
                //def.TemplateType = 10500;

                // works well, just reset TemplateType as GetRandomDefinition<> generated random TemplateType
                def.TemplateType = 0;
                def.TemplateName = "CustomerList";
            });

            var siteModel = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddSandboxSolution(sandboxSolution);
                });

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(subWeb =>
                    {
                        subWeb.AddWebFeature(new FeatureDefinition
                        {
                            Id = new Guid("b997a462-8efb-44cf-92c0-457e75c81798"),
                            Scope = FeatureDefinitionScope.Web,
                            Enable = true,
                            ForceActivate = true
                        });

                        subWeb.AddList(customListDef);
                    });


                });

            TestModel(siteModel, webModel);
        }

        #endregion

        #region localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.Localization")]
        public void CanDeploy_Localized_List()
        {
            var definition = GetLocalizedDefinition();
            var subWebDefinition = GetLocalizedDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(definition);

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddList(subWebDefinition);
                });
            });

            TestModel(model);
        }

        #endregion

        #region special props

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_List_With_MajorVersionLimit()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = Rnd.String();
#pragma warning restore 618
                def.CustomUrl = string.Empty;

                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                def.TemplateName = string.Empty;

                def.EnableMinorVersions = true;
                def.EnableVersioning = true;

                def.MajorVersionLimit = Rnd.Int(50) + 1;
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef);
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_List_With_MajorWithMinorVersionsLimit()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.ForceCheckout = false;

#pragma warning disable 618
                def.Url = Rnd.String();
#pragma warning restore 618
                def.CustomUrl = string.Empty;

                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                def.TemplateName = string.Empty;

                def.EnableMinorVersions = true;
                def.EnableVersioning = true;

                def.MajorVersionLimit = Rnd.Int(50) + 1;
                def.MajorWithMinorVersionsLimit = Rnd.Int(50) + 1;
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef);
            });

            TestModel(webModel);
        }

        #endregion

        #region utils

        protected ListDefinition GetLocalizedDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<ListDefinition>();
            var localeIds = Rnd.LocaleIds();

            foreach (var localeId in localeIds)
            {
                definition.TitleResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedTitle_{0}", localeId)
                });

                definition.DescriptionResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedDescription_{0}", localeId)
                });
            }

            return definition;
        }

        #endregion

        #region index props

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists.IndexedProps")]
        public void CanDeploy_Lists_WithIndexed_Props()
        {
            var testList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.CustomUrl = Rnd.String();

                def.IndexedRootFolderPropertyKeys.Add(new IndexedPropertyValue
                {
                    Name = string.Format("name_{0}", Rnd.String()),
                    Value = string.Format("value_{0}", Rnd.String()),
                });

                def.IndexedRootFolderPropertyKeys.Add(new IndexedPropertyValue
                {
                    Name = string.Format("name_{0}", Rnd.String()),
                    Value = string.Format("value_{0}", Rnd.String()),
                });
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(testList);
            });

            TestModel(model);
        }

        #endregion
    }
}
