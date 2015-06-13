using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Fields
{
    [TestClass]
    public class DependentLookupFieldScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region options

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DependentLookupField.Scope")]
        public void CanDeploy_DependentLookupField_OnSite()
        {
            var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.ShowInNewForm = true;
                def.ShowInDisplayForm = true;
                def.ShowInEditForm = true;
                def.ShowInListSettings = true;
                def.ShowInViewForms = true;

                def.Hidden = false;
                def.Required = false;
                def.AllowMultipleValues = false;
            });

            var dependentIdLookupField = ModelGeneratorService.GetRandomDefinition<DependentLookupFieldDefinition>(
                def =>
                {
                    def.LookupField = BuiltInInternalFieldNames.ID;
                    def.PrimaryLookupFieldId = lookupField.Id;
                });

            var dependentTitleLookupField = ModelGeneratorService.GetRandomDefinition<DependentLookupFieldDefinition>(
                def =>
                {
                    def.LookupField = BuiltInInternalFieldNames.Title;
                    def.PrimaryLookupFieldId = lookupField.Id;
                });

            var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();
            var childList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddLookupField(lookupField);

                site.AddDependentLookupField(dependentIdLookupField);
                site.AddDependentLookupField(dependentTitleLookupField);
            });

            TestModel(siteModel);

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(masterList, list =>
                {
                    for (var i = 0; i < 10; i++)
                    {
                        list.AddListItem(new ListItemDefinition
                        {
                            Title = string.Format("master item {0} - {1}", i, Rnd.String())
                        });
                    }
                });

                web.AddList(childList, list =>
                {
                    list.AddListFieldLink(lookupField);
                });
            });

            TestModel(webModel);

            // rebind lookup 
            lookupField.LookupListUrl = masterList.GetListUrl();

            TestModel(siteModel);

            // add dep field
            var depWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(childList, list =>
                {
                    list.AddListFieldLink(dependentIdLookupField);
                    list.AddListFieldLink(dependentTitleLookupField);

                    list.AddListView(new ListViewDefinition
                    {
                        Title = "Test View",
                        Fields = new Collection<string>
                        {
                            BuiltInInternalFieldNames.Edit,
                            BuiltInInternalFieldNames.ID,
                            BuiltInInternalFieldNames.Title,
                            lookupField.InternalName,
                            dependentIdLookupField.InternalName,
                            dependentTitleLookupField.InternalName
                        },
                        IsDefault = true
                    });
                });
            });

            TestModel(depWebModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DependentLookupField.Scope")]
        public void CanDeploy_DependentLookupField_OnContentType()
        {
            var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            {
                def.ShowInNewForm = true;
                def.ShowInDisplayForm = true;
                def.ShowInEditForm = true;
                def.ShowInListSettings = true;
                def.ShowInViewForms = true;

                def.Hidden = false;
                def.Required = false;
                def.AllowMultipleValues = false;
            });

            var contentTypeWithDepLookup = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Hidden = false;
            });

            var dependentIdLookupField = ModelGeneratorService.GetRandomDefinition<DependentLookupFieldDefinition>(
                def =>
                {
                    def.LookupField = BuiltInInternalFieldNames.ID;
                    def.PrimaryLookupFieldId = lookupField.Id;
                });

            var dependentTitleLookupField = ModelGeneratorService.GetRandomDefinition<DependentLookupFieldDefinition>(
                def =>
                {
                    def.LookupField = BuiltInInternalFieldNames.Title;
                    def.PrimaryLookupFieldId = lookupField.Id;
                });

            var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();
            var childList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(list =>
            {
                list.ContentTypesEnabled = true;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddLookupField(lookupField);

                site.AddDependentLookupField(dependentIdLookupField);
                site.AddDependentLookupField(dependentTitleLookupField);

                site.AddContentType(contentTypeWithDepLookup, contentType =>
                {
                    contentType
                        .AddContentTypeFieldLink(lookupField)
                        .AddContentTypeFieldLink(dependentIdLookupField)
                        .AddContentTypeFieldLink(dependentTitleLookupField);
                });
            });

            TestModel(siteModel);

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(masterList, list =>
                {
                    for (var i = 0; i < 10; i++)
                    {
                        list.AddListItem(new ListItemDefinition
                        {
                            Title = string.Format("master item {0} - {1}", i, Rnd.String())
                        });
                    }
                });

                web.AddList(childList, list =>
                {
                    // list.AddListFieldLink(lookupField);
                });
            });

            TestModel(webModel);

            // rebind lookup 
            lookupField.LookupListUrl = masterList.GetListUrl();

            TestModel(siteModel);

            // add dep field
            var depWebModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(childList, list =>
                {
                    list.AddContentTypeLink(contentTypeWithDepLookup);
                    list.AddUniqueContentTypeOrder(new UniqueContentTypeOrderDefinition
                    {
                        ContentTypes = new List<ContentTypeLinkValue>
                        {
                            new ContentTypeLinkValue { ContentTypeName = contentTypeWithDepLookup.Name }
                        }
                    });

                    //list.AddListFieldLink(dependentIdLookupField);
                    //list.AddListFieldLink(dependentTitleLookupField);

                    list.AddListView(new ListViewDefinition
                    {
                        Title = "Test View",
                        Fields = new Collection<string>
                        {
                            BuiltInInternalFieldNames.Edit,
                            BuiltInInternalFieldNames.ID,
                            BuiltInInternalFieldNames.Title,
                            lookupField.InternalName,
                            dependentIdLookupField.InternalName,
                            dependentTitleLookupField.InternalName
                        },
                        IsDefault = true
                    });
                });
            });

            TestModel(depWebModel);
        }

        //private Guid ExtractFieldId(Models.OnCreatingContext<object, DefinitionBase> context)
        //{
        //    var obj = context.Object;
        //    var objType = context.Object.GetType();

        //    if (objType.ToString().Contains("Microsoft.SharePoint.Client.FieldLookup"))
        //    {
        //        return (Guid)obj.GetPropertyValue("Id");
        //    }
        //    else if (objType.ToString().Contains("Microsoft.SharePoint.SPFieldLookup"))
        //    {
        //        return (Guid)obj.GetPropertyValue("Id");
        //    }
        //    else
        //    {
        //        throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
        //    }
        //}

        //private Guid ExtractWebId(Models.OnCreatingContext<object, DefinitionBase> context)
        //{
        //    var obj = context.Object;
        //    var objType = context.Object.GetType();

        //    if (objType.ToString().Contains("Microsoft.SharePoint.Client.Web"))
        //    {
        //        return (Guid)obj.GetPropertyValue("Id");
        //    }
        //    else if (objType.ToString().Contains("Microsoft.SharePoint.SPWeb"))
        //    {
        //        return (Guid)obj.GetPropertyValue("ID");
        //    }
        //    else
        //    {
        //        throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
        //    }
        //}

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DependentLookupField.Scope")]
        public void CanDeploy_DependentLookupField_OnWeb()
        {
            throw new SPMeta2NotImplementedException("");

            //WithDisabledPropertyUpdateValidation(() =>
            //{

            //    var lookupField = ModelGeneratorService.GetRandomDefinition<LookupFieldDefinition>(def =>
            //    {
            //        def.ShowInNewForm = true;
            //        def.ShowInDisplayForm = true;
            //        def.ShowInEditForm = true;
            //        def.ShowInListSettings = true;
            //        def.ShowInViewForms = true;

            //        def.Indexed = false;

            //        def.Hidden = false;
            //        def.Required = false;
            //        def.AllowMultipleValues = false;
            //    });

            //    var dependentIdLookupField = ModelGeneratorService.GetRandomDefinition<DependentLookupFieldDefinition>(
            //        def =>
            //        {
            //            def.LookupField = BuiltInInternalFieldNames.ID;
            //            def.PrimaryLookupFieldId = lookupField.Id;
            //        });

            //    var dependentTitleLookupField = ModelGeneratorService
            //        .GetRandomDefinition<DependentLookupFieldDefinition>(
            //            def =>
            //            {
            //                def.LookupField = BuiltInInternalFieldNames.Title;
            //                def.PrimaryLookupFieldId = lookupField.Id;
            //            });

            //    var masterList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();
            //    var childList = ModelGeneratorService.GetRandomDefinition<ListDefinition>();

            //    var lookupSubWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>();

            //    var depIdFieldId = Guid.Empty;
            //    var depTitleFieldId = Guid.Empty;

            //    var lookupWebId = Guid.Empty;

            //    var subWebBootstrapperModel = SPMeta2Model.NewWebModel(web =>
            //    {
            //        web.AddWeb(lookupSubWeb, subWeb =>
            //        {
            //            subWeb.OnProvisioned<object>(context =>
            //            {
            //                //lookupWebId = ExtractWebId(context);
            //                lookupField.LookupWebId = lookupWebId;
            //            });
            //        });
            //    });

            //    TestModel(subWebBootstrapperModel);

            //    var subWebModel = SPMeta2Model.NewWebModel(web =>
            //    {
            //        web.AddWeb(lookupSubWeb, subWeb =>
            //        {
            //            subWeb.AddLookupField(lookupField);

            //            subWeb.AddDependentLookupField(dependentIdLookupField, field =>
            //            {
            //                field.OnProvisioned<object>(context =>
            //                {
            //                    depIdFieldId = ExtractFieldId(context);
            //                });
            //            });

            //            subWeb.AddDependentLookupField(dependentTitleLookupField, field =>
            //            {
            //                field.OnProvisioned<object>(context =>
            //                {
            //                    depTitleFieldId = ExtractFieldId(context);
            //                });
            //            });
            //        });
            //    });

            //    TestModel(subWebModel);

            //    var webModel = SPMeta2Model.NewWebModel(web =>
            //    {
            //        web.AddWeb(lookupSubWeb, subWeb =>
            //        {
            //            subWeb.AddList(masterList, list =>
            //            {
            //                for (var i = 0; i < 10; i++)
            //                {
            //                    list.AddListItem(new ListItemDefinition
            //                    {
            //                        Title = string.Format("master item {0} - {1}", i, Rnd.String())
            //                    });
            //                }
            //            });

            //            subWeb.AddList(childList, list =>
            //            {
            //                list.AddListFieldLink(lookupField);
            //            });
            //        });
            //    });

            //    TestModel(webModel);

            //    // rebind llookup 
            //    lookupField.LookupListUrl = masterList.GetListUrl();

            //    TestModel(subWebModel);

            //    // add dep field
            //    var depWebModel = SPMeta2Model.NewWebModel(web =>
            //    {
            //        web.AddWeb(lookupSubWeb, subWeb =>
            //        {
            //            subWeb.AddList(childList, list =>
            //            {
            //                list.AddListFieldLink(new ListFieldLinkDefinition
            //                {
            //                    FieldId = depIdFieldId
            //                });

            //                list.AddListFieldLink(new ListFieldLinkDefinition
            //                {
            //                    FieldId = depTitleFieldId
            //                });

            //                list.AddListView(new ListViewDefinition
            //                {
            //                    Title = "Test View",
            //                    Fields = new Collection<string>
            //                    {
            //                        BuiltInInternalFieldNames.Edit,
            //                        BuiltInInternalFieldNames.ID,
            //                        BuiltInInternalFieldNames.Title,
            //                        lookupField.InternalName,
            //                        dependentIdLookupField.InternalName,
            //                        dependentTitleLookupField.InternalName
            //                    },
            //                    IsDefault = true
            //                });
            //            });
            //        });
            //    });

            //    TestModel(depWebModel);
            //});
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.DependentLookupField.Scope")]
        public void CanDeploy_DependentLookupField_OnList()
        {
            throw new SPMeta2NotImplementedException("");
        }

        #endregion
    }
}
