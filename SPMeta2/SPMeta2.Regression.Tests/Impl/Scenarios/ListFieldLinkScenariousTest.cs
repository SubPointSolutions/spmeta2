using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // add ootb list field link, TODO
        // add ootb list field links, TODO

        // add custom list field link, TODO
        // add custom list field links, TODO

        // add ootb AND custom list field links, TODO

        #endregion
    }
}
