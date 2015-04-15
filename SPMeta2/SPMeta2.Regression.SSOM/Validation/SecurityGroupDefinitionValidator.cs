using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityGroupDefinitionValidator : SecurityGroupModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
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


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Owner);
                var dstProp = d.GetExpressionValue(ct => ct.GetOwnerLogin());

                var isValid = srcProp.Value.ToString().ToUpper().Replace("\\", "/") ==
                              dstProp.Value.ToString().ToUpper().Replace("\\", "/");


                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.DefaultUser);
                var dstProp = d.GetExpressionValue(ct => ct.GetDefaultUserLoginName());

                var isValid = srcProp.Value.ToString().ToUpper().Replace("\\", "/") ==
                            dstProp.Value.ToString().ToUpper().Replace("\\", "/");

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });
        }
    }

    internal static class SPGroupExtensions
    {
        public static string GetOwnerLogin(this SPGroup group)
        {
            if (group.Owner is SPGroup)
                return (group.Owner as SPGroup).LoginName;

            if (group.Owner is SPUser)
                return (group.Owner as SPUser).LoginName;

            throw new SPMeta2Exception(string.Format("Cannot get LoginName for object:[{0}] of type:[{1}]",
                group, group.Owner != null ? group.Owner.GetType().ToString() : "NULL"));
        }

        public static string GetDefaultUserLoginName(this SPGroup group)
        {
            return group.Users[0].LoginName;
        }
    }
}
