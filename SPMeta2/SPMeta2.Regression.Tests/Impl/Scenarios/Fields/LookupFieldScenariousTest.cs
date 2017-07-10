using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;

using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Regression.Tests.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Utils;
using SPMeta2.Definitions.ContentTypes;

using SPMeta2.Regression.Tests.Extensions;

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

            public ModelNode WebModel { get; set; }

            public LookupFieldDefinition LookupField { get; set; }

            public ListDefinition ChildList { get; set; }
            public ListDefinition MasterList { get; set; }

            public ModelNode ChildListNode { get; set; }
        }

        protected LookupFieldEnvironment GetLookupFieldEnvironment(Action<LookupFieldEnvironment> action)
        {
            return GetLookupFieldEnvironment(action, null);
        }

        protected LookupFieldEnvironment GetLookupFieldEnvironment(Action<LookupFieldEnvironment> action,
            WebDefinition destinationWebDefinition)
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
                def.Indexed = false;
                //def.LookupListTitle = dataList.Title;
            });

            ModelNode childListNode = null;

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(lookupField);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddField(lookupField);
            });

            var childWebModel = SPMeta2Model.NewWebModel(web =>
            {
                if (destinationWebDefinition != null)
                {
                    web.AddWeb(destinationWebDefinition, subWeb =>
                    {
                        subWeb.AddList(dataList, list =>
                        {
                            childListNode = list;

                            list
                                .AddRandomListItem()
                                .AddRandomListItem()
                                .AddRandomListItem();
                        });
                    });
                }
                else
                {
                    web.AddList(dataList, list =>
                    {
                        childListNode = list;

                        list
                            .AddRandomListItem()
                            .AddRandomListItem()
                            .AddRandomListItem();
                    });
                }
            });

            var masterWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(masterList, list =>
                {
                    list.AddListFieldLink(lookupField);
                });
            });

            result.LookupField = lookupField;

            result.ChildList = dataList;
            result.ChildListNode = childListNode;
            result.ChildListModel = childWebModel;


            result.MasterList = masterList;
            result.MasterListModel = masterWebModel;

            result.SiteModel = siteModel;
            result.WebModel = webModel;

            if (action != null)
                action(result);

            return result;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect")]
        public void CanDeploy_LookupField_AsEmptySingleSelect()
        {
            var field = GetSingleSelectLookupDefinition(def =>
            {
                def.LookupListTitle = string.Empty;
                def.LookupListUrl = string.Empty;
                def.LookupList = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
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
            WithDisabledPropertyUpdateValidation(() =>
            {
                var lookupEnvironment = GetLookupFieldEnvironment(env =>
                {
                    env.LookupField.LookupListTitle = env.ChildList.Title;
                });

                TestModels(new ModelNode[]{
                    lookupEnvironment.ChildListModel, 
                    lookupEnvironment.SiteModel, 
                    lookupEnvironment.MasterListModel
                });
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

            TestModels(new ModelNode[]
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

            TestModels(new ModelNode[]
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

        //    TestModels(new  ModelNode[]
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

            TestModels(new ModelNode[]
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
#pragma warning disable 618
                env.LookupField.LookupListUrl = env.ChildList.GetListUrl();
#pragma warning restore 618
            });

            TestModels(new ModelNode[]
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

        #region scopes


        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.LookupField.Scopes")]
        //public void CanDeploy_LookupField_ToSite()
        //{
        //    WithDisabledPropertyUpdateValidation(() =>
        //    {
        //        var lookupEnvironment = GetLookupFieldEnvironment(env =>
        //        {
        //            env.LookupField.AllowMultipleValues = true;
        //            env.LookupField.LookupListTitle = env.ChildList.Title;
        //        });

        //        TestModels(new  ModelNode[]
        //        {
        //            lookupEnvironment.ChildListModel,
        //            lookupEnvironment.SiteModel,
        //            lookupEnvironment.MasterListModel
        //        });
        //    });
        //}


        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.LookupField.Scopes")]
        //public void CanDeploy_LookupField_ToRootWeb()
        //{
        //    WithDisabledPropertyUpdateValidation(() =>
        //    {
        //        var lookupEnvironment = GetLookupFieldEnvironment(env =>
        //        {
        //            env.LookupField.AllowMultipleValues = true;
        //            env.LookupField.LookupListTitle = env.ChildList.Title;
        //        });

        //        TestModels(new  ModelNode[]
        //        {
        //            lookupEnvironment.ChildListModel,
        //            lookupEnvironment.WebModel,
        //            lookupEnvironment.MasterListModel
        //        });
        //    });
        //}


        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.LookupField.Scopes")]
        //public void CanDeploy_LookupField_ToSubWeb()
        //{
        //    WithDisabledPropertyUpdateValidation(() =>
        //    {
        //        var lookupEnvironment = GetLookupFieldEnvironment(env =>
        //        {
        //            env.LookupField.AllowMultipleValues = true;
        //            env.LookupField.LookupListTitle = env.ChildList.Title;
        //        });

        //        TestModels(new  ModelNode[]
        //        {
        //            lookupEnvironment.ChildListModel,
        //            lookupEnvironment.WebModel,
        //            lookupEnvironment.MasterListModel
        //        });
        //    });
        //}

        #endregion

        #region count related

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.CountRelated")]
        public void CanDeploy_LookupField_With_CountRelated()
        {
            var field1 = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.CountRelated = Rnd.Bool();
            });

            var field2 = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.CountRelated = !field1.CountRelated;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field1);
                site.AddField(field2);
            });

            TestModel(siteModel);
        }

        #endregion

        #region multi seelct

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.MultiSelect")]
        public void CanDeploy_LookupField_AsEmptyMultiSelectSelect()
        {
            var field = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.AllowMultipleValues = true;

                def.LookupListTitle = string.Empty;
                def.LookupListUrl = string.Empty;
                def.LookupList = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

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
            WithDisabledPropertyUpdateValidation(() =>
            {
                var lookupEnvironment = GetLookupFieldEnvironment(env =>
                {
                    env.LookupField.AllowMultipleValues = true;
                    env.LookupField.LookupListTitle = env.ChildList.Title;
                });

                TestModels(new ModelNode[]
                {
                    lookupEnvironment.ChildListModel,
                    lookupEnvironment.SiteModel,
                    lookupEnvironment.MasterListModel
                });
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

            TestModels(new ModelNode[]
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

            TestModels(new ModelNode[]
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

        //    TestModels(new  ModelNode[]
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

            TestModels(new ModelNode[]
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
#pragma warning disable 618
                env.LookupField.LookupListUrl = env.ChildList.GetListUrl();
#pragma warning restore 618
            });

            TestModels(new ModelNode[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        #endregion

        #region post bindings

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.PostBinding")]
        public void CanDeploy_LookupField_WithPostBinding_AsListTitle()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupList = string.Empty;
                env.LookupField.LookupListTitle = string.Empty;
                env.LookupField.LookupListUrl = string.Empty;
            });

            TestModels(new ModelNode[]
            {
                lookupEnvironment.SiteModel, 
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.MasterListModel,
            });

            // binding
            lookupEnvironment.LookupField.LookupListTitle = lookupEnvironment.ChildList.Title;

            TestModels(new ModelNode[]
            {
                lookupEnvironment.SiteModel, 
            });

            // this would not pass validation as lookup should be already bound 
            WithExcpectedException(typeof(AssertFailedException), () =>
            {
                lookupEnvironment.LookupField.LookupListTitle = lookupEnvironment.MasterList.Title;

                TestModels(new ModelNode[]            {
                    lookupEnvironment.SiteModel, 
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.PostBinding")]
        public void CanDeploy_LookupField_WithPostBinding_AsListUrl()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupList = string.Empty;
                env.LookupField.LookupListTitle = string.Empty;
                env.LookupField.LookupListUrl = string.Empty;
            });

            TestModels(new ModelNode[]
            {
                lookupEnvironment.SiteModel, 
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.MasterListModel,
            });

            // binding
#pragma warning disable 618
            lookupEnvironment.LookupField.LookupListUrl = lookupEnvironment.ChildList.GetListUrl();
#pragma warning restore 618

            TestModels(new ModelNode[]
            {
                lookupEnvironment.SiteModel, 
            });

            // this would not pass validation as lookup should be already bound 
            WithExcpectedException(typeof(AssertFailedException), () =>
            {
                lookupEnvironment.LookupField.LookupListTitle = lookupEnvironment.MasterList.Title;

                TestModels(new ModelNode[]            {
                    lookupEnvironment.SiteModel, 
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.PostBinding")]
        public void CanDeploy_LookupField_WithPostBinding_AsListId()
        {
            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.LookupField.LookupList = string.Empty;
                env.LookupField.LookupListTitle = string.Empty;
                env.LookupField.LookupListUrl = string.Empty;
            });

            lookupEnvironment.ChildListNode.OnProvisioned<object>(context =>
            {
                lookupEnvironment.LookupField.LookupList = ExtractListId(context).ToString();
            });

            TestModels(new ModelNode[]
            {
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel,
                lookupEnvironment.ChildListModel, 
            });

            TestModels(new ModelNode[]
            {
                lookupEnvironment.SiteModel, 
            });

            // this would not pass validation as lookup should be already bound 
            WithExcpectedException(typeof(AssertFailedException), () =>
            {
                lookupEnvironment.LookupField.LookupListTitle = lookupEnvironment.MasterList.Title;

                TestModels(new ModelNode[]            {
                    lookupEnvironment.SiteModel, 
                });
            });
        }



        #endregion

        #region custom sub web

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect.WebUrl")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListById_OnSubWeb()
        {
            var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
                env.ChildListNode.OnProvisioned<object>(context =>
                {
                    env.LookupField.LookupList = ExtractListId(context).ToString();
                    env.LookupField.LookupWebUrl = UrlUtility.CombineUrl("~sitecollection", subWeb.Url);
                });
            }, subWeb);

            TestModels(new ModelNode[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect.WebUrl")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListByTitle_OnSubWeb()
        {
            WithDisabledPropertyUpdateValidation(() =>
            {
                var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
                {

                });

                var lookupEnvironment = GetLookupFieldEnvironment(env =>
                {
                    env.LookupField.LookupListTitle = env.ChildList.Title;
                    env.LookupField.LookupWebUrl = UrlUtility.CombineUrl("~sitecollection", subWeb.Url);

                }, subWeb);

                TestModels(new ModelNode[]{
                    lookupEnvironment.ChildListModel, 
                    lookupEnvironment.SiteModel, 
                    lookupEnvironment.MasterListModel
                 });
            });

        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect.WebUrl")]
        public void CanDeploy_LookupField_AsSingleSelectAndBindToListUrl_OnSubWeb()
        {
            var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var lookupEnvironment = GetLookupFieldEnvironment(env =>
            {
#pragma warning disable 618
                env.LookupField.LookupListUrl = env.ChildList.GetListUrl();
#pragma warning restore 618
                env.LookupField.LookupWebUrl = UrlUtility.CombineUrl("~sitecollection", subWeb.Url);

            }, subWeb);

            TestModels(new ModelNode[]
            {
                lookupEnvironment.ChildListModel, 
                lookupEnvironment.SiteModel, 
                lookupEnvironment.MasterListModel
            });
        }

        #endregion

        #region custom cases

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect.WebUrl")]
        public void CanDeploy_LookupField_As_SiteMaster_And_SubWebChild()
        {
            // https://github.com/SubPointSolutions/spmeta2/issues/694

            // 1 - Installing a generic list (Departments) on Top-Level-Site-Collection and add some data (Just using Title-Field).
            var masterDepartmentsList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;

#pragma warning disable 618
                def.Url = string.Empty;
#pragma warning restore 618
                def.CustomUrl = string.Format("Lists/{0}", Rnd.String());
            });

            var masterDepartmentsRootWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(masterDepartmentsList, list =>
                {
                    list.AddRandomListItem();
                    list.AddRandomListItem();
                    list.AddRandomListItem();
                });
            });

            // 2 - Creating a Site-Column (Department) of type Lookup, Data Comes from previous mentioned List.
            var departmentsFieldLookup = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.LookupListTitle = masterDepartmentsList.Title;
                def.LookupWebUrl = "~sitecollection";
            });

            // 3 -  Creating a Site-Content-Type on Top-Level-Site-Collection of type Document (Contract) and add the Site-Column-Lookup-field Department.
            var contractDocumentContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Document;
            });

            // the site model for 2-3 containing field and content type (IA -> information architecture :)
            var masterIASiteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(departmentsFieldLookup);

                site.AddContentType(contractDocumentContentType, contentType =>
                {
                    contentType.AddContentTypeFieldLink(departmentsFieldLookup);
                });
            });

            // 4 - Now on a Sub-Site of the Top-Level-Site-Collection 
            // create a Document-Library (Contracts) and add the previous mentioned Site-Content-Type.

            var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var contractsDocumentLibrary = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;

#pragma warning disable 618
                def.Url = string.Empty;
#pragma warning restore 618
                def.CustomUrl = string.Format("{0}", Rnd.String());

                // just don't want to go site content -> find a list..
                def.OnQuickLaunch = true;
            });

            var contractsSubWebModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddList(contractsDocumentLibrary, list =>
                    {
                        list.AddContentTypeLink(contractDocumentContentType);

                        // making the content type defullt
                        list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                        {
                            ContentTypes = new List<ContentTypeLinkValue>
                            {
                                new ContentTypeLinkValue { ContentTypeName = contractDocumentContentType.Name}
                            }
                        });
                    });
                });
            });

            // deployment
            // 1 - deploy root list
            TestModel(masterDepartmentsRootWebModel);

            // 2 - deploy lookup list pointing a site level (root web) list and content type
            TestModel(masterIASiteModel);

            // 3 - deploy the sunu web, list, attach content type to a list and make it nice
            TestModel(contractsSubWebModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.LookupField.SingleSelect.WebUrl")]
        public void CanDeploy_LookupField_As_SubWebMaster_And_SubWebChild()
        {
            // https://github.com/SubPointSolutions/spmeta2/issues/694

            // 1 - Installing a generic list (Departments) on subweb and add some data (Just using Title-Field).
            var masterDepartmentsList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;

#pragma warning disable 618
                def.Url = string.Empty;
#pragma warning restore 618
                def.CustomUrl = string.Format("Lists/{0}", Rnd.String());
            });

            // on a Sub-Site of the Top-Level-Site-Collection 
            // create a Document-Library (Contracts) and add the previous mentioned Site-Content-Type.
            var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {

            });

            var masterDepartmentsSubWebModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddList(masterDepartmentsList, list =>
                    {
                        list.AddRandomListItem();
                        list.AddRandomListItem();
                        list.AddRandomListItem();
                    });
                });
            });

            // 2 - Creating a web-column (Department) of type Lookup, Data Comes from previous mentioned List.
            // ~site token must refer to the sub web and the field is web-scoped
            var departmentsFieldLookup = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.LookupListTitle = masterDepartmentsList.Title;
                def.LookupWebUrl = "~site";
            });

            // 3 -  Creating a Site-Content-Type on Top-Level-Site-Collection of type Document (Contract) and add the Site-Column-Lookup-field Department.
            var contractDocumentContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Document;
            });

            // the sub web model for 2-3 containing field and content type (IA -> information architecture :)
            var masterIASubWebModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddField(departmentsFieldLookup);

                    web.AddContentType(contractDocumentContentType, contentType =>
                    {
                        contentType.AddContentTypeFieldLink(departmentsFieldLookup);
                    });
                });
            });

            var contractsDocumentLibrary = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;

#pragma warning disable 618
                def.Url = string.Empty;
#pragma warning restore 618
                def.CustomUrl = string.Format("{0}", Rnd.String());

                // just don't want to go site content -> find a list..
                def.OnQuickLaunch = true;
            });

            var contractsSubWebModel = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(subWeb, web =>
                {
                    web.AddList(contractsDocumentLibrary, list =>
                    {
                        list.AddContentTypeLink(contractDocumentContentType);

                        // making the content type defullt
                        list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                        {
                            ContentTypes = new List<ContentTypeLinkValue>
                            {
                                new ContentTypeLinkValue { ContentTypeName = contractDocumentContentType.Name}
                            }
                        });
                    });
                });
            });

            // deployment
            // 1 - deploy root list
            TestModel(masterDepartmentsSubWebModel);

            // 2 - deploy lookup list pointing a site level (root web) list and content type
            TestModel(masterIASubWebModel);

            // 3 - deploy the sunu web, list, attach content type to a list and make it nice
            TestModel(contractsSubWebModel);
        }

        #endregion
    }
}
