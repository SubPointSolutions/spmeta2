using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System.Linq;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.Regression.CSOM.Extensions;

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
                    .ShouldBeEqual(m => m.OnlyAllowMembersViewMembership, o => o.OnlyAllowMembersViewMembership)

                    .ShouldBeEqualIfNotNullOrEmpty(m => m.Description, o => o.Description)

                    .ShouldBeEqualIfHasValue(m => m.AllowMembersEditMembership, o => o.AllowMembersEditMembership)
                    .ShouldBeEqualIfHasValue(m => m.AllowRequestToJoinLeave, o => o.AllowRequestToJoinLeave)
                    .ShouldBeEqualIfHasValue(m => m.AutoAcceptRequestToJoinLeave, o => o.AutoAcceptRequestToJoinLeave);

                if (!string.IsNullOrEmpty(definition.Owner))
                {

                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.Owner);
                        var dstProp = d.GetExpressionValue(ct => ct.GetOwnerLogin());

                        var srcOwner = dstProp.Value.ToString().ToUpper().Replace("\\", "/");
                        var dstOwner = dstProp.Value.ToString().ToUpper().Replace("\\", "/");

                        var isValid = dstProp.Value.ToString().ToUpper().Replace("\\", "/").EndsWith(
                            srcProp.Value.ToString().ToUpper().Replace("\\", "/"));

                        if (!isValid)
                        {
                            isValid = dstOwner.Contains(srcOwner);
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.Owner, "Owner is null or empty");
                }

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

                var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

                assert
                    .ShouldNotBeNull(spObject)

                    .SkipProperty(m => m.Name)
                    .SkipProperty(m => m.AllowMembersEditMembership)
                    .SkipProperty(m => m.AllowRequestToJoinLeave)
                    .SkipProperty(m => m.AutoAcceptRequestToJoinLeave)
                    .SkipProperty(m => m.DefaultUser)
                    .SkipProperty(m => m.Description)
                    .SkipProperty(m => m.IsAssociatedMemberGroup)
                    .SkipProperty(m => m.IsAssociatedVisitorsGroup)
                    .SkipProperty(m => m.IsAssociatedOwnerGroup)
                    .SkipProperty(m => m.OnlyAllowMembersViewMembership)
                    .SkipProperty(m => m.Owner);
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
}
