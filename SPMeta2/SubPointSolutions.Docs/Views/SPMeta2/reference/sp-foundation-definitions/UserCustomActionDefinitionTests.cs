using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using System.IO;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Utils;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Enumerations;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.UserCustomAction)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class UserCustomActionDefinitionTests : ProvisionTestBase
    {
        #region methods

      
        [TestMethod]
        [TestCategory("Docs.UserCustomActionDefinition")]

        [SampleMetadata(Title = "Add custom action to site",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployUserCustomActionUnderSite()
        {
            var siteLogger = new UserCustomActionDefinition
            {
                Name = "m2SiteLogger",
                Location = "ScriptLink",
                ScriptBlock = "console.log('site logger on site:' + _spPageContextInfo.siteAbsoluteUrl);",
                Sequence = 1000
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddUserCustomAction(siteLogger);
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.UserCustomActionDefinition")]

        [SampleMetadata(Title = "Add custom action with jQuery",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployUserCustomActionWithJquery()
        {
            var jQueryFile = new ModuleFileDefinition
            {
                FileName = "jquery-1.11.2.min.js",
                Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Docs.Modules.jquery-1.11.2.min.js"),
                Overwrite = true
            };

            var appScriptsFolder = new FolderDefinition
            {
                Name = "M2 App Scripts"
            };

            var jQueryCustomAction = new UserCustomActionDefinition
            {
                Name = "m2jQuery",
                Location = "ScriptLink",
                ScriptSrc = UrlUtility.CombineUrl(new string[]
                {
                    "~sitecollection",
                    BuiltInListDefinitions.StyleLibrary.GetListUrl(),
                    appScriptsFolder.Name,
                    jQueryFile.FileName
                }),
                Sequence = 1500
            };

            var jQuerySiteLogger = new UserCustomActionDefinition
            {
                Name = "m2jQuerySiteLogger",
                Location = "ScriptLink",
                ScriptBlock = "jQuery(document).ready( function() { console.log('jQuery site logger on site:' + _spPageContextInfo.siteAbsoluteUrl); } );",
                Sequence = 1600
            };

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site
                  .AddUserCustomAction(jQueryCustomAction)
                  .AddUserCustomAction(jQuerySiteLogger);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.StyleLibrary, list =>
                {
                    list.AddFolder(appScriptsFolder, folder =>
                    {
                        folder.AddModuleFile(jQueryFile);
                    });
                });
            });

            DeployModel(siteModel);
            DeployModel(webModel);
        }


     
        [TestMethod]
        [TestCategory("Docs.UserCustomActionDefinition")]

        [SampleMetadata(Title = "Add custom action to web",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployUserCustomActionOnWeb()
        {
            var webLogger = new UserCustomActionDefinition
            {
                Name = "m2WebLogger",
                Location = "ScriptLink",
                ScriptBlock = "console.log('site logger on web:' + _spPageContextInfo.webAbsoluteUrl);",
                Sequence = 1800
            };

            var loggerWeb = new WebDefinition
            {
                Title = "M2 Logger Web",
                Url = "m2logging",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(loggerWeb, subWeb =>
                {
                    subWeb.AddUserCustomAction(webLogger);
                });
            });

            DeployModel(model);
        }

        [SampleMetadata(Title = "Add custom action to list",
                    Description = ""
                    )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployUserCustomActionOnList()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                // TODO
            });

            DeployModel(model);
        }

        #endregion
    }
}