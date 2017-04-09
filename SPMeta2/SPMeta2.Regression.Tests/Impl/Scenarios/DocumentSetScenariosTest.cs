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
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class DocumentSetScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.DocumentSet")]
        public void CanDeploy_DocumentSet_To_List_With_ContentTypeId()
        {
            var docSetDef = ModelGeneratorService.GetRandomDefinition<DocumentSetDefinition>(def =>
            {
                def.ContentTypeId = BuiltInContentTypeId.DocumentSet_Correct;
                def.ContentTypeName = null;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.DocumentSets.Inherit(f =>
                {
                    f.Enable = true;
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(BuiltInContentTypeId.DocumentSet_Correct);
                    list.AddDocumentSet(docSetDef);
                });
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.DocumentSet")]
        public void CanDeploy_DocumentSet_To_Folder_With_ContentTypeId()
        {
            var docSetDef = ModelGeneratorService.GetRandomDefinition<DocumentSetDefinition>(def =>
            {
                def.ContentTypeId = BuiltInContentTypeId.DocumentSet_Correct;
                def.ContentTypeName = null;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.DocumentSets.Inherit(f =>
                {
                    f.Enable = true;
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(BuiltInContentTypeId.DocumentSet_Correct);

                    list.AddRandomFolder(folder =>
                    {
                        folder.AddDocumentSet(docSetDef);
                    });
                });
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.DocumentSet")]
        public void CanDeploy_DocumentSet_To_List_With_ContentTypeName()
        {
            var docSetDef = ModelGeneratorService.GetRandomDefinition<DocumentSetDefinition>(def =>
            {
                def.ContentTypeId = null;
                def.ContentTypeName = BuiltInContentTypeNames.DocumentSet_Correct;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.DocumentSets.Inherit(f =>
                {
                    f.Enable = true;
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(BuiltInContentTypeId.DocumentSet_Correct);

                    list.AddDocumentSet(docSetDef);
                });
            });

            TestModel(siteModel, model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.DocumentSet")]
        public void CanDeploy_DocumentSet_To_Folder_With_ContentTypeName()
        {
            var docSetDef = ModelGeneratorService.GetRandomDefinition<DocumentSetDefinition>(def =>
            {
                def.ContentTypeId = null;
                def.ContentTypeName = BuiltInContentTypeNames.DocumentSet_Correct;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.DocumentSets.Inherit(f =>
                {
                    f.Enable = true;
                }));
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(BuiltInContentTypeId.DocumentSet_Correct);

                    list.AddRandomFolder(folder =>
                    {
                        folder.AddDocumentSet(docSetDef);
                    });
                });
            });

            TestModel(siteModel, model);
        }

        #endregion
    }
}
