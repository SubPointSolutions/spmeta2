using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FolderScenariousTest : SPMeta2RegresionScenarioTestBase
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

        protected void AttachFolderHierarchyToFolder(FolderModelNode node)
        {
            node
                .AddRandomFolder(rootFolder =>
                {
                    rootFolder
                         .AddRandomFolder(subFolder =>
                         {
                             subFolder
                                 .AddRandomFolder()
                                 .AddRandomFolder();
                         })
                        .AddRandomFolder(subFolder =>
                        {
                            subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                        });
                })
                .AddRandomFolder(rootFolder =>
                {
                    rootFolder
                        .AddRandomFolder(subFolder =>
                        {
                            subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                        })
                       .AddRandomFolder(subFolder =>
                       {
                           subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                       });
                });
        }

        protected void AttachFolderHierarchyToList(ListModelNode node)
        {
            node
                .AddRandomFolder(rootFolder =>
                {
                    rootFolder
                         .AddRandomFolder(subFolder =>
                         {
                             subFolder
                                 .AddRandomFolder()
                                 .AddRandomFolder();
                         })
                        .AddRandomFolder(subFolder =>
                        {
                            subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                        });
                })
                .AddRandomFolder(rootFolder =>
                {
                    rootFolder
                        .AddRandomFolder(subFolder =>
                        {
                            subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                        })
                       .AddRandomFolder(subFolder =>
                       {
                           subFolder
                                .AddRandomFolder()
                                .AddRandomFolder();
                       });
                });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders")]
        public void CanDeploy_FolderHierarchy_InList()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, list => AttachFolderHierarchyToList(list));
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders.ContentType")]
        public void CanDeploy_FolderWithContentTypeId_InList()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Folder;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var folderDef = ModelGeneratorService.GetRandomDefinition<FolderDefinition>(def =>
            {
                def.ContentTypeId = contentTypeDef.GetContentTypeId();
                def.ContentTypeName = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddFolder(folderDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders.ContentType")]
        public void CanDeploy_FolderWithContentTypeName_InList()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Folder;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var folderDef = ModelGeneratorService.GetRandomDefinition<FolderDefinition>(def =>
            {
                def.ContentTypeId = string.Empty;
                def.ContentTypeName = contentTypeDef.Name;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddFolder(folderDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders")]
        public void CanDeploy_FolderHierarchy_InLibrary()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, list => AttachFolderHierarchyToList(list));
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders.ContentType")]
        public void CanDeploy_FolderWithContentTypeId_InLibrary()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Folder;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var folderDef = ModelGeneratorService.GetRandomDefinition<FolderDefinition>(def =>
            {
                def.ContentTypeId = contentTypeDef.GetContentTypeId();
                def.ContentTypeName = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddFolder(folderDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Folders.ContentType")]
        public void CanDeploy_FolderWithContentTypeName_InLibrary()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Folder;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var folderDef = ModelGeneratorService.GetRandomDefinition<FolderDefinition>(def =>
            {
                def.ContentTypeId = string.Empty;
                def.ContentTypeName = contentTypeDef.Name;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddFolder(folderDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        #endregion
    }
}
