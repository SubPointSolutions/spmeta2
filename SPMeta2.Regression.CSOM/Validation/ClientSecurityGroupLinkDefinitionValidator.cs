using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityGroupLinkDefinitionValidator : SecurityGroupLinkModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var securableObject = modelHost.WithAssertAndCast<SecurableObject>("modelHost", value => value.RequireNotNull());
            var securityGroupLinkModel = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);

            var context = web.Context;

            context.Load(web, w => w.SiteGroups);
            context.Load(securableObject, s => s.RoleAssignments.Include(r => r.Member));

            context.ExecuteQuery();

            var securityGroup = WebExtensions.FindGroupByName(web.SiteGroups, securityGroupLinkModel.SecurityGroupName);

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] securableObject:[{1}]", securityGroupLinkModel, securityGroup));

                traceScope.WithTraceIndent(trace =>
                {
                    // asserting it exists
                    trace.WriteLine(string.Format("Validating existance..."));

                    var existingRoleAssignments = FindClientRoleRoleAssignment(securableObject.RoleAssignments, securityGroup);

                    Assert.IsNotNull(existingRoleAssignments);

                    trace.WriteLine(string.Format("RoleAssignments for security group link [{0}] exists.", securityGroupLinkModel.SecurityGroupName));
                });
            });
        }

        protected RoleAssignment FindClientRoleRoleAssignment(RoleAssignmentCollection roleAssignments, Group securityGroup)
        {
            foreach (var ra in roleAssignments)
                if (ra.Member.Id == securityGroup.Id)
                    return ra;

            return null;
        }
    }
}
