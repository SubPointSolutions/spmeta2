using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultXmlBasedPropertiesValidationServiceTests : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public DefaultXmlBasedPropertiesValidationServiceTests()
        {
            Service = new DefaultXmlBasedPropertiesValidationService();
        }

        #endregion

        #region properties

        public DefaultXmlBasedPropertiesValidationService Service { get; set; }


        #endregion

        #region caml

        [TestMethod]
        [TestCategory("Regression.Services.DefaultXmlBasedPropertiesValidationService.Caml")]
        public void ShouldPass_On_Valid_CAML()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddListView(new ListViewDefinition
                    {
                        Title = Rnd.String(),
                        Query = Rnd.CamlQuery()

                    });
                });
            });

            Service.DeployModel(null, model);

        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultXmlBasedPropertiesValidationService.Caml")]
        public void ShouldFail_On_Invalid_CAML()
        {
            WithExcpectedExceptions(new[]
            {
                typeof (SPMeta2ModelValidationException)
            }, () =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddRandomList(list =>
                    {
                        list.AddListView(new ListViewDefinition
                        {
                            Title = Rnd.String(),
                            Query = Rnd.CamlQuery().Replace("<Where>", "Where")

                        });
                    });
                });

                Service.DeployModel(null, model);
            });
        }

        #endregion

        #region caml

        [TestMethod]
        [TestCategory("Regression.Services.DefaultXmlBasedPropertiesValidationService.Xml")]
        public void ShouldPass_On_Valid_Xml()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddRandomWebPartPage(page =>
                    {
                        page.AddWebPart(new WebPartDefinition
                        {
                            WebpartXmlTemplate = BuiltInWebPartTemplates.ContentEditorWebPart
                        });
                    });
                });
            });

            Service.DeployModel(null, model);
        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultXmlBasedPropertiesValidationService.Xml")]
        public void ShouldFail_On_Invalid_Xml()
        {
            WithExcpectedExceptions(new[]
            {
                typeof (SPMeta2ModelValidationException)
            }, () =>
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list.AddRandomWebPartPage(page =>
                        {
                            page.AddWebPart(new WebPartDefinition
                            {
                                WebpartXmlTemplate = BuiltInWebPartTemplates.ContentEditorWebPart
                                                                            .Replace("<Title>", "Title")
                            });
                        });
                    });
                });

                Service.DeployModel(null, model);
            });
        }

        #endregion
    }
}
