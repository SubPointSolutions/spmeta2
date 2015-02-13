using System;
using System.Collections.Generic;
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

        protected LookupFieldDefinition GetSingleSelectLookupDefinition()
        {
            return GetSingleSelectLookupDefinition(null);
        }

        protected LookupFieldDefinition GetSingleSelectLookupDefinition(Action<LookupFieldDefinition> action)
        {
            var result = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.ShowInNewForm = true;
                def.Hidden = false;
                def.Required = false;
                def.AllowMultipleValues = false;
            });

            if (action != null)
                action(result);

            return result;
        }

        protected class LookupFieldEnvironment
        {
            public ModelNode ChildListModel { get; set; }
            public ModelNode MasterListModel { get; set; }

            public ModelNode SiteModel { get; set; }

            public LookupFieldDefinition LookupField { get; set; }

            public ListDefinition ChildList { get; set; }

            public ModelNode ChildListNode { get; set; }
        }

        protected LookupFieldEnvironment GetLookupFieldEnvironment(Action<LookupFieldEnvironment> action)
        {
            var result = new LookupFieldEnvironment();

            var dataList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var lookupField = GetSingleSelectLookupDefinition(def =>
            {
                //def.LookupListTitle = dataList.Title;
            });

            ModelNode childListNode = null;

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(lookupField);
            });

            var childWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(dataList, list =>
                {
                    childListNode = list;

                    list
                        .AddRandomListItem()
                        .AddRandomListItem()
                        .AddRandomListItem();
                });
            });

            var masterWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(masterList, list =>
                    {
                        list.AddListFieldLink(lookupField);
                    });
            });

            result.LookupField = lookupField;

            result.LookupField = lookupField;
            result.ChildList = dataList;
            result.ChildListNode = childListNode;

            result.ChildListModel = childWebModel;
            result.MasterListModel = masterWebModel;
            result.SiteModel = siteModel;

            if (action != null)
                action(result);

            return result;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelect()
        {
            var field = GetSingleSelectLookupDefinition();

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
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupListTitle = env.ChildList.Title;
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });

        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListById()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.ChildListNode.OnProvisioned<object>(context =>
                {
                    env.LookupField.LookupList = ExtractListId(context).ToString();
                });
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToSelf()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupList = "Self";
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        //public void CanDeploy_LookupField_AsSingleSelectAndBindToDocs()
        //{
        //    var lookupEnvironment = GetLookupFieldEnvironment(env =>
        //    {
        //        env.LookupField.LookupList = "Docs";
        //    });

        //    TestModels(new[]
        //    {
        //        lookupEnvironment.ChildListModel, 
        //        lookupEnvironment.SiteModel, 
        //        lookupEnvironment.MasterListModel
        //    });
        //}

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToUserInfo()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupList = "UserInfo";
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListUrl()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupListUrl = env.ChildList.GetListUrl();
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
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

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsMultiSelectAndBindToListByTitle()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.AllowMultipleValues = true;
                env.LookupField.LookupListTitle = env.ChildList.Title;
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });

        }



        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsMultiSelectAndBindToListById()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.ChildListNode.OnProvisioned<object>(context =>
                {
                    env.LookupField.AllowMultipleValues = true;
                    env.LookupField.LookupList = ExtractListId(context).ToString();
                });
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsMultiSelectAndBindToSelf()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.AllowMultipleValues = true;
                env.LookupField.LookupList = "Self";
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        //public void CanDeploy_LookupField_AsMultiSelectAndBindToDocs()
        //{
        //    var lookupEnvironment = GetLookupFieldEnvironment(env =>
        //    {
        //        env.LookupField.AllowMultipleValues = true;
        //        env.LookupField.LookupList = "Docs";
        //    });

        //    TestModels(new[]
        //    {
        //        lookupEnvironment.ChildListModel, 
        //        lookupEnvironment.SiteModel, 
        //        lookupEnvironment.MasterListModel
        //    });
        //}

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsMultiSelectAndBindToUserInfo()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.AllowMultipleValues = true;
                env.LookupField.LookupList = "UserInfo";
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsMultiSelectAndBindToListUrl()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.AllowMultipleValues = true;
                env.LookupField.LookupListUrl = env.ChildList.GetListUrl();
            });

            TestModels(new[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        #endregion
    }
}
