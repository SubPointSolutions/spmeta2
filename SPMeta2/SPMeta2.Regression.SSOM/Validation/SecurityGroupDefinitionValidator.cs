using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Assertion;
using SPMeta2.Regression.Utils;
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
                             .ShouldBeEqual(m => m.Description, o => o.Description);


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

                var isValid = srcProp.Value.ToString().Replace("\\", "/") ==
                            dstProp.Value.ToString().Replace("\\", "/");

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
            return (group.Owner as SPUser).LoginName;
        }

        public static string GetDefaultUserLoginName(this SPGroup group)
        {
            return group.Users[0].LoginName;
        }
    }
}
