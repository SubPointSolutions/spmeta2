using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListFieldLinkScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        public ListFieldLinkScenariosTest()
        {
            EnablePropertyUpdateValidation = false;
        }

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

        #region by id or by internla name

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListFieldLinkDefinition.IdOrName")]
        public void CanDeploy_ListFieldLink_WithFieldId()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddListFieldLink(new ListFieldLinkDefinition
                    {
                        FieldId = BuiltInFieldId.URL
                    });
                });
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListFieldLinkDefinition.IdOrName")]
        public void CanDeploy_ListFieldLink_WithFieldInternalName()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddListFieldLink(new ListFieldLinkDefinition
                    {
                        FieldInternalName = BuiltInInternalFieldNames.URL
                    });
                });
            });

            TestModel(webModel);
        }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_ListFieldLink_RenameTitle()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddListFieldLink(new ListFieldLinkDefinition
                    {
                        FieldId = BuiltInFieldId.Title,
                        DisplayName = Rnd.String()
                    });
                });
            });

            TestModel(webModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Scopes")]
        public void CanDeploy_ListFieldLink_WithSiteAndWebFields()
        {
            var siteField = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            var webField = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(siteField);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWeb(subWeb =>
                {
                    subWeb.AddField(webField);
                    subWeb.AddRandomList(list =>
                    {
                        list.AddListFieldLink(siteField);
                        list.AddListFieldLink(webField);
                    });
                });
            });

            TestModel(siteModel, webModel);
        }

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

            TestModels(new ModelNode[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Options")]
        public void CanDeploy_ListFieldLink_AsAddToDefaultView_ViaAddFieldOptions()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            field.AddFieldOptions = BuiltInAddFieldOptions.AddFieldToDefaultView;

            var listFieldLink = new ListFieldLinkDefinition
            {
                FieldId = field.Id,
                AddFieldOptions = BuiltInAddFieldOptions.AddFieldToDefaultView
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

            TestModels(new ModelNode[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Options")]
        public void CanDeploy_ListFieldLink_AsAddToAllContentTypes()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            field.AddToDefaultView = true;

            var listFieldLink = new ListFieldLinkDefinition
            {
                FieldId = field.Id,
                AddFieldOptions = BuiltInAddFieldOptions.AddToAllContentTypes,
                AddToDefaultView = true
            };

            var ct1 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var ct2 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var ct3 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var genericList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var siteModel = SPMeta2Model
                   .NewSiteModel(site =>
                   {
                       site.AddContentType(ct1);
                       site.AddContentType(ct2);
                       site.AddContentType(ct3);

                       site.AddField(field);
                   });

            var webModel = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddList(genericList, list =>
                       {
                           list.AddContentTypeLink(ct1);
                           list.AddContentTypeLink(ct2);
                           list.AddContentTypeLink(ct3);

                           list.AddListFieldLink(listFieldLink);
                       });
                   });

            // content types are deployed after fields
            // so, to test AddToAllContentTypes, we need to deploy content type first, and then the rest
            //var fieldLinkModel = SPMeta2Model
            //       .NewWebModel(web =>
            //       {
            //           web.AddHostList(genericList, list =>
            //           {

            //           });
            //       });

            TestModels(new ModelNode[] { siteModel, webModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.Options")]
        public void CanDeploy_ListFieldLink_MultipleAddFieldOption()
        {
            var field = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
            field.AddFieldOptionList = new List<BuiltInAddFieldOptions>
            {
                BuiltInAddFieldOptions.AddFieldToDefaultView,
                BuiltInAddFieldOptions.AddFieldInternalNameHint,
                BuiltInAddFieldOptions.AddToAllContentTypes
            };

            var listFieldLink = new ListFieldLinkDefinition
            {
                FieldId = field.Id,
                AddFieldOptionList = new List<BuiltInAddFieldOptions>
                {
                BuiltInAddFieldOptions.AddFieldToDefaultView,
                BuiltInAddFieldOptions.AddFieldInternalNameHint,
                BuiltInAddFieldOptions.AddToAllContentTypes
                }
            };

            var ct1 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var ct2 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var ct3 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var genericList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var siteModel = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddContentType(ct1);
                    site.AddContentType(ct2);
                    site.AddContentType(ct3);

                    site.AddField(field);
                });

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(genericList, list =>
                    {
                        list.AddContentTypeLink(ct1);
                        list.AddContentTypeLink(ct2);
                        list.AddContentTypeLink(ct3);

                        list.AddListFieldLink(listFieldLink);
                    })
                    .AddRandomList(list =>
                    {
                        list.AddListFieldLink(listFieldLink);
                    });
                });

            TestModels(new ModelNode[] { siteModel, webModel });
        }
    }

    #endregion
}
