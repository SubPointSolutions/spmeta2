using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;
using System.Collections.Generic;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebPartPages)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class DeleteWebPartsDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.DeleteWebPartsDefinition")]

        [SampleMetadata(Title = "Delete web part by Title",
                        Description = ""
                        )]
        public void CanDeployDeleteWebPartsDefinition_ByTitle()
        {
            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 webparts",
                FileName = "web-parts.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            // aiming to delete two web part with the following titles:
            // 'My Tasks'
            // 'My Projects'
            var myWebPartDeletionDef = new DeleteWebPartsDefinition
            {
                WebParts = new List<WebPartMatch>(new WebPartMatch[] { 
                    new WebPartMatch {
                        Title = "My Tasks"
                    },
                    new WebPartMatch {
                        Title = "My Projects"
                    }
                })
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddWebPartPage(webPartPage, page =>
                    {
                        page.AddDeleteWebParts(myWebPartDeletionDef);
                    });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}