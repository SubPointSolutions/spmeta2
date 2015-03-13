using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Fields
{
    [TestClass]
    public class UserFieldScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region bindings

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.UserField.Bindings")]
        public void CanDeploy_UserField_BindedBySecurityGroupId()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();
            var userField = ModelGeneratorService.GetRandomDefinition<UserFieldDefinition>(def =>
            {
                def.SelectionGroup = null;
                def.SelectionGroupName = string.Empty;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddSecurityGroup(securityGroup, group =>
                    {
                        group.OnProvisioned<object>(context =>
                        {
                            userField.SelectionGroup = ExtractGroupId(context);
                        });
                    })
                    .AddUserField(userField);
            });

            TestModel(siteModel);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.UserField.Bindings")]
        public void CanDeploy_UserField_BindedBySecurityGroupName()
        {
            var securityGroup = ModelGeneratorService.GetRandomDefinition<SecurityGroupDefinition>();
            var userField = ModelGeneratorService.GetRandomDefinition<UserFieldDefinition>(def =>
            {
                def.SelectionGroup = null;
                def.SelectionGroupName = securityGroup.Name;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddSecurityGroup(securityGroup)
                    .AddUserField(userField);
            });

            TestModel(siteModel);
        }

        #endregion

        #region utils

        private int ExtractGroupId(Models.OnCreatingContext<object, DefinitionBase> context)
        {
            var obj = context.Object;
            var objType = context.Object.GetType();

            if (objType.ToString().Contains("Microsoft.SharePoint.Client.Group"))
            {
                return (int)obj.GetPropertyValue("Id");
            }
            else if (objType.ToString().Contains("Microsoft.SharePoint.SPGroup"))
            {
                return (int)obj.GetPropertyValue("ID");
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("ID property extraction is not implemented for type: [{0}]", objType));
            }
        }

        #endregion
    }
}
