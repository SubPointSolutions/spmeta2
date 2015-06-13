using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
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

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Webparts
{
    [TestClass]
    public class XsltListViewWebPartScenariosTest : ListViewWebPartScenariosTestBase
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

        #region xml - xslt

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXmlDefinition()
        {
            // this test gonna fail for CSOM due lack of API support
            // XmlDefinition setup is not supported via CSOM

            // http://officespdev.uservoice.com/forums/224641-general/suggestions/6358731-import-xsltlistviewwebpart-definition
            // http://sharepoint.stackexchange.com/questions/90433/add-document-library-xsltlistviewwebpart-using-csom-or-web-services

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                def.XmlDefinition = string.Format("<View BaseViewID=\"{0}\" />", 2);
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
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
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXmlDefinitionLink()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                def.XmlDefinitionLink = Rnd.HttpUrl();
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
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
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXslLink()
        {
            var xslFileName = string.Format("m2.{0}.xsl", Rnd.String());

            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                // this needs to be set to get XslLink working
                // either SP1 issue or http context = null 
                def.BaseXsltHashKey = Guid.NewGuid().ToString();
                def.XslLink = "/Style Library/" + xslFileName;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
                        .AddHostList(BuiltInListDefinitions.StyleLibrary, list =>
                        {
                            list.AddModuleFile(new ModuleFileDefinition
                            {
                                Content = Encoding.UTF8.GetBytes(XsltStrings.XsltListViewWebPart_TestXsl),
                                FileName = xslFileName
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
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithXsl()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                // this needs to be set to get XslLink working
                // either SP1 issue or http context = null 
                def.BaseXsltHashKey = Guid.NewGuid().ToString();
                def.Xsl = XsltStrings.XsltListViewWebPart_TestXsl;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
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
        [TestCategory("Regression.Scenarios.Webparts.XsltListViewWebPart.XmlAndXslt")]
        public void CanDeploy_XsltListViewWebPart_WithGhostedXslLink()
        {
            var xsltListViewWebpart = ModelGeneratorService.GetRandomDefinition<XsltListViewWebPartDefinition>(def =>
            {
                def.ListId = Guid.Empty;
                def.ListTitle = BuiltInListDefinitions.SitePages.Title;
                def.ListUrl = string.Empty;

                def.ViewName = string.Empty;
                def.ViewId = null;

                def.GhostedXslLink = "blog.xsl";
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web
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
    }


}
