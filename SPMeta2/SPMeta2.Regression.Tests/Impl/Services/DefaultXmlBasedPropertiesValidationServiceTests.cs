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
using SPMeta2.Services.Impl.Validation;

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
            var isValid = false;

            try
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
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
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
                    // #1, web part XML
                    list.AddRandomWebPartPage(page =>
                    {
                        // web parts v2
                        page.AddWebPart(new WebPartDefinition
                        {
                            WebpartXmlTemplate = BuiltInWebPartTemplates.ContentEditorWebPart
                        });

                        // web parts v3
                        page.AddWebPart(new WebPartDefinition
                        {
                            WebpartXmlTemplate = BuiltInWebPartTemplates.ScriptEditorWebPart
                        });
                    });

                    // #2.1, bit of CAML in list view
                    list.AddRandomListView();

                    // #2.2 views with custom CAML
                    list.AddListView(new ListViewDefinition
                    {
                        Title = "Custom View",
                        Query = @"<GroupBy Collapse='TRUE' GroupLimit='30'>
                                    <FieldRef Name='STW_FAQCategory' />
                                    <FieldRef Name='STW_FAQQuestion' />
                                    </GroupBy>
                                    <OrderBy>
                                    <FieldRef Name='ID' />
                                    <FieldRef Name='STW_FAQQuestion' />
                                    </OrderBy>"
                    });

                    // #3.1, Raw field XML
                    list.AddField(new FieldDefinition
                    {
                        Id = Guid.NewGuid(),
                        InternalName = Guid.NewGuid().ToString(),
                        FieldType = BuiltInFieldTypes.Boolean,
                        RawXml = @"<Field ID='{060E50AC-E9C1-4D3C-B1F9-DE0BCAC300F6}'
                                     Name='Amount'
                                     DisplayName='Amount'
                                     Type='Currency'
                                     Decimals='2'
                                     Min='0'
                                     Required='FALSE'
                                     Group='Financial Columns' />"
                    });

                    // #3.2, Raw field XML
                    list.AddField(new FieldDefinition
                    {
                        Id = Guid.NewGuid(),
                        InternalName = Guid.NewGuid().ToString(),
                        FieldType = BuiltInFieldTypes.Boolean,
                        RawXml = @"<Field ID='{943E7530-5E2B-4C02-8259-CCD93A9ECB18}'
                                         Name='CostCenter'
                                         DisplayName='Cost Center'
                                         Type='Choice'
                                         Required='FALSE'
                                         Group='Financial Columns'>
                                    <CHOICES>
                                      <CHOICE>Administration</CHOICE>
                                      <CHOICE>Information</CHOICE>
                                      <CHOICE>Facilities</CHOICE>
                                      <CHOICE>Operations</CHOICE>
                                      <CHOICE>Sales</CHOICE>
                                      <CHOICE>Marketing</CHOICE>
                                    </CHOICES>
                                  </Field>"
                    });

                });
            });

            Service.DeployModel(null, model);
        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultXmlBasedPropertiesValidationService.Xml")]
        public void ShouldFail_On_Invalid_Xml()
        {
            var isValid = false;

            try
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
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }


        #endregion
    }
}
