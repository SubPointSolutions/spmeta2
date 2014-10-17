using System;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityRoleDefinitionValidator : SecurityRoleModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var site = siteModelHost.HostSite;

            var definition = model.WithAssertAndCast<SecurityRoleDefinition>("model", value => value.RequireNotNull());

            var web = site.RootWeb;

            var securityRoles = web.RoleDefinitions;
            var spObject = securityRoles[definition.Name];

            var assert = ServiceFactory.AssertService
                    .NewAssert(definition, spObject)
                          .ShouldBeEqual(m => m.Name, o => o.Name)
                          .ShouldBeEqual(m => m.Description, o => o.Description);

            assert
               .ShouldBeEqual((p, s, d) =>
               {
                   var srcProp = s.GetExpressionValue(def => def.BasePermissions);
                   var dstProp = d.GetExpressionValue(ct => ct.BasePermissions);

                   var hasCorrectRights = true;

                   foreach (var srcRight in s.BasePermissions)
                   {
                       var srcPermission = (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), srcRight);

                       var tmpRight = d.BasePermissions.HasFlag(srcPermission);

                       if (tmpRight == false)
                           hasCorrectRights = false;
                   }

                   return new PropertyValidationResult
                   {
                       Tag = p.Tag,
                       Src = srcProp,
                       Dst = dstProp,
                       IsValid = hasCorrectRights
                   };
               });



            //TraceUtils.WithScope(traceScope =>
            //{
            //    var securityRoles = web.RoleDefinitions;
            //    var spSecurityRole = securityRoles[definition.Name];

            //    traceScope.WriteLine(string.Format("Validate model:[{0}] security role:[{1}]", definition, spSecurityRole));

            //    // assert base properties
            //    traceScope.WithTraceIndent(trace =>
            //    {
            //        trace.WriteLine(string.Format("Validate Name: model:[{0}] security role:[{1}]", definition.Name, spSecurityRole.Name));
            //        Assert.AreEqual(definition.Name, spSecurityRole.Name);

            //        trace.WriteLine(string.Format("Validate Description: model:[{0}] security role:[{1}]", definition.Description, spSecurityRole.Description));
            //        Assert.AreEqual(definition.Description, spSecurityRole.Description);

            //        // validate base permissions
            //        trace.WriteLine(string.Format("Validate permissions..."));

            //        traceScope.WithTraceIndent(permissionTrace =>
            //        {
            //            foreach (var permission in definition.BasePermissions)
            //            {
            //                trace.WriteLine(string.Format("Validate permission presence: [{0}]", permission));

            //                var spPermission = (int)(SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), permission);
            //                Assert.IsTrue((spPermission & (int)spSecurityRole.BasePermissions) != 0);
            //            }
            //        });

            //    });
            //});
        }
    }
}
