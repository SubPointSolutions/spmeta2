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
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Services;
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
        public void CanDeploy_CustomListItemContentType_ByParentName()
        {
            TestRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = string.Empty;
                def.ParentContentTypeName = BuiltInContentTypeNames.Item;
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

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes")]
        public void CanDeploy_CustomDocumentContentType_ByParentName()
        {
            TestRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = string.Empty;
                def.ParentContentTypeName = BuiltInContentTypeNames.Document;
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

            public SiteModelNode SiteModel { get; set; }
        }

        private ContentTypeEnvironment GetContentTypeSandbox(
            Action<SiteModelNode, ContentTypeEnvironment> siteModelConfig,
            Action<ContentTypeModelNode, ContentTypeEnvironment> contentTypeModelConfig)
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
                                     //link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                     link.Options.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeFieldLink(fldSecond, link =>
                                 {
                                     result.SecondLink = link;
                                     //link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                     link.Options.RequireSelfProcessing = true;
                                 })
                                 .AddContentTypeFieldLink(fldThird, link =>
                                 {
                                     result.ThirdLink = link;
                                     //link.Options.RequireSelfProcessing = link.Value.RequireSelfProcessing = true;
                                     link.Options.RequireSelfProcessing = true;
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

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.FieldLinks")]
        public void CanDeploy_CanSetupUniqueContentTypeFieldsOrder_At_OOTB_List_Scope()
        {
            var fieldDef = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.Hidden = false;

                def.ShowInDisplayForm = true;
                def.ShowInEditForm = true;
                def.ShowInListSettings = true;
                def.ShowInNewForm = true;
                def.ShowInViewForms = true;

                def.AddFieldOptions = BuiltInAddFieldOptions.AddToAllContentTypes
                    | BuiltInAddFieldOptions.AddFieldInternalNameHint;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = false;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var contentTypeLinkDef = new ContentTypeLinkDefinition
            {
                ContentTypeName = BuiltInContentTypeNames.Item,
                ContentTypeId = BuiltInContentTypeId.Item
            };

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                // adding field first
                web.AddList(listDef, list =>
                {
                    list.AddField(fieldDef, field =>
                    {

                    });
                });

                // then working with the content type
                web.AddList(listDef.Inherit(), list =>
                {
                    list.AddContentTypeLink(contentTypeLinkDef, contenTypeLink =>
                    {
                        contenTypeLink.RegExcludeFromEventsValidation();

                        contenTypeLink.AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                        {

                            Fields = new List<FieldLinkValue>
                            {
                                    new FieldLinkValue {InternalName = fieldDef.InternalName},
                                    new FieldLinkValue {InternalName = "Title"},
                            }
                        });
                    });
                });
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.FieldLinks")]
        public void CanDeploy_CanSetupUniqueContentTypeFieldsOrder_At_List_Scope()
        {
            // Support 'UniqueContentTypeFieldsOrderDefinition' at list level content types #742
            // https://github.com/SubPointSolutions/spmeta2/issues/742

            WithDisabledPropertyUpdateValidation(() =>
            {
                var first = string.Empty;
                var second = string.Empty;

                var env = GetContentTypeSandbox(
                    (siteModel, e) =>
                    {

                    },
                    (contentTypeModel, e) =>
                    {
                        first = e.First.InternalName;
                        second = e.Second.InternalName;

                        contentTypeModel
                            .AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                            {
                                Fields = new List<FieldLinkValue>
                                {
                                    new FieldLinkValue {InternalName = e.Second.InternalName},
                                    new FieldLinkValue {InternalName = e.First.InternalName},
                                }
                            });
                    });

                var webModel = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddRandomList(list =>
                    {
                        (list.Value as ListDefinition).ContentTypesEnabled = true;

                        list.AddContentTypeLink(env.ContentType, contenTypeLink =>
                        {
                            contenTypeLink.AddUniqueContentTypeFieldsOrder(new UniqueContentTypeFieldsOrderDefinition
                            {

                                Fields = new List<FieldLinkValue>
                                {
                                    new FieldLinkValue {InternalName = first},
                                    new FieldLinkValue {InternalName = second},
                                }
                            });
                        });
                    });
                });

                TestModel(env.SiteModel, webModel);
            });
        }

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

            TestModels(new ModelNode[] { siteModel, contentModel });
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
#pragma warning disable 618
               documentTemplateLibrary.Url,
#pragma warning restore 618
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

            TestModels(new ModelNode[] { webModel, siteModel, contentModel });
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
#pragma warning disable 618
               documentTemplateLibrary.Url,
#pragma warning restore 618
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

            TestModels(new ModelNode[] { webModel, siteModel, contentModel });
        }

        #endregion

        #region localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.Localization")]
        public void CanDeploy_Localized_Site_ContentType()
        {
            var definition = GetLocalizedDefinition();

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(definition);
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.Localization")]
        public void CanDeploy_Localized_Web_ContentType()
        {
            var contentType = GetLocalizedDefinition();
            var subWebContentType = GetLocalizedDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddContentType(contentType);

                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddContentType(subWebContentType);
                });
            });

            TestModel(model);
        }

        #endregion

        #region renaming

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.Renaming")]
        public void CanDeploy_ContentType_And_Rename()
        {
            // Enhance ContentTypeDefinition - support Name renaming #924
            // https://github.com/SubPointSolutions/spmeta2/issues/924

            // we should be able to deploy and then rename content types
            // 3 waves of deployment: original, and two renames
            // all should work, including model validations with the new Names

            // we need to add content type field links to ensure two paths:
            // 1) model handler provisions content types
            // 2) model handler resolves content type to pass further to the provision flow

            var originalContentTypes = new List<ContentTypeDefinition>();

            originalContentTypes.Add(ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>());
            originalContentTypes.Add(ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>());

            var firstRenames = originalContentTypes.Select(d =>
            {
                return d.Inherit(def =>
                {
                    def.Name = Rnd.String();
                });
            });

            var secondRenames = originalContentTypes.Select(d =>
            {
                return d.Inherit(def =>
                {
                    def.Name = Rnd.String();
                });
            });

            // deploy and test original content types
            var originalModel = SPMeta2Model.NewSiteModel(site =>
            {
                foreach (var ct in originalContentTypes)
                {
                    site.AddContentType(ct, contentType =>
                    {
                        contentType.AddContentTypeFieldLink(BuiltInFieldId.Title);
                    });
                }
            });

            TestModel(originalModel);

            // deploy and test first renames
            var firstRenamesModel = SPMeta2Model.NewSiteModel(site =>
            {
                foreach (var ct in firstRenames)
                {
                    site.AddContentType(ct, contentType =>
                    {
                        contentType.AddContentTypeFieldLink(BuiltInFieldId.Title);
                    });
                }
            });

            TestModel(firstRenamesModel);

            // deploy and test first renames
            var secondRenamesModel = SPMeta2Model.NewSiteModel(site =>
            {
                foreach (var ct in secondRenames)
                {
                    site.AddContentType(ct, contentType =>
                    {
                        contentType.AddContentTypeFieldLink(BuiltInFieldId.Title);
                    });
                }
            });

            TestModel(secondRenamesModel);
        }

        #endregion

        #region utils

        protected ContentTypeDefinition GetLocalizedDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();
            var localeIds = Rnd.LocaleIds();

            foreach (var localeId in localeIds)
            {
                definition.NameResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedName_{0}", localeId)
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

        #region addint out of the box content types

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.OOTB")]
        public void CanDeploy_Item_ContentType_To_List()
        {
            // "Item" ContentTypeLink #1016
            // https://github.com/SubPointSolutions/spmeta2/issues/1016

            var announcementList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Announcements;
                def.ContentTypesEnabled = true;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(announcementList, list =>
                {
                    list.AddContentTypeLink(new ContentTypeLinkDefinition
                    {
                        ContentTypeId = BuiltInContentTypeId.Item,
                        ContentTypeName = "Item"
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes.OOTB")]
        public void CanDeploy_Item_ContentType_To_Library()
        {
            // "Item" ContentTypeLink #1016
            // https://github.com/SubPointSolutions/spmeta2/issues/1016

            var announcementList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
                def.ContentTypesEnabled = true;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(announcementList, list =>
                {
                    list.AddContentTypeLink(new ContentTypeLinkDefinition
                    {
                        ContentTypeId = BuiltInContentTypeId.Item,
                        ContentTypeName = "Item"
                    });
                });
            });

            TestModel(model);
        }

        #endregion
    }
}
