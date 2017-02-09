using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Data;
using SPMeta2.Regression.Tests.Prototypes;
using SPMeta2.Syntax.Extended;

using SPMeta2.Containers.Extensions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ModuleFileScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public ModuleFileScenariousTest()
        {
            RegressionService.ProvisionGenerationCount = 2;
        }

        #endregion

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

        #region content types and values


        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Typed")]
        public void CanDeploy_ModuleFile_AsJavaScriptDisplayTemplate()
        {
            var moduleFile = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.ContentTypeName = "JavaScript Display Template";
                def.DefaultValues = new List<FieldValue>
                {
                    new FieldValue
                    {
                        FieldName = "DisplayTemplateJSTargetControlType",
                        Value = "Form"
                    },
                     new FieldValue
                    {
                        FieldName = "DisplayTemplateJSTargetScope",
                        Value = Rnd.String()
                    },
                     new FieldValue
                    {
                        FieldName = "DisplayTemplateJSTemplateType",
                        Value ="Override"
                    },
                    
                };
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                {
                    list.AddModuleFile(moduleFile);
                });
            });

            TestModel(webModel);
        }

        #endregion

        #region size

        protected void DeployModuleFile(long lenght)
        {
            TestRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Content = Rnd.Content(lenght);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Size")]
        public void CanDeploy_2Mb_ModuleFile()
        {
            DeployModuleFile(1024 * 1024 * 2);
        }

        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_5Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 5);
        //}


        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_15Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 15);
        //}

        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_25Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 25);
        //}

        //[TestMethod]
        //[TestCategory("Regression.Rnd.List")]
        //public void CanDeploy_50Mb_ModuleFile()
        //{
        //    DeployModuleFile(1024 * 1024 * 50);
        //}

        #endregion

        #region hosts


        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToLibrary()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomModuleFile();
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomModuleFile();
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.OOTBLists")]
        public void CanDeploy_ModuleFile_ToStyleLibrary()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddHostStyleLibraryList(rndList =>
                    {
                        rndList.AddRandomModuleFile();
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts.Lists")]
        public void CanDeploy_ModuleFile_ToGenericList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            TestModuleFileDeploymentToList(list);
        }


        private void TestModuleFileDeploymentToList(ListDefinition list)
        {
            var blogSite = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.Blog;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddWeb(blogSite, blogWeb =>
                    {
                        blogWeb.AddList(list, rndList =>
                        {
                            rndList.AddModuleFile(new ModuleFileDefinition
                            {
                                FileName = "AllItems.aspx",
                                Content = Encoding.Default.GetBytes(PageTemplates.CustomAllItemsPage),
                                Overwrite = true
                            });

                            rndList.AddWelcomePage(new WelcomePageDefinition
                            {
                                Url = "AllItems.aspx"
                            });
                        });
                    });

                });

            TestModels(new ModelNode[] { model });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts.Lists")]
        public void CanDeploy_ModuleFile_ToIssueTrackingList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.IssueTracking;
            });

            TestModuleFileDeploymentToList(list);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts.Blog")]
        public void CanDeploy_ModuleFile_ToPostList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.CustomUrl = "Lists/Posts";

                def.ContentTypesEnabled = false;

                def.TemplateType = 0;
                def.TemplateName = BuiltInListTemplates.Posts.InternalName;
            });

            TestModuleFileDeploymentToList(list);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts.Blog")]
        public void CanDeploy_ModuleFile_ToCommentsList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.CustomUrl = "Lists/Comments";

                def.ContentTypesEnabled = false;

                def.TemplateType = 0;
                def.TemplateName = BuiltInListTemplates.Comments.InternalName;
            });

            TestModuleFileDeploymentToList(list);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts.Blog")]
        public void CanDeploy_ModuleFile_ToCategoriesList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.CustomUrl = "Lists/Categories";

                def.ContentTypesEnabled = false;

                def.TemplateType = 0;
                def.TemplateName = BuiltInListTemplates.Categories.InternalName;
            });

            TestModuleFileDeploymentToList(list);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToLibraryFolder()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomFolder(folder =>
                        {
                            folder.AddRandomModuleFile();
                        });
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToLibrary_FormsFolder()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddHostFolder(BuiltInFolderDefinitions.Forms, folder =>
                        {
                            folder.AddRandomModuleFile();
                        });
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToWeb()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomModuleFile();
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Hosts")]
        public void CanDeploy_ModuleFile_ToContentType()
        {
            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddRandomContentType(contentType =>
                    {
                        contentType.AddRandomModuleFile();
                    });
                });

            TestModel(model);
        }

        #endregion

        #region field values


        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Values")]
        public void CanDeploy_ModuleFile_With_RequiredFieldValues()
        {
            var requiredText = RItemValues.GetRequiredTextField(ModelGeneratorService);

            var text1 = RItemValues.GetRandomTextField(ModelGeneratorService);
            var text2 = RItemValues.GetRandomTextField(ModelGeneratorService);

            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var itemDef = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.Title = Rnd.String();
                def.ContentTypeName = contentTypeDef.Name;

                def.DefaultValues.Add(new FieldValue()
                {
                    FieldName = requiredText.InternalName,
                    Value = Rnd.String()
                });

                def.Values.Add(new FieldValue()
                {
                    FieldName = text1.InternalName,
                    Value = Rnd.String()
                });

                def.Values.Add(new FieldValue()
                {
                    FieldName = text2.InternalName,
                    Value = Rnd.String()
                });

            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(requiredText);
                site.AddField(text1);
                site.AddField(text2);

                site.AddContentType(contentTypeDef, contentType =>
                {
                    contentType.AddContentTypeFieldLink(requiredText);
                    contentType.AddContentTypeFieldLink(text1);
                    contentType.AddContentTypeFieldLink(text2);
                });
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddModuleFile(itemDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Values")]
        public void CanDeploy_ModuleFile_With_ContentType_ByName()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Document;
            });

            var itemDef = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
            {
                def.ContentTypeName = contentTypeDef.Name;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
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
                    list.AddModuleFile(itemDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        #endregion


    }

    [TestClass]
    public class ModuleFileVersioningScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public ModuleFileVersioningScenariousTest()
        {
            RegressionService.ProvisionGenerationCount = 1;

            RegressionService.EnableDefinitionValidation = false;
            RegressionService.EnableEventValidation = false;
        }

        #endregion

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

        #region versioning

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Versioning")]
        public void CanDeploy_ModuleFile_MoreThan_511_Times_NoModeration()
        {
            if (!TestOptions.EnableModuleFile511Tests)
                return;

            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.EnableVersioning = true;
                def.EnableMinorVersions = true;

                def.EnableModeration = false;

                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            CanDeploy_ModuleFile_MoreThan_511_Times(list);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ModuleFiles.Versioning")]
        public void CanDeploy_ModuleFile_MoreThan_511_Times_WithModeration()
        {
            if (!TestOptions.EnableModuleFile511Tests)
                return;

            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.EnableVersioning = true;
                def.EnableMinorVersions = true;

                def.EnableModeration = true;

                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            CanDeploy_ModuleFile_MoreThan_511_Times(list);
        }

        public void CanDeploy_ModuleFile_MoreThan_511_Times(ListDefinition list)
        {
            // Module file provision fails at minor version 511 #930
            // https://github.com/SubPointSolutions/spmeta2/issues/930                        

            var moduleFileName = string.Format("{0}.txt", Rnd.String());
            var moduleFiles = new List<ModuleFileDefinition>();

            var filesCount = 520;

            for (var i = 0; i < filesCount; i++)
            {
                var moduleFileDef = ModelGeneratorService.GetRandomDefinition<ModuleFileDefinition>(def =>
                {
                    def.FileName = moduleFileName;
                    def.Content = new byte[3] { 1, 2, 3 };
                });

                moduleFiles.Add(moduleFileDef);
            }

            // RegExcludeFromValidation - exclude all defs from the validation
            // don't need to check them as we test ability to perfrom more than 511 updates

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(list, rndList =>
                {
                    rndList.RegExcludeFromValidation();

                    foreach (var moduleFile in moduleFiles)
                    {
                        rndList.AddModuleFile(moduleFile, file =>
                        {
                            file.RegExcludeFromValidation();
                        });
                    }
                });

            });

            TestModel(model, true);
        }

        #endregion
    }
}
