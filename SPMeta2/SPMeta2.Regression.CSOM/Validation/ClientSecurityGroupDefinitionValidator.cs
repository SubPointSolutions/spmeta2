using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Assertion;
using SPMeta2.Regression.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityGroupDefinitionValidator : SecurityGroupModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var definition = model as SecurityGroupDefinition;

            var context = web.Context;

            // well, this should be pulled up to the site handler and init Load/Exec query
            context.Load(web, tmpWeb => tmpWeb.SiteGroups);
            context.ExecuteQuery();

            var spObject = FindSecurityGroupByTitle(web.SiteGroups, definition.Name);

            var assert = ServiceFactory.AssertService
                       .NewAssert(definition, spObject)
                             .ShouldNotBeNull(spObject)
                             .ShouldBeEqual(m => m.Name, o => o.Title)
                             .ShouldBeEqual(m => m.Description, o => o.Description);


            // TODO
            // skip the owner as it is different from onprem to Office 365 instances
            // later should be done depending on the current login credentials

            assert.SkipProperty(m => m.Owner, "Owner is skipped. Validation will be implemented in further versions of SPMeta2 library.");

            //assert.ShouldBeEqual((p, s, d) =>
            //{
            //    var srcProp = s.GetExpressionValue(def => def.Owner);
            //    var dstProp = d.GetExpressionValue(ct => ct.GetOwnerLogin());

            //    // hope domain is the same, just check the username

            //    var isValid = dstProp.Value.ToString().ToUpper().Replace("\\", "/").EndsWith(
            //                  srcProp.Value.ToString().ToUpper().Replace("\\", "/"));

            //    return new PropertyValidationResult
            //    {
            //        Tag = p.Tag,
            //        Src = srcProp,
            //        Dst = dstProp,
            //        IsValid = isValid
            //    };
            //});

            assert.SkipProperty(m => m.DefaultUser, "DefaultUser cannot be setup via CSOM API. Skipping.");

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
                group.Context.ExecuteQuery();

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
                group.Context.ExecuteQuery();

                return users[0].LoginName;
            }

            return group.Users[0].LoginName;
        }
    }
}
