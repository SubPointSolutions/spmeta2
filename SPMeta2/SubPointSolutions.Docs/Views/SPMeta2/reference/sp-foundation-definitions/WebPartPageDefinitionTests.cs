using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebPartPages)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class WebPartPageDefinitionTests : ProvisionTestBase
    {
        #region methods


        [SampleMetadata(Title = "Add web part page",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        [TestMethod]
        [TestCategory("Docs.WebPartPageDefinition")]
        public void CanDeployWebPartPages()
        {
            var customersReportPage = new WebPartPageDefinition
            {
                Title = "Customer reports",
                FileName = "Customers-report.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var parthesReportPage = new WebPartPageDefinition
            {
                Title = "Parthers reports",
                FileName = "Parthers-report.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd2
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddWebPartPage(customersReportPage)
                        .AddWebPartPage(parthesReportPage);
                });
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.WebPartPageDefinition")]

        [SampleMetadata(Title = "Add custom web part page",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployWebPartPageWithCustomTemplate()
        {
            var customizedWebPartPage = new WebPartPageDefinition
            {
                Title = "Customers report",
                FileName = "Customers-report.aspx",
                CustomPageLayout = "___ a custom web part page template here ___ "
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddWebPartPage(customizedWebPartPage);
                });
            });

            DeployModel(model);
        }



        [TestMethod]
        [TestCategory("Docs.WebPartPageDefinition")]

        [SampleMetadata(Title = "Add web part page to folder",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployWebPartPagesUnderFolders()
        {
            // clients folder and pages
            var clientsFolder = new FolderDefinition()
            {
                Name = "Customers"
            };

            var clientMay2015Page = new WebPartPageDefinition
            {
                Title = "May 2015",
                FileName = "May-2015-analytics.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var clientJune2015Page = new WebPartPageDefinition
            {
                Title = "June 2015",
                FileName = "June-2015-analytics.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            // parthers folder and pages
            var parthersFolder = new FolderDefinition()
            {
                Name = "Parthers"
            };

            var parther2014AnnualReport = new WebPartPageDefinition
            {
                Title = "Annual report 2014",
                FileName = "Annual-report-2014.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var parther2015AnnualReport = new WebPartPageDefinition
            {
                Title = "Annual report 2015",
                FileName = "Annual-report-2015.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1,
            };

            // linking everything together
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddFolder(clientsFolder, folder =>
                        {
                            folder
                                .AddWebPartPage(clientMay2015Page)
                                .AddWebPartPage(clientJune2015Page);
                        })
                        .AddFolder(parthersFolder, folder =>
                        {
                            folder
                              .AddWebPartPage(parther2014AnnualReport)
                              .AddWebPartPage(parther2015AnnualReport);
                        });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}