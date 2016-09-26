using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;

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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebParts)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ScriptEditorWebPartDefinitionTests : ProvisionTestBase
    {
        #region methods

        

        [TestMethod]
        [TestCategory("Docs.ScriptEditorWebPartDefinition")]

        [SampleMetadata(Title = "Add Script Editor web part",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleScriptEditorWebPartDefinition()
        {
            var scriptEditor = new ScriptEditorWebPartDefinition
            {
                Title = "Empty Script Editor",
                Id = "m2EmptyScriptEditorrWhichMustBeMoreThan32Chars",
                ZoneIndex = 10,
                ZoneId = "Main"
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 Script Editor provision",
                FileName = "script-editor-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddScriptEditorWebPart(scriptEditor);
                      });
                  });
            });

            DeployModel(model);
        }

       

        [TestMethod]
        [TestCategory("Docs.ScriptEditorWebPartDefinition")]

        [SampleMetadata(Title = "Add Script Editor web part with content",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployScriptEditorWebPartwithContent()
        {
            var scriptEditor = new ScriptEditorWebPartDefinition
            {
                Title = "Pre-provisioned Script Editor",
                Id = "m2ScriptEditorWithLoggerWhichMustBeMoreThan32Chars",
                ZoneIndex = 20,
                ZoneId = "Main",
                Content = " <script> console.log('script editor log');  </script> Pre-provisioned Script Editor Content"
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 Script Editor provision",
                FileName = "script-editor-webpart-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                  .AddHostList(BuiltInListDefinitions.SitePages, list =>
                  {
                      list.AddWebPartPage(webPartPage, page =>
                      {
                          page.AddScriptEditorWebPart(scriptEditor);
                      });
                  });
            });

            DeployModel(model);
        }

        #endregion
    }
}