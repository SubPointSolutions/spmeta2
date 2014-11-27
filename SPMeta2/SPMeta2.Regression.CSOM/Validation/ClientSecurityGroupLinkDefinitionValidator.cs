using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
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

            context.ExecuteQuery();

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
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsAssociatedMemberGroup);
                    var assosiatedMemberGroup = web.AssociatedMemberGroup;

                    var isValid = spObject.Id == assosiatedMemberGroup.Id;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedMemberGroup, "IsAssociatedMemberGroup is false. Skipping.");
            }

            if (definition.IsAssociatedOwnerGroup)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsAssociatedOwnerGroup);
                    var assosiatedOwnerGroup = web.AssociatedOwnerGroup;

                    var isValid = spObject.Id == assosiatedOwnerGroup.Id;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedOwnerGroup, "IsAssociatedOwnerGroup is false. Skipping.");
            }

            if (definition.IsAssociatedVisitorGroup)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsAssociatedVisitorGroup);
                    var assosiatedVisitorGroup = web.AssociatedVisitorGroup;

                    var isValid = spObject.Id == assosiatedVisitorGroup.Id;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValid
                    };
                });
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

    //internal static class SPGroupLinkExtensions
    //{
    //    public static Group GetAssociatedVisitorGroup(this Group group)
    //    {
    //        return group.we.AssociatedVisitorGroup;
    //    }

    //    public static SPGroup GetAssociatedOwnerGroup(this SPGroup group)
    //    {
    //        return group.ParentWeb.AssociatedOwnerGroup;
    //    }

    //    public static SPGroup GetAssociatedMemberGroup(this SPGroup group)
    //    {
    //        return group.ParentWeb.AssociatedMemberGroup;
    //    }
    //}
}
