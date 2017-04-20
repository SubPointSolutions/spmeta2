using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityGroupDefinitionValidator : SecurityGroupModelHandler
    {
        protected string GetSafeUserLogin(string userLoginString)
        {
            var result = userLoginString.ToString().ToUpper().Replace("\\", "/");

            if (result.Contains("|"))
                result = result.Split('|')[1];

            if (result.Contains('\\'))
                result = result.Split('\\')[1];

            if (result.Contains('/'))
                result = result.Split('/')[1];

            return result;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (modelHost is SiteModelHost)
            {
                var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
                var site = siteModelHost.HostSite;

                var definition = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

                var web = site.RootWeb;

                var securityGroups = web.SiteGroups;
                var spObject = securityGroups[definition.Name];

                var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Name)
                                 .ShouldBeEqual(m => m.OnlyAllowMembersViewMembership, o => o.OnlyAllowMembersViewMembership);
                //ShouldBeEqual(m => m.Description, o => o.Description);

                if (!string.IsNullOrEmpty(definition.Description))
                    assert.ShouldBeEqual(m => m.Description, o => o.Description);
                else
                    assert.SkipProperty(m => m.Description, "Description is NULL. Skipping.");

                if (definition.AllowMembersEditMembership.HasValue)
                    assert.ShouldBeEqual(m => m.AllowMembersEditMembership, o => o.AllowMembersEditMembership);
                else
                    assert.SkipProperty(m => m.AllowMembersEditMembership, "AllowMembersEditMembership is NULL. Skipping.");

                if (definition.AllowRequestToJoinLeave.HasValue)
                    assert.ShouldBeEqual(m => m.AllowRequestToJoinLeave, o => o.AllowRequestToJoinLeave);
                else
                    assert.SkipProperty(m => m.AllowRequestToJoinLeave, "AllowRequestToJoinLeave is NULL. Skipping.");

                if (definition.AutoAcceptRequestToJoinLeave.HasValue)
                    assert.ShouldBeEqual(m => m.AutoAcceptRequestToJoinLeave, o => o.AutoAcceptRequestToJoinLeave);
                else
                    assert.SkipProperty(m => m.AutoAcceptRequestToJoinLeave, "AutoAcceptRequestToJoinLeave is NULL. Skipping.");

                if (!string.IsNullOrEmpty(definition.DefaultUser))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.Owner);
                        var dstProp = d.GetExpressionValue(ct => ct.GetOwnerLogin());

                        var srcUserLogin = GetSafeUserLogin(srcProp.Value.ToString());
                        var dstsUserLogin = GetSafeUserLogin(dstProp.Value.ToString());

                        var isValid = srcUserLogin == dstsUserLogin;

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
                    assert.SkipProperty(m => m.Owner, "");
                }

                if (!string.IsNullOrEmpty(definition.DefaultUser))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.DefaultUser);

                        // should be in the group
                        var userNames = d.Users.OfType<SPUser>()
                                               .Select(u => u.LoginName);

                        var isValid = false;

                        foreach (var userName in userNames)
                        {

                            var srcUserLogin = GetSafeUserLogin(srcProp.Value.ToString());
                            var dstsUserLogin = GetSafeUserLogin(userName);

                            if (srcUserLogin == dstsUserLogin)
                            {
                                isValid = true;
                                break;
                            }
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.DefaultUser, "");
                }
            }
            else if (modelHost is SecurityGroupModelHost)
            {
                // skip everything, just check if the group is therw

                var securityGroupModelHost = modelHost.WithAssertAndCast<SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
                var definition = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

                var webMember = securityGroupModelHost.SecurityGroup.ParentWeb.EnsureUser(definition.Name);
                var spObject = securityGroupModelHost.SecurityGroup.Users.OfType<SPPrincipal>()
                                                          .FirstOrDefault(u => u.ID == webMember.ID);

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


        }
    }
}
