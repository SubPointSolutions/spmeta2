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

using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Utils;
using SPMeta2.Containers.Templates.Documents;
using SPMeta2.Standard.Enumerations;

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

        #endregion

        #region utils

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
