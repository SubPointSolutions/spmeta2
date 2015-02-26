using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
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

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Webparts
{
    [TestClass]
    public class XsltListViewWebPartScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region list binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByListTitle()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInDefinitions.BuiltInListDefinitions.StyleLibrary.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });


            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList)
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByListUrl()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
                def.ListUrl = BuiltInDefinitions.BuiltInListDefinitions.StyleLibrary.GetListUrl();

                def.ViewName = string.Empty;
                def.ViewId = null;
            });


            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList)
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByListId()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList, list =>
                        {
                            list.OnProvisioned<object>(context =>
                            {
                                xsltListViewWebpart.ListId = ExtractListId(context);
                            });
                        })
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }



        #endregion

        #region list view binding tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByViewId()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var sourceView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Fields = new System.Collections.ObjectModel.Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.Title                    
                };

                def.IsDefault = false;
            });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
                def.ListUrl = sourceList.GetListUrl();

                def.ViewName = string.Empty;
                def.ViewId = null;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList, list =>
                        {
                            list.AddListView(sourceView, view =>
                            {
                                view.OnProvisioned<object>(context =>
                                {
                                    xsltListViewWebpart.ViewId = ExtractViewId(context);
                                });
                            });
                        })
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart")]
        public void CanDeploy_XsltListViewWebPart_ByViewName()
        {
            var sourceList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def => { });
            var sourceView = ModelGeneratorService.GetRandomDefinition<ListViewDefinition>(def =>
            {
                def.Fields = new System.Collections.ObjectModel.Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.Edit,
                    BuiltInInternalFieldNames.Title                    
                };

                def.IsDefault = false;
            });

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = string.Empty;
                def.ListUrl = sourceList.GetListUrl();

                def.ViewName = sourceView.Title;
                def.ViewId = null;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddList(sourceList, list =>
                        {
                            list.AddListView(sourceView);
                        })
                        .AddHostList(BuiltInListDefinitions.SitePages, list =>
                        {
                            list
                                .AddRandomWebPartPage(page =>
                                {
                                    page.AddXsltListViewWebPart(xsltListViewWebpart);
                                });
                        });

                });

            TestModel(model);
        }


        #endregion

        #region utils

        private Guid ExtractViewId(Models.OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.View"))
            {
                return (Guid)obj.GetPropertyValue("Id");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPView"))
            {
                return (Guid)obj.GetPropertyValue("ID");
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
            }
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
    }
}
