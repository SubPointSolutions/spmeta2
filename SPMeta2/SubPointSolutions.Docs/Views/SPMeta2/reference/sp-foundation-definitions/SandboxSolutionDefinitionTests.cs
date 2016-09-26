using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using System.IO;
using SPMeta2.Syntax.Default.Utils;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.SiteCollection)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class SandboxSolutionDefinitionTests : ProvisionTestBase
    {
        #region methods



        [TestMethod]
        [TestCategory("Docs.SandboxSolutionDefinition")]

        [SampleMetadata(Title = "Add sandbox solution",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleSandboxSolutionDefinition()
        {
            // FileName could be different to the original solution name
            // FileName must not have "." to avoid fails (DesignPackage API limitations)
            
            // Content is a byte array, so get ot from whatever source you want
            
            // SolutionId is used to lookup existing sandbox package
            // get SolutionId from the VS project or XML inside WSP package
            
            // Activate must be always true for CSOM (DesignPackage API limitations)

            var myBranding = new SandboxSolutionDefinition
            {
                FileName = "MyBranding.wsp",
                Content = File.ReadAllBytes("MySandboxBranding.wsp"),
                SolutionId = new Guid("0CDCC076-A472-4DD9-9A1F-0E1E761ED61D"),
                Activate = true,
            };

            var myTasks = new SandboxSolutionDefinition
            {
                FileName = "MyTasks.wsp",
                Content = ModuleFileUtils.FromResource(GetType().Assembly, "MyIntranet.Resources.MyTasks.wsp"),
                SolutionId = new Guid("3D279748-92FC-49F9-A6C5-A10FBCD2DB24"),
                Activate = true,
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                  .AddSandboxSolution(myBranding)
                  .AddSandboxSolution(myTasks);
            });

            DeployModel(model);
        }

        #endregion
    }
}