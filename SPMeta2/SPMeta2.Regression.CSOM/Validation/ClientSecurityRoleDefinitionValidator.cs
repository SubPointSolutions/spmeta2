using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityRoleDefinitionValidator : SecurityRoleModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var definition = model.WithAssertAndCast<SecurityRoleDefinition>("model", value => value.RequireNotNull());

            var context = web.Context;

            // well, this should be pulled up to the site handler and init Load/Exec query
            context.Load(web, tmpWeb => tmpWeb.SiteGroups);
            context.ExecuteQuery();

            var spObject = FindRoleDefinition(web.RoleDefinitions, definition.Name);

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
                       var srcPermission = (PermissionKind)Enum.Parse(typeof(PermissionKind), srcRight);

                       var tmpRight = d.BasePermissions.Has(srcPermission);

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
        }
    }
}
