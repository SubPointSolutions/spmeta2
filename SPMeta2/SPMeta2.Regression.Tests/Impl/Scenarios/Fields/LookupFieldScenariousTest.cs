using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Fields
{
    [TestClass]
    public class LookupFieldScenariousTest : SPMeta2RegresionScenarioTestBase
    {
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

        #region single select

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelect()
        {
            var field = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.Hidden = false;
                def.AllowMultipleValues = false;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListByTitle()
        {
            var dataList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });


            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(dataList, list =>
                {
                    list
                        .AddRandomListItem()
                        .AddRandomListItem()
                        .AddRandomListItem();
                });
            });

            var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.Hidden = false;
                def.Required = false;
                def.AllowMultipleValues = false;
                def.LookupListTitle = dataList.Title;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(lookupField);
            });

            var masterModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(masterList, list =>
                    {
                        list.AddListFieldLink(lookupField);
                    });
            });

            TestModels(new[] { webModel, siteModel, masterModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListById()
        {
            var dataList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.Hidden = false;
                def.Required = false;
                def.AllowMultipleValues = false;
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(dataList, list =>
                    {
                        list.OnProvisioned<object>(context =>
                        {
                            lookupField.LookupList = ExtractListId(context).ToString();
                        });

                        list
                            .AddRandomListItem()
                            .AddRandomListItem()
                            .AddRandomListItem();
                    });
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(lookupField);
            });

            var masterModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(masterList, list =>
                    {
                        list.AddListFieldLink(lookupField);
                    });
            });

            TestModels(new[] { webModel, siteModel, masterModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToSelf()
        {
            var dataList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.Hidden = false;
                def.Required = false;
                def.AllowMultipleValues = false;
                def.LookupList = "Self";
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web

                    .AddList(dataList, list =>
                    {
                        list
                            .AddRandomListItem()
                            .AddRandomListItem()
                            .AddRandomListItem();
                    });
            });

            var masterModel = SPMeta2Model.NewWebModel(web =>
            {
                web

                    .AddHostList(dataList, list =>
                    {
                        list.AddListFieldLink(lookupField);
                    });
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(lookupField);
            });

            TestModels(new[] { webModel, siteModel, masterModel });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListUrl()
        {
            var dataList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });


            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(dataList, list =>
                {
                    list
                        .AddRandomListItem()
                        .AddRandomListItem()
                        .AddRandomListItem();
                });
            });

            var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.Hidden = false;
                def.AllowMultipleValues = false;
                def.LookupListUrl = dataList.GetListUrl();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(lookupField);
            });

            var masterModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(masterList, list =>
                    {
                        list.AddListFieldLink(lookupField);
                    });
            });

            TestModels(new[] { webModel, siteModel, masterModel });
        }

        private Guid ExtractListId(Models.OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.List"))
            {
                return (Guid)obj.GetPropertyValue("Id");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPList"))
            {
                return (Guid)obj.GetPropertyValue("ID");
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
            }
        }


        #endregion

        #region multi seelct

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsMultiSelectSelect()
        {
            var field = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.AllowMultipleValues = true;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

        #endregion
    }
}
