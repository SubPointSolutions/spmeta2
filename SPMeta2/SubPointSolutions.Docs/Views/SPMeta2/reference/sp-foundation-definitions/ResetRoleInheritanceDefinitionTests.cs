using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Security)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ResetRoleInheritanceDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.ResetRoleInheritanceDefinition")]


        [SampleMetadata(Title = "Reset role inheritance on list",
                            Description = ""
                            )]
        public void CanDeployResetRoleInheritanceDefinition_OnList()
        {
            var listDef = new ListDefinition
            {
                Title = "Public records",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                CustomUrl = "lists/public-records",
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddResetRoleInheritance(new ResetRoleInheritanceDefinition(), resetList =>
                    {
                        // resetList is your list but after resetting role inheritance
                        // build your model as usual

                        // resetList.AddListView(...)
                    });
                });
            });

            DeployModel(model);
        }

        [TestMethod]
        [TestCategory("Docs.ResetRoleInheritanceDefinition")]


        [SampleMetadata(Title = "Reset role inheritance on web",
                            Description = ""
                            )]
        public void CanDeployResetRoleInheritanceDefinition_OnWeb()
        {
            var publicProjectWebDef = new WebDefinition
            {
                Title = "Public project",
                Url = "public-project",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(publicProjectWebDef, publicProjectWeb =>
                {
                    publicProjectWeb.AddResetRoleInheritance(new ResetRoleInheritanceDefinition(), publicProjectResetWeb =>
                    {
                        // publicProjectResetWeb is your web but after resetting role inheritance
                        // build your model as usual

                        // publicProjectResetWeb.AddList(...)
                    });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}