using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebSite)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class TreeViewSettingsDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.TreeViewSettingsDefinition")]

        [SampleMetadata(Title = "Add tree view settings to web",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleTreeViewSettingsDefinition()
        {
            var treeViewSettings = new TreeViewSettingsDefinition
            {
                TreeViewEnabled = true,
                QuickLaunchEnabled = true
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddTreeViewSettings(treeViewSettings);
            });

            DeployModel(model);
        }

        #endregion
    }
}