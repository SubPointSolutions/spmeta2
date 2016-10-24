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

    public class BreakRoleInheritanceDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.BreakRoleInheritanceDefinition")]

        [SampleMetadata(Title = "Break role inheritance on web",
                        Description = ""
                        )]
        public void CanDeploySimpleBreakRoleInheritance_OnWeb()
        {
            var privateProjectWebDef = new WebDefinition
            {
                Title = "Private project",
                Url = "private-project",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };

            var privateProjectWebBreakRoleInheritance = new BreakRoleInheritanceDefinition
            {
                CopyRoleAssignments = false
            };

            var privateSecurityGroupMembers = new SecurityGroupDefinition
            {
                Name = "Private Project Group Members"
            };

            var privateSecurityGroupViewers = new SecurityGroupDefinition
            {
                Name = "Private Project Group Viewers"
            };

            // site model with the groups
            var siteModel = SPMeta2Model.NewSiteModel(site =>
           {
               site.AddSecurityGroup(privateSecurityGroupMembers);
               site.AddSecurityGroup(privateSecurityGroupViewers);
           });

            // web model 
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(privateProjectWebDef, publicProjectWeb =>
                {
                    publicProjectWeb.AddBreakRoleInheritance(privateProjectWebBreakRoleInheritance, privateProjectResetWeb =>
                    {
                        // privateProjectResetWeb is your web but after breaking role inheritance

                        // link group with roles by SecurityRoleType / SecurityRoleName
                        // use BuiltInSecurityRoleTypes or BuiltInSecurityRoleNames 

                        // add group with contributor permission
                        privateProjectResetWeb.AddSecurityGroupLink(privateSecurityGroupMembers, group =>
                        {
                            group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                            {
                                SecurityRoleType = BuiltInSecurityRoleTypes.Contributor
                            });
                        });

                        // add group with reader permission
                        privateProjectResetWeb.AddSecurityGroupLink(privateSecurityGroupViewers, group =>
                        {
                            group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                            {
                                SecurityRoleType = BuiltInSecurityRoleTypes.Reader
                            });
                        });
                    });
                });
            });

            // deploy site model with groups, and then web model with the rest
            DeployModel(siteModel);
            DeployModel(webModel);
        }


        [TestMethod]
        [TestCategory("Docs.BreakRoleInheritanceDefinition")]

        [SampleMetadata(Title = "Break role inheritance on list",
                        Description = ""
                        )]
        public void CanDeploySimpleBreakRoleInheritance_OnList()
        {
            var privateListDef = new ListDefinition
            {
                Title = "Private records",
                TemplateType = BuiltInListTemplateTypeId.GenericList,
                CustomUrl = "lists/private-records",
            };

            var privateProjectWebBreakRoleInheritance = new BreakRoleInheritanceDefinition
            {
                CopyRoleAssignments = false
            };

            var privateSecurityGroupMembers = new SecurityGroupDefinition
            {
                Name = "Private Project Group Members"
            };

            var privateSecurityGroupViewers = new SecurityGroupDefinition
            {
                Name = "Private Project Group Viewers"
            };

            // site model with the groups
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(privateSecurityGroupMembers);
                site.AddSecurityGroup(privateSecurityGroupViewers);
            });

            // web model 
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(privateListDef, publicProjectWeb =>
                {
                    publicProjectWeb.AddBreakRoleInheritance(privateProjectWebBreakRoleInheritance, privateResetList =>
                    {
                        // privateResetList is your list but after breaking role inheritance

                        // link group with roles by SecurityRoleType / SecurityRoleName
                        // use BuiltInSecurityRoleTypes or BuiltInSecurityRoleNames 

                        // add group with contributor permission
                        privateResetList.AddSecurityGroupLink(privateSecurityGroupMembers, group =>
                        {
                            group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                            {
                                SecurityRoleType = BuiltInSecurityRoleTypes.Contributor
                            });
                        });

                        // add group with reader permission
                        privateResetList.AddSecurityGroupLink(privateSecurityGroupViewers, group =>
                        {
                            group.AddSecurityRoleLink(new SecurityRoleLinkDefinition
                            {
                                SecurityRoleType = BuiltInSecurityRoleTypes.Reader
                            });
                        });
                    });
                });
            });

            // deploy site model with groups, and then web model with the rest
            DeployModel(siteModel);
            DeployModel(webModel);
        }

        #endregion
    }
}