using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;
using System;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ListsAndLibraries)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ListFieldLinkDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.ListFieldLinkDefinition")]
        [SampleMetadata(Title = "Add field links to list",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleListFieldLinkDefinition()
        {
            var fieldDef = new TextFieldDefinition
            {
                Title = "Customer number",
                InternalName = "m2CustomNumber",
                Id = new Guid("87247c7d-1ecc-4503-bfd5-21f107b442fb")
            };

            var listDef = new ListDefinition
            {
                Title = "Customers",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                CustomUrl = "lists/customers",
            };

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddTextField(fieldDef);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    // will add a link to the site level field
                    list.AddListFieldLink(fieldDef);
                });
            });

            DeployModel(siteModel);
            DeployModel(webModel);
        }

        #endregion
    }
}