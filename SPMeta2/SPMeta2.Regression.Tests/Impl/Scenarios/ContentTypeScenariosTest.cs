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
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Utils;
using SPMeta2.Containers.Templates.Documents;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ContentTypeScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region scopes

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.Scopes")]
        public void CanDeploy_SiteScoped_ContentType()
        {
            var contentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var model = SPMeta2Model
                   .NewSiteModel(site =>
                   {
                       site.AddContentType(contentType);
                   });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.Scopes")]
        public void CanDeploy_WebScoped_ContentType()
        {
            var contentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var model = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddRandomWeb(subWeb =>
                       {
                           subWeb.AddContentType(contentType);
                       });
                   });

            TestModel(model);
        }


        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes")]
        public void CanDeploy_CustomListItemContentType()
        {
            TestRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes")]
        public void CanDeploy_CustomDocumentContentType()
        {
            TestRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Document;
            });
        }

        protected List<ContentTypeDefinition> GetHierarchicalContentTypes()
        {
            var root = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var levelOne = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = root.GetContentTypeId();
            });

            var levelTwo = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = levelOne.GetContentTypeId();
            });

            return new List<ContentTypeDefinition>(new ContentTypeDefinition[]
            {
                root,   
                levelOne,
                levelTwo
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.ParentChild")]
        public void CanDeploy_HierarchicalContentTypesOrderByIdAcs()
        {
            var contentTypes = GetHierarchicalContentTypes();

            contentTypes.Sort(delegate(ContentTypeDefinition c1, ContentTypeDefinition c2)
            {
                return c1.IsChildOf(c2) ? -1 : 1;
            });

            var siteModel = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddContentTypes(contentTypes);
                });

            TestModel(siteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.ParentChild")]
        public void CanDeploy_HierarchicalContentTypesOrderByIdDesc()
        {
            var contentTypes = GetHierarchicalContentTypes();
            contentTypes = contentTypes.OrderByDescending(c => c.Id).ToList();

            contentTypes.Sort(delegate(ContentTypeDefinition c1, ContentTypeDefinition c2)
            {
                return c1.IsChildOf(c2) ? 1 : -1;
            });

            var siteModel = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddContentTypes(contentTypes);
                });

            TestModel(siteModel);
        }

        public static class LConsts
        {

            public static string DefaultMetadataGroup = "sd";
        }

        #region how tos

        public static class LContentTypes
        {

            public static ContentTypeDefinition InformationLink = new ContentTypeDefinition
            {
                Name = "Information Link",
                Id = new Guid("98AD3CDE-FC96-47CA-93D6-F7D747AD45A8"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition BusinessApplicationLink = new ContentTypeDefinition
            {
                Name = "Business Application Link",
                Id = new Guid("EBA48FCD-77DF-4AE0-90BF-BE62A37D6808"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition StaffDirectoryLink = new ContentTypeDefinition
            {
                Name = "Staff Directory Link",
                Id = new Guid("9CAA5DA6-5A23-4CD1-979D-F0B09A8A85F2"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = LConsts.DefaultMetadataGroup
            };

            #endregion

            #region properties

            public static ContentTypeDefinition LuxbetGeneralDocument = new ContentTypeDefinition
            {
                Id = new Guid("{0a4eae83-fe04-4d47-bb30-f4aef8aac652}"),
                Name = "Luxbet General Document",
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetCorporateDocument = new ContentTypeDefinition
            {
                Id = new Guid("{6242bc6c-e0d4-4d7e-b14f-8214cbf4a015}"),
                Name = "Luxbet Corporate Document",
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetAuditDocument = new ContentTypeDefinition
            {
                Id = new Guid("{e4f1f6de-4e2e-4a9c-84bc-199a52781ee2}"),
                Name = "Luxbet Audit Document",
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetReport = new ContentTypeDefinition
            {
                Id = new Guid("{4687e300-cfa8-49a2-b396-9d984e406cb1}"),
                Name = "Luxbet Report",
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetEvent = new ContentTypeDefinition
            {
                Id = new Guid("{8ecab7cf-2d6c-4f51-8699-82fda00b1d44}"),
                Name = "Luxbet Event",
                ParentContentTypeId = BuiltInContentTypeId.Event,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetForm = new ContentTypeDefinition
            {
                Id = new Guid("{8f23a727-ede5-4a2f-85eb-c084b8fb8cdb}"),
                Name = "Luxbet Form",
                ParentContentTypeId = LContentTypes.LuxbetCorporateDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetStandard = new ContentTypeDefinition
            {
                Id = new Guid("{4ac27812-d233-4ebd-ab97-256edb116981}"),
                Name = "Luxbet Standard",
                ParentContentTypeId = LContentTypes.LuxbetCorporateDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetProcedureAndProcess = new ContentTypeDefinition
            {
                Id = new Guid("{45728966-2fcd-4b04-8146-181baa453625}"),
                Name = "Luxbet Procedure and Process",
                ParentContentTypeId = LContentTypes.LuxbetCorporateDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetGuideline = new ContentTypeDefinition
            {
                Id = new Guid("{fc084261-638a-4bb0-90c6-0c8122d5f58f}"),
                Name = "Luxbet Guideline",
                ParentContentTypeId = LContentTypes.LuxbetCorporateDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetPolicy = new ContentTypeDefinition
            {
                Id = new Guid("{f91e118e-b081-49f0-aedf-05554dc642f0}"),
                Name = "Luxbet Policy",
                ParentContentTypeId = LContentTypes.LuxbetCorporateDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetTemplate = new ContentTypeDefinition
            {
                Id = new Guid("{d81fc88d-10bd-4154-a061-8b0eb5639070}"),
                Name = "Luxbet Template",
                ParentContentTypeId = LContentTypes.LuxbetCorporateDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetMeetingAndMinutes = new ContentTypeDefinition
            {
                Id = new Guid("{b5afa0e4-a1e3-4329-9f2f-70000fd08092}"),
                Name = "Luxbet Meeting and Minutes",
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetMarketingAndPromotions = new ContentTypeDefinition
            {
                Id = new Guid("{ea5fa8c4-846b-4151-9c77-6c27d52e2d31}"),
                Name = "Luxbet Marketing and Promotions",
                ParentContentTypeId = LContentTypes.LuxbetGeneralDocument.GetContentTypeId(),
                Group = LConsts.DefaultMetadataGroup
            };


            public static ContentTypeDefinition LuxbetWikiArticle = new ContentTypeDefinition
            {
                Id = new Guid("{3cf749b6-2db4-46c3-99d4-171bb5189aa1}"),
                Name = "Luxbet Wiki Article",
                ParentContentTypeId = BuiltInContentTypeId.WikiDocument,
                Group = LConsts.DefaultMetadataGroup
            };


            public static ContentTypeDefinition LuxbetLeave = new ContentTypeDefinition
            {
                Id = new Guid("{30512bbd-aefb-4e54-842b-44f961f7123d}"),
                Name = "Luxbet Leave",
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = LConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition LuxbetNewsArticle = new ContentTypeDefinition
            {
                Id = new Guid("8FEF0FBA-299F-44B7-8831-D55858ACCFB2"),
                Name = "Luxbet News Article",
                ParentContentTypeId = BuiltInPublishingContentTypeId.ArticlePage,
                Group = LConsts.DefaultMetadataGroup
            };
        }

        #endregion

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.ParentChild")]
        public void CanDeploy_HierarchicalCustomContentTypes()
        {
            var parent = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = "Parent";
                def.Id = new Guid("{0a4eae83-fe04-4d47-bb30-f4aef8aac652}");
            });

            var child = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = "Child";
                def.Id = new Guid("{ea5fa8c4-846b-4151-9c77-6c27d52e2d31}");
                def.ParentContentTypeId = parent.GetContentTypeId();
            });

            var siteModel = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddContentTypes(child);
                    site.AddContentTypes(parent);

                    site
                        .AddContentType(LContentTypes.InformationLink, contentType =>
                        {
                        })

                         .AddContentType(LContentTypes.BusinessApplicationLink, contentType =>
                         {
                           
                         })

                          .AddContentType(LContentTypes.StaffDirectoryLink, contentType =>
                          {
                              
                          })
                        .AddContentType(LContentTypes.LuxbetGeneralDocument, contentType =>
                        {
                          
                        })
                        .AddContentType(LContentTypes.LuxbetCorporateDocument, contentType =>
                        {
                            
                        })
                        .AddContentType(LContentTypes.LuxbetPolicy, contentType =>
                        {
                          
                        })
                        .AddContentType(LContentTypes.LuxbetAuditDocument, contentType =>
                        {
                           
                        })

                        .AddContentType(LContentTypes.LuxbetEvent, contentType =>
                        {
                           
                        })
                        .AddContentType(LContentTypes.LuxbetForm, contentType =>
                        {
                          
                        })

                        .AddContentType(LContentTypes.LuxbetGuideline, contentType =>
                        {
                           
                        })
                        .AddContentType(LContentTypes.LuxbetMeetingAndMinutes, contentType =>
                        {
                           
                        })
                        .AddContentType(LContentTypes.LuxbetProcedureAndProcess, contentType =>
                        {
                           
                        })
                        .AddContentType(LContentTypes.LuxbetReport, contentType =>
                        {
                            
                        })
                        .AddContentType(LContentTypes.LuxbetStandard, contentType =>
                        {
                         
                        })
                        .AddContentType(LContentTypes.LuxbetTemplate, contentType =>
                        {
                          
                        })
                        .AddContentType(LContentTypes.LuxbetLeave)
                        .AddContentType(LContentTypes.LuxbetMarketingAndPromotions, contentType =>
                        {
                           
                        })
                        .AddContentType(LContentTypes.LuxbetWikiArticle)
                        .AddContentType(LContentTypes.LuxbetNewsArticle, contentType =>
                        {
                           
                        });
                  
                });

            TestModel(siteModel);
        }


        protected class ContentTypeEnvironment
        {
            public FieldDefinition First { get; set; }
            public FieldDefinition Second { get; set; }
            public FieldDefinition Third { get; set; }

            public ModelNode FirstLink { get; set; }
            public ModelNode SecondLink { get; set; }
            public ModelNode ThirdLink { get; set; }

            public ContentTypeDefinition ContentType { get; set; }

            public ModelNode SiteModel { get; set; }
        }

        private ContentTypeEnvironment GetContentTypeSandbox(
            Action<ModelNode, ContentTypeEnvironment> siteModelConfig,
            Action<ModelNode, ContentTypeEnvironment> contentTypeModelConfig)
        {
            var result = new ContentTypeEnvironment();

            // site model

            FieldDefinition fldFirst = null;
            FieldDefinition fldSecond = null;
            FieldDefinition fldThird = null;

            var siteModel = SPMeta2Model
                 .NewSiteModel(site =>
                 {
                     site
                         .AddRandomField(ct => { fldFirst = ct.Value as FieldDefinition; })
                         .AddRandomField(ct => { fldSecond = ct.Value as FieldDefinition; })
                         .AddRandomField(ct => { fldThird = ct.Value as FieldDefinition; })
                         .AddRandomContentType(contentType =>
                         {
                             fldFirst.Title = "first_" + fldFirst.Title;
                             fldSecond.Title = "second_" + fldSecond.Title;
                             fldThird.Title = "third_" + fldThird.Title;

                             result.First = fldFirst;
                             result.Second = fldSecond;
                             result.Third = fldThird;

                             result.ContentType = contentType.Value as ContentTypeDefinition;

                             contentType
                                 .AddContentTypeFieldLink(fldFirst, link =>
                                 {
                                     result.FirstLink = link;
                                     link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeFieldLink(fldSecond, link =>
                                 {
                                     result.SecondLink = link;
                                     link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeFieldLink(fldThird, link =>
                                 {
                                     result.ThirdLink = link;
                                     link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                 });

                             if (contentTypeModelConfig != null)
                                 contentTypeModelConfig(contentType, result);
                         });
                 });

            result.SiteModel = siteModel;

            if (siteModelConfig != null)
                siteModelConfig(result.SiteModel, result);

            return result;
        }



        #endregion

        #region content type fields links

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.FieldLinks")]
        public void CanDeploy_CanHideContentTypeFieldLinks()
        {
            var env = GetContentTypeSandbox(
                (siteModel, e) =>
                {

                },
                (contentTypeModel, e) =>
                {
                    contentTypeModel
                       .AddHideContentTypeFieldLinks(new HideContentTypeFieldLinksDefinition
                       {
                           Fields = new List<FieldLinkValue>
                           {
                                          new FieldLinkValue { InternalName = e.Second.InternalName },
                                          new FieldLinkValue { InternalName = e.First.InternalName },
                           }
                       });
                });

            TestModel(env.SiteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.FieldLinks")]
        public void CanDeploy_CanRemoveContentTypeFieldLinks()
        {
            var env = GetContentTypeSandbox(
                (siteModel, e) =>
                {

                },
                (contentTypeModel, e) =>
                {
                    contentTypeModel
                       .AddRemoveContentTypeFieldLinks(new RemoveContentTypeFieldLinksDefinition
                       {
                           Fields = new List<FieldLinkValue>
                           {
                                          new FieldLinkValue { InternalName = e.Second.InternalName },
                                          new FieldLinkValue { InternalName = e.First.InternalName },
                           }
                       }, m =>
                       {
                           m.OnProvisioned<object>(ctx =>
                           {
                               // disable validation on content type field links as they would be deleted by 'RemoveContentTypeFieldLinksDefinition'

                               e.FirstLink.Options.RequireSelfProcessing = e.FirstLink.Value.RequireSelfProcessing = false;
                               e.SecondLink.Options.RequireSelfProcessing = e.SecondLink.Value.RequireSelfProcessing = false;
                               e.ThirdLink.Options.RequireSelfProcessing = e.ThirdLink.Value.RequireSelfProcessing = false;
                           });
                       });
                });

            TestModel(env.SiteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.FieldLinks")]
        public void CanDeploy_CanSetupUniqueContentTypeFieldsOrder()
        {
            var env = GetContentTypeSandbox(
                (siteModel, e) =>
                {

                },
                (contentTypeModel, e) =>
                {
                    contentTypeModel
                       .AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                       {

                           Fields = new List<FieldLinkValue>
                           {
                                          new FieldLinkValue { InternalName = e.Second.InternalName },
                                          new FieldLinkValue { InternalName = e.First.InternalName },
                           }
                       });
                });

            TestModel(env.SiteModel);
        }

        // 

        #endregion

        #region doc templates



        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.DocumentTemplate")]
        public void CanDeploy_ContentType_WithDocTemplate_In_ResourceFolder()
        {
            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();
            var documentTemplate = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Content = DocumentTemplates.SPMeta2_MSWord_Template;
                def.FileName = Rnd.String() + ".dotx";
            });


            siteContentType.DocumentTemplate = documentTemplate.FileName;

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(siteContentType, contentType =>
                {
                    contentType.AddModuleFile(documentTemplate);
                });
            });

            var contentModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddContentTypeLink(siteContentType);
                });
            });

            TestModels(new[] { siteModel, contentModel });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.DocumentTemplate")]
        public void CanDeploy_ContentType_WithDocTemplate_In_RootWeb_DocumentLibrary()
        {
            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var documentTemplate = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Content = DocumentTemplates.SPMeta2_MSWord_Template;
                def.FileName = Rnd.String() + ".dotx";
            });

            var documentTemplateLibrary = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            siteContentType.DocumentTemplate = UrlUtility.CombineUrl(new[]{
               "~sitecollection", 
               documentTemplateLibrary.Url,
               documentTemplate.FileName 
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(siteContentType);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(documentTemplateLibrary, list =>
                {
                    list.AddModuleFile(documentTemplate);
                });
            });

            var contentModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddContentTypeLink(siteContentType);
                });
            });

            TestModels(new[] { webModel, siteModel, contentModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.DocumentTemplate")]
        public void CanDeploy_ContentType_WithDocTemplate_In_SubWeb_DocumentLibrary()
        {
            var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var documentTemplate = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Content = DocumentTemplates.SPMeta2_MSWord_Template;
                def.FileName = Rnd.String() + ".dotx";
            });

            var documentTemplateLibrary = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var subWebDef = ModelGeneratorService.GetRandomDefinition<WebDefinition>();

            siteContentType.DocumentTemplate = UrlUtility.CombineUrl(new[]{
               "~sitecollection", 
               subWebDef.Url,
               documentTemplateLibrary.Url,
               documentTemplate.FileName 
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(siteContentType);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(subWebDef, subWeb =>
                {
                    subWeb.AddList(documentTemplateLibrary, list =>
                    {
                        list.AddModuleFile(documentTemplate);
                    });
                });


            });

            var contentModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomDocumentLibrary(list =>
                {
                    list.AddContentTypeLink(siteContentType);
                });
            });

            TestModels(new[] { webModel, siteModel, contentModel });
        }

        #endregion
    }
}
