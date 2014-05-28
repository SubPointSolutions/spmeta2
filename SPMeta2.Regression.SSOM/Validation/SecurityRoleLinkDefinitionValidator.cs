using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityRoleLinkDefinitionValidator : SPMeta2.SSOM.ModelHandlers.SecurityRoleLinkModelHandler
    {
        #region methods

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var modelHostContext = modelHost.WithAssertAndCast<SPMeta2.SSOM.ModelHosts.SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var securityRoleLinkModel = model.WithAssertAndCast<SecurityRoleLinkDefinition>("model", value => value.RequireNotNull());

            var securableObject = modelHostContext.SecurableObject;
            var securityGroup = modelHostContext.SecurityGroup;

            if (securableObject == null || securableObject is SPWeb)
            {
                // this is SPGroup -> SPRoleLink deployment
                AssertSPGroupHost(securityGroup, securityGroup, securityRoleLinkModel);
            }
            else if (securableObject is SPList)
            {
                AssertSPListHost(securableObject as SPList, securityGroup, securityRoleLinkModel);
            }
            else
            {
                throw new Exception(string.Format("modelHost of type:[{0}] is not supported.", modelHost.GetType()));
            }
        }

        private void AssertSPListHost(SPList targetList, SPGroup securityGroup, SecurityRoleLinkDefinition securityRoleLinkModel)
        {
            var web = targetList.ParentWeb;
            var role = web.RoleDefinitions[securityRoleLinkModel.SecurityRoleName];

            // check if roleAssignment has current  role

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] securableObject:[{1}]", securityRoleLinkModel, targetList));

                traceScope.WithTraceIndent(trace =>
                {
                    // asserting it exists
                    traceScope.WriteLine(string.Format("Validating existance..."));

                    var roleAssignment = targetList
                                            .RoleAssignments
                                            .OfType<SPRoleAssignment>()
                                            .FirstOrDefault(r => r.Member.ID == securityGroup.ID);

                    Assert.IsNotNull(roleAssignment);

                    traceScope.WriteLine(string.Format("Validating role presence..."));
                    roleAssignment.RoleDefinitionBindings.Contains(role);

                    traceScope.WriteLine(string.Format("Role [{0}] exists!", securityRoleLinkModel.SecurityRoleName));
                });
            });
        }

        private void AssertSPGroupHost(SPGroup securableObject, SPGroup securityGroup, SecurityRoleLinkDefinition securityRoleLinkModel)
        {
            var web = securityGroup.ParentWeb;
            var role = web.RoleDefinitions[securityRoleLinkModel.SecurityRoleName];

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] securableObject:[{1}]", securityRoleLinkModel, web));

                traceScope.WithTraceIndent(trace =>
                {
                    traceScope.WriteLine(string.Format("Validating existance..."));
                    
                    var roleAssignment = web
                                    .RoleAssignments
                                    .OfType<SPRoleAssignment>()
                                    .FirstOrDefault(r => r.Member.ID == securityGroup.ID);

                    Assert.IsNotNull(roleAssignment);

                    traceScope.WriteLine(string.Format("Validating role presence..."));
                    roleAssignment.RoleDefinitionBindings.Contains(role);

                    traceScope.WriteLine(string.Format("Role [{0}] exists!", securityRoleLinkModel.SecurityRoleName));
                });
            });
        }

        #endregion
    }
}
