using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions.Fields;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;
using System;
using System.Collections.ObjectModel;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Fields)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class MultiChoiceFieldDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.MultiChoiceFieldDefinition")]

        [SampleMetadata(Title = "Add multichoice field",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleMultiChoiceFieldDefinition()
        {
            var fieldDef = new MultiChoiceFieldDefinition
            {
                Title = "Tasks label",
                InternalName = "dcs_ProgressTag",
                Group = "SPMeta2.Samples",
                Id = new Guid("b08325aa-a750-4bf9-a73e-c470b86d37c8"),
                Choices = new Collection<string>
                {
                    "internal",
                    "external",
                    "bug",
                    "easy fix",
                    "enhancement"
                }
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddMultiChoiceField(fieldDef);
            });

            DeployModel(model);
        }

        #endregion
    }
}