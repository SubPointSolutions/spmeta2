using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityRoleDefinitionValidator : SecurityRoleModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var securityRoleModel = model.WithAssertAndCast<SecurityRoleDefinition>("model", value => value.RequireNotNull());

            var context = web.Context;

            // well, this should be pulled up to the site handler and init Load/Exec query
            context.Load(web, tmpWeb => tmpWeb.SiteGroups);
            context.ExecuteQuery();

            var currentRole = FindRoleDefinition(web.RoleDefinitions, securityRoleModel.Name);

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] security role:[{1}]", securityRoleModel, currentRole));
                Assert.IsNotNull(currentRole);

                traceScope.WithTraceIndent(trace =>
                {
                    traceScope.WriteLine(string.Format("Validate Name: model:[{0}] list view:[{1}]", securityRoleModel.Name, currentRole.Name));
                    Assert.AreEqual(securityRoleModel.Name, currentRole.Name);

                    traceScope.WriteLine(string.Format("Validate Description: model:[{0}] list view:[{1}]", securityRoleModel.Description, currentRole.Description));
                    Assert.AreEqual(securityRoleModel.Description, currentRole.Description);

                    // validate base permissions
                    trace.WriteLine(string.Format("Validate permissions..."));

                    // 
                    context.Load(currentRole, r => r.BasePermissions);
                    context.ExecuteQuery();

                    traceScope.WithTraceIndent(permissionTrace =>
                    {
                        foreach (var permission in securityRoleModel.BasePermissions)
                        {
                            trace.WriteLine(string.Format("Validate permission presence: [{0}]", permission));

                            var spPermission = (PermissionKind)Enum.Parse(typeof(PermissionKind), permission);
                            Assert.IsTrue(currentRole.BasePermissions.Has(spPermission));
                        }
                    });
                });
            });
        }
    }
}
