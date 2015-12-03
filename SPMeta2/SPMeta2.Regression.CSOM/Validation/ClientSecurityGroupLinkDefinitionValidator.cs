using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityGroupLinkDefinitionValidator : SecurityGroupLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);
            var context = web.Context;

            context.Load(web, w => w.SiteGroups);

            context.Load(web, w => w.AssociatedMemberGroup);
            context.Load(web, w => w.AssociatedOwnerGroup);
            context.Load(web, w => w.AssociatedVisitorGroup);

            context.Load(securableObject, s => s.RoleAssignments.Include(r => r.Member));

            context.ExecuteQueryWithTrace();

            var spObject = ResolveSecurityGroup(definition, web, context);

            var assert = ServiceFactory.AssertService
                    .NewAssert(definition, spObject)
                          .ShouldNotBeNull(spObject);


            if (!string.IsNullOrEmpty(definition.SecurityGroupName))
                assert.ShouldBeEqual(m => m.SecurityGroupName, o => o.Title);
            else
                assert.SkipProperty(m => m.SecurityGroupName, "SecurityGroupName is null or empty. Skipping.");

            if (definition.IsAssociatedMemberGroup)
            {
                assert.ShouldBeEqual(m => m.IsAssociatedMemberGroup, spObject.Id == web.AssociatedMemberGroup.Id);
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedMemberGroup, "IsAssociatedMemberGroup is false. Skipping.");
            }

            if (definition.IsAssociatedOwnerGroup)
            {
                assert.ShouldBeEqual(m => m.IsAssociatedOwnerGroup, spObject.Id == web.AssociatedOwnerGroup.Id);
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedOwnerGroup, "IsAssociatedOwnerGroup is false. Skipping.");
            }

            if (definition.IsAssociatedVisitorGroup)
            {
                assert.ShouldBeEqual(m => m.IsAssociatedVisitorGroup, spObject.Id == web.AssociatedVisitorGroup.Id);
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedVisitorGroup, "IsAssociatedVisitorsGroup is false. Skipping.");
            }
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
