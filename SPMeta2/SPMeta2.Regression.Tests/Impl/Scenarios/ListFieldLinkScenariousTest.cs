using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using SPMeta2.Containers;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListFieldLinkScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        public ListFieldLinkScenariousTest()
        {
            this.EnablePropertyUpdateValidation = false;
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
                    list.AddListFieldLink(new ListFieldLinkDefinition()
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
                    list.AddListFieldLink(new ListFieldLinkDefinition()
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
                    list.AddListFieldLink(new ListFieldLinkDefinition()
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

            TestModels(new[] { siteModel, webModel });
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

            var ct_1 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var ct_2 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var ct_3 = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
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
                       site.AddContentType(ct_1);
                       site.AddContentType(ct_2);
                       site.AddContentType(ct_3);

                       site.AddField(field);
                   });

            var webModel = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web.AddList(genericList, list =>
                       {
                           list.AddContentTypeLink(ct_1);
                           list.AddContentTypeLink(ct_2);
                           list.AddContentTypeLink(ct_3);

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

            TestModels(new[] { siteModel, webModel });
        }

        #endregion
    }
}
