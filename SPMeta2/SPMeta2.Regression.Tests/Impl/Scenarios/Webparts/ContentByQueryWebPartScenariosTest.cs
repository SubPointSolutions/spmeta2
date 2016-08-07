using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard;
using SPMeta2.CSOM;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Exceptions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Extensions;
using System.Collections.Generic;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Definitions;
using SPMeta2.Services;
using SPMeta2.ModelHandlers;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Regression.Definitions.Extended;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Standard.Syntax;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Standard.Services.Webparts;
using SPMeta2.Standard.BuiltInDefinitions;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Webparts
{
    [TestClass]
    public class ContentByQueryWebPartScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors



        #endregion

        #region internal

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

        #endregion

        #region list bindings

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.Lists")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_BindedToList_ByUrl()
        {
            var webpartDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is to " + BuiltInListDefinitions.WorkflowTasks.CustomUrl;

                def.WebUrl = "~sitecollection";

                def.ListId = Guid.Empty;
                def.ListUrl = BuiltInListDefinitions.WorkflowTasks.CustomUrl;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(webpartDef);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.Lists")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_BindedToLibrary_ByUrl()
        {
            var webpartDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is to " + BuiltInListDefinitions.StyleLibrary.CustomUrl;

                def.WebUrl = "~sitecollection";

                def.ListId = Guid.Empty;
                def.ListUrl = BuiltInListDefinitions.StyleLibrary.CustomUrl;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(webpartDef);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region list types

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_ListTemplateType_Posts()
        {
            var templateTypeId = BuiltInListTemplateTypeId.Posts;

            var webpartDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId;

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(webpartDef);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_ListTemplateType_GenericList()
        {
            var templateTypeId = BuiltInListTemplateTypeId.GenericList;

            var webpartDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId;

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(webpartDef);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_ListTemplateType_DocumentLibrary()
        {
            var templateTypeId = BuiltInListTemplateTypeId.DocumentLibrary;

            var webpartDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId;

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(webpartDef);
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_ListTemplateType_AssetLibrary()
        {
            var templateTypeId = BuiltInListTemplateTypeId.AssetLibrary;

            var webpartDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId;

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(webpartDef);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region asc-desc

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_Sorting()
        {
            var templateTypeId = BuiltInListTemplateTypeId.AssetLibrary;

            var ascDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and sort Asc";

                def.ServerTemplate = templateTypeId;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Asc";
            });

            var descDef = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and sort Desc";

                def.ServerTemplate = templateTypeId;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(ascDef);
                        page.AddContentByQueryWebPart(descDef);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region limit

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_Limit()
        {
            var templateTypeId = BuiltInListTemplateTypeId.Posts;

            var defLimit1 = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and limit 1";

                def.ServerTemplate = templateTypeId;

                def.ItemLimit = 1;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Asc";
            });

            var defLimit3 = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and limit 3";

                def.ServerTemplate = templateTypeId;

                def.ItemLimit = 3;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";
            });


            var defLimit5 = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and limit 5";

                def.ServerTemplate = templateTypeId;

                def.ItemLimit = 5;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";
            });


            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(defLimit1);
                        page.AddContentByQueryWebPart(defLimit3);
                        page.AddContentByQueryWebPart(defLimit5);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region styling

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_Styles()
        {
            var templateTypeId = BuiltInListTemplateTypeId.Posts;

            var defDefault = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and item style Default";

                def.ServerTemplate = templateTypeId;

                def.ItemStyle = BuiltInItemStyleNames.Default;
                def.ItemLimit = 3;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";
            });


            var defBullets = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and item style Bullets";

                def.ServerTemplate = templateTypeId;

                def.ItemStyle = BuiltInItemStyleNames.Bullets;
                def.ItemLimit = 3;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";
            });


            var defTitleOnly = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and item style TitleOnly";

                def.ServerTemplate = templateTypeId;

                def.ItemStyle = BuiltInItemStyleNames.TitleOnly;
                def.ItemLimit = 3;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";
            });

            var defNoImage = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
          {
              def.Title = "As is with template type " + templateTypeId + " and item style NoImage";

              def.ServerTemplate = templateTypeId;

              def.ItemStyle = BuiltInItemStyleNames.NoImage;
              def.ItemLimit = 3;

              def.SortByFieldType = "DateTime";
              def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
              def.SortByDirection = "Desc";
          });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(defNoImage);
                        page.AddContentByQueryWebPart(defBullets);
                        page.AddContentByQueryWebPart(defDefault);
                        page.AddContentByQueryWebPart(defTitleOnly);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region mics props

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.Mics")]
        //public void CanDeploy_ContentByQueryWebPart_AsIs_With_MicsProps()
        //{
        //    var templateTypeId = BuiltInListTemplateTypeId.AssetLibrary;

        //    var misc1 = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
        //    {
        //        def.Title = "As is with template type " + templateTypeId + " and sort Asc";

        //        def.ServerTemplate = templateTypeId;

        //        def.PlayMediaInBrowser = Rnd.Bool();
        //        def.ShowUntargetedItems = Rnd.Bool();
        //        def.UseCopyUtil = Rnd.Bool();
        //    });

        //    var misc2 = misc1.Inherit(def =>
        //    {
        //        def.PlayMediaInBrowser = !def.PlayMediaInBrowser;
        //        def.ShowUntargetedItems = !def.ShowUntargetedItems;
        //        def.UseCopyUtil = !def.UseCopyUtil;
        //    });

        //    var model = SPMeta2Model.NewWebModel(web =>
        //    {
        //        web.AddHostList(BuiltInListDefinitions.SitePages, list =>
        //        {
        //            list.AddRandomWebPartPage(page =>
        //            {
        //                page.AddContentByQueryWebPart(misc1);
        //                page.AddContentByQueryWebPart(misc2);
        //            });
        //        });
        //    });

        //    TestModel(model);
        //}

        #endregion

        #region filters

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.TemplateTypes")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_Filters()
        {
            var templateTypeId = BuiltInListTemplateTypeId.GenericList;

            var defToday = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " today";

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;

                def.FilterType1 = "DateTime";
                def.FilterField1 = BuiltInFieldId.Created.ToString("B");
                def.FilterDisplayValue1 = "[TODAY]";
                def.Filter1ChainingOperator = "And";
                def.FilterOperator1 = "Eq";
            });

            var defFromLastWeek = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " from last week";

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;

                def.FilterType1 = "DateTime";
                def.FilterField1 = BuiltInFieldId.Created.ToString("B");
                def.FilterDisplayValue1 = "[TODAY]";
                def.Filter1ChainingOperator = "And";
                def.FilterOperator1 = "Leq";

                def.FilterType2 = "DateTime";
                def.FilterField2 = BuiltInFieldId.Created.ToString("B");
                def.FilterDisplayValue2 = "-7";
                def.Filter2ChainingOperator = "And";
                def.FilterOperator2 = "Geq";

                def.Filter2IsCustomValue = true;
                def.Filter2ChainingOperator = "And";
            });

            var defFromLastWeekContains = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " from last week contains";

                //def.WebUrl = "~sitecollection";
                def.ServerTemplate = templateTypeId;

                def.FilterType1 = "DateTime";
                def.FilterField1 = BuiltInFieldId.Created.ToString("B");
                def.FilterDisplayValue1 = "[TODAY]";
                def.Filter1ChainingOperator = "And";
                def.FilterOperator1 = "Leq";

                def.FilterType2 = "DateTime";
                def.FilterField2 = BuiltInFieldId.Created.ToString("B");
                def.FilterDisplayValue2 = "-7";
                def.Filter2ChainingOperator = "And";
                def.FilterOperator2 = "Geq";

                def.Filter2IsCustomValue = true;
                def.Filter2ChainingOperator = "And";

                def.FilterType3 = "Text";
                def.FilterField3 = BuiltInFieldId.Title.ToString("B");
                def.FilterDisplayValue3 = "m2";
                def.FilterOperator3 = "Contains";

                def.Filter3IsCustomValue = false;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(defToday);
                        page.AddContentByQueryWebPart(defFromLastWeek);
                        page.AddContentByQueryWebPart(defFromLastWeekContains);
                    });
                });
            });

            TestModel(model);
        }

        #endregion

        #region field mappings

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.ContentByQueryWebPart.Mappings")]
        public void CanDeploy_ContentByQueryWebPart_AsIs_With_Mappings()
        {
            var cqwpBindingService = new ContentByQueryWebPartBindingService();

            cqwpBindingService
                .AddDataMappingViewFields(new[]
                {
                    BuiltInPublishingFieldDefinitions.PublishedLinksURL,
                    BuiltInFieldDefinitions.EncodedAbsThumbnailUrl,
                    BuiltInPublishingFieldDefinitions.PublishingRollupImage,
                    BuiltInPublishingFieldDefinitions.Title,
                    BuiltInFieldDefinitions.Description,
                    BuiltInFieldDefinitions.PublishedDate,
                    BuiltInFieldDefinitions.LikesCount
                })
                .AddDataMapping("Description", new[] { BuiltInFieldDefinitions.PublishedDate, BuiltInFieldDefinitions.LikesCount })
                .AddDataMapping("ImageUrl", new[] { BuiltInFieldDefinitions.EncodedAbsThumbnailUrl, BuiltInPublishingFieldDefinitions.PublishingRollupImage })
                .AddDataMapping("Title", new[] { BuiltInPublishingFieldDefinitions.Title, BuiltInFieldDefinitions.Description })
                .AddDataMapping("LinkUrl", new[] { BuiltInPublishingFieldDefinitions.PublishedLinksURL });

            var dataMappingViewFields = cqwpBindingService.DataMappingViewFields;
            var dataMapping = cqwpBindingService.DataMapping;

            var templateTypeId = BuiltInListTemplateTypeId.Posts;

            var defDefault = ModelGeneratorService.GetRandomDefinition<ContentByQueryWebPartDefinition>(def =>
            {
                def.Title = "As is with template type " + templateTypeId + " and item style Default";

                def.ServerTemplate = templateTypeId;

                def.ItemStyle = BuiltInItemStyleNames.Default;
                def.ItemLimit = 3;

                def.SortByFieldType = "DateTime";
                def.SortBy = BuiltInFieldDefinitions.Created.Id.ToString("B");
                def.SortByDirection = "Desc";

                def.DataMappings = dataMapping;
                def.DataMappingViewFields = dataMappingViewFields;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddContentByQueryWebPart(defDefault);
                    });
                });
            });

            TestModel(model);
        }

        #endregion
    }
}
