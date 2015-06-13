using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using System.Collections.Generic;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Dependencies
{
    [TestClass]
    public class ArtifactDependenciesTest : SPMeta2RegresionTestBase
    {
        public ArtifactDependenciesTest()
        {
            RegressionService.ProvisionGenerationCount = 2;
        }

        #region common

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        //[TestMethod]
        //[TestCategory("Regression.Dependencies")]
        //public void SiteFields_Before_SiteContentTypes()
        //{
        //    var fieldDefinitionTypes = new List<Type>();

        //    // foundation defs
        //    fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(FieldDefinition).Assembly));

        //    // standard defs
        //    fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(TaxonomyFieldDefinition).Assembly));

        //    var fieldDefinitions = new List<FieldDefinition>();

        //    foreach (var fieldDefinitionType in fieldDefinitionTypes)
        //        fieldDefinitions.Add(ModelGeneratorService.GetRandomDefinition(fieldDefinitionType) as FieldDefinition);

        //    var contentTypeDefinition = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

        //    var siteModel = SPMeta2Model
        //        .NewSiteModel(site =>
        //        {
        //            foreach (var fieldDefinition in fieldDefinitions)
        //                site.AddField(fieldDefinition);

        //            site.AddContentType(contentTypeDefinition, contentType =>
        //            {
        //                foreach (var fieldDefinition in fieldDefinitions)
        //                    contentType.AddContentTypeFieldLink(fieldDefinition);
        //            });
        //        });

        //    TestModel(siteModel);
        //}

        //[TestMethod]
        //[TestCategory("Regression.Dependencies")]
        //public void ListContentTypes_Before_ListViews()
        //{
        //    var siteField = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();
        //    var siteContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

        //    var webList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
        //    {
        //        def.ContentTypesEnabled = true;
        //    });
        //    var webListView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
        //    {
        //        def.Fields = new Collection<string>
        //        {
        //            siteField.InternalName
        //        };
        //    });

        //    var siteModel = SPMeta2Model
        //        .NewSiteModel(site =>
        //        {
        //            site.AddField(siteField);
        //            site.AddContentType(siteContentType, contentType =>
        //            {
        //                contentType.AddContentTypeFieldLink(siteField);
        //            });
        //        });

        //    var webModel = SPMeta2Model
        //       .NewWebModel(site =>
        //       {
        //           site.AddList(webList, list =>
        //           {
        //               list.AddContentTypeLink(siteContentType);
        //               list.AddView(webListView);
        //           });
        //       });

        //    TestModels(new[] { siteModel, webModel });
        //}

        protected void EnsureListFieldScopedWeigh()
        {
            var listWeight = DefaultModelWeigh.Weighs.FirstOrDefault(w => w.Model == typeof(ListDefinition));

            if (!listWeight.ChildModels.ContainsKey(typeof(FieldDefinition)))
                listWeight.ChildModels.Add(typeof(FieldDefinition), 50);
        }

        [TestMethod]
        [TestCategory("Regression.Dependencies")]
        public void ListFields_Before_ListViews()
        {
            var useListScopedeFix = true;

            if (useListScopedeFix)
            {
                EnsureListFieldScopedWeigh();
                EnsureListFieldScopedWeigh();
            }

            var listField = ModelGeneratorService.GetRandomDefinition<FieldDefinition>();

            var webList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
            });
            var webListView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Fields = new Collection<string>
                {
                    listField.InternalName
                };
            });

            var webModel = SPMeta2Model
               .NewWebModel(site =>
               {
                   site.AddList(webList, list =>
                   {
                       list.AddField(listField);
                       list.AddView(webListView);
                   });
               });

            TestModels(new[] { webModel });
        }

        #endregion
    }
}
