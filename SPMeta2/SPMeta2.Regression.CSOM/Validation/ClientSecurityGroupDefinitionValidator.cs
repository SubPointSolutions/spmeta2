using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System.Linq;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityGroupDefinitionValidator : SecurityGroupModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (modelHost is SiteModelHost)
            {
                var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost",
                    value => value.RequireNotNull());

                var web = webModelHost.HostSite.RootWeb;
                var definition = model as SecurityGroupDefinition;

                var context = web.Context;

                // well, this should be pulled up to the site handler and init Load/Exec query
                context.Load(web, tmpWeb => tmpWeb.SiteGroups);
                context.ExecuteQueryWithTrace();

                var spObject = FindSecurityGroupByTitle(web.SiteGroups, definition.Name);

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, spObject)
                    .ShouldNotBeNull(spObject)
                    .ShouldBeEqual(m => m.Name, o => o.Title)
                    .ShouldBeEqual(m => m.OnlyAllowMembersViewMembership, o => o.OnlyAllowMembersViewMembership);
                //.ShouldBeEqual(m => m.Description, o => o.Description);

                if (!string.IsNullOrEmpty(definition.Description))
                    assert.ShouldBeEqual(m => m.Description, o => o.Description);
                else
                    assert.SkipProperty(m => m.Description, "Description is NULL. Skipping.");

                if (definition.AllowMembersEditMembership.HasValue)
                    assert.ShouldBeEqual(m => m.AllowMembersEditMembership, o => o.AllowMembersEditMembership);
                else
                    assert.SkipProperty(m => m.AllowMembersEditMembership,
                        "AllowMembersEditMembership is NULL. Skipping.");

                if (definition.AllowRequestToJoinLeave.HasValue)
                    assert.ShouldBeEqual(m => m.AllowRequestToJoinLeave, o => o.AllowRequestToJoinLeave);
                else
                    assert.SkipProperty(m => m.AllowRequestToJoinLeave, "AllowRequestToJoinLeave is NULL. Skipping.");

                if (definition.AutoAcceptRequestToJoinLeave.HasValue)
                    assert.ShouldBeEqual(m => m.AutoAcceptRequestToJoinLeave, o => o.AutoAcceptRequestToJoinLeave);
                else
                    assert.SkipProperty(m => m.AutoAcceptRequestToJoinLeave,
                        "AutoAcceptRequestToJoinLeave is NULL. Skipping.");


                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Owner);
                    var dstProp = d.GetExpressionValue(ct => ct.GetOwnerLogin());

                    var isValid = dstProp.Value.ToString().ToUpper().Replace("\\", "/").EndsWith(
                        srcProp.Value.ToString().ToUpper().Replace("\\", "/"));

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });

                assert.SkipProperty(m => m.DefaultUser, "DefaultUser cannot be setup via CSOM API. Skipping.");
            }
            else if (modelHost is SecurityGroupModelHost)
            {
                // skip everything, just check if the group is therw
                var securityGroupModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
                var definition = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

                var context = securityGroupModelHost.HostClientContext;
                var currentGroup = securityGroupModelHost.SecurityGroup;
                var subGroup = securityGroupModelHost.HostWeb.EnsureUser(definition.Name);

                context.Load(subGroup);
                context.Load(currentGroup, g => g.Users);

                context.ExecuteQueryWithTrace();

                var spObject = currentGroup.Users.FirstOrDefault(u => u.Id == subGroup.Id);


                var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);

                assert.SkipProperty(m => m.Name);
                assert.SkipProperty(m => m.AllowMembersEditMembership, "");
                assert.SkipProperty(m => m.AllowRequestToJoinLeave, "");
                assert.SkipProperty(m => m.AutoAcceptRequestToJoinLeave, "");
                assert.SkipProperty(m => m.DefaultUser, "");
                assert.SkipProperty(m => m.Description, "");
                assert.SkipProperty(m => m.IsAssociatedMemberGroup, "");
                assert.SkipProperty(m => m.IsAssociatedVisitorsGroup, "");
                assert.SkipProperty(m => m.IsAssociatedOwnerGroup, "");

                assert.SkipProperty(m => m.OnlyAllowMembersViewMembership, "");
                assert.SkipProperty(m => m.Owner, "");

            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException("modelHost");
            }

            //assert.ShouldBeEqual((p, s, d) =>
            //{
            //    var srcProp = s.GetExpressionValue(def => def.DefaultUser);
            //    var dstProp = d.GetExpressionValue(ct => ct.GetDefaultUserLoginName());

            //    var isValid = srcProp.Value.ToString().Replace("\\", "/") ==
            //                dstProp.Value.ToString().Replace("\\", "/");

            //    return new PropertyValidationResult
            //    {
            //        Tag = p.Tag,
            //        Src = srcProp,
            //        Dst = dstProp,
            //        IsValid = isValid
            //    };
            //});
        }
    }

    internal static class SPGroupExtensions
    {
        public static string GetOwnerLogin(this Group group)
        {
            if (!group.IsPropertyAvailable("Owner"))
            {
                var owner = group.Owner;

                group.Context.Load(owner, g => g.LoginName);
                group.Context.ExecuteQueryWithTrace();

                return owner.LoginName;
            }

            return group.Owner.LoginName;
        }

        public static string GetDefaultUserLoginName(this Group group)
        {
            if (!group.IsPropertyAvailable("Users"))
            {
                var users = group.Users;

                group.Context.Load(users, g => g.Include(gg => gg.LoginName));
                group.Context.ExecuteQueryWithTrace();

                return users[0].LoginName;
            }

            return group.Users[0].LoginName;
        }
    }
}
