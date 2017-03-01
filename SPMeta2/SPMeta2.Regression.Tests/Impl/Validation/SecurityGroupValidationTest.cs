using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services.Impl.Validation;

namespace SPMeta2.Regression.Tests.Impl.Validation
{
    [TestClass]
    public class SecurityGroupValidationTest : SPMeta2DefinitionRegresionTestBase
    {
        #region group link options

        [TestMethod]
        [TestCategory("CI.Core")]
        [TestCategory("Regression.Validation.SecurityGroup")]
        public void Validation_SecurityGroup_ShouldAllow_EmptyName_If_BuiltIn()
        {
            var builtInGroups = new List<SecurityGroupDefinition>()
            {
                BuiltInSecurityGroupDefinitions.AssociatedMemberGroup.Inherit(),
                BuiltInSecurityGroupDefinitions.AssociatedOwnerGroup.Inherit(),
                BuiltInSecurityGroupDefinitions.AssociatedVisitorsGroup.Inherit()
            };

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroups(builtInGroups);
            });

            var service = new DefaultRequiredPropertiesValidationService();
            service.DeployModel(null, siteModel);
        }

        [TestMethod]
        [TestCategory("CI.Core")]
        [TestCategory("Regression.Validation.SecurityGroup")]
        public void Validation_SecurityGroup_ShouldFail_On_EmptyName_If_Not_BuiltIn()
        {
            var builtInGroups = new List<SecurityGroupDefinition>()
            {
                BuiltInSecurityGroupDefinitions.AssociatedMemberGroup.Inherit(def =>
                {
                    def.Name = null;
                    
                    def.IsAssociatedMemberGroup = false;
                    def.IsAssociatedOwnerGroup = false;
                    def.IsAssociatedVisitorsGroup = false;
                }),
                BuiltInSecurityGroupDefinitions.AssociatedOwnerGroup.Inherit(def =>
                {
                    def.Name = null;
                    
                    def.IsAssociatedMemberGroup = false;
                    def.IsAssociatedOwnerGroup = false;
                    def.IsAssociatedVisitorsGroup = false;
                }),
                BuiltInSecurityGroupDefinitions.AssociatedVisitorsGroup.Inherit(def =>
                {
                    def.Name = null;
                    
                    def.IsAssociatedMemberGroup = false;
                    def.IsAssociatedOwnerGroup = false;
                    def.IsAssociatedVisitorsGroup = false;
                })
            };

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroups(builtInGroups);
            });

            var isValidException = false;

            try
            {
                var service = new DefaultRequiredPropertiesValidationService();
                service.DeployModel(null, siteModel);
            }
            catch (Exception e)
            {
                isValidException = e is SPMeta2ModelDeploymentException
                                   && e.InnerException is SPMeta2AggregateException
                                   &&
                                   (e.InnerException as SPMeta2AggregateException).InnerExceptions[0] is
                                   SPMeta2ModelValidationException;
            }

            Assert.IsTrue(isValidException);
        }

        #endregion
    }
}
