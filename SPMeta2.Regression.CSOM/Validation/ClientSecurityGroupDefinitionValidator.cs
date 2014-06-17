using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSecurityGroupDefinitionValidator : SecurityGroupModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostSite.RootWeb;
            var securityGroupModel = model as SecurityGroupDefinition;

            var context = web.Context;

            // well, this should be pulled up to the site handler and init Load/Exec query
            context.Load(web, tmpWeb => tmpWeb.SiteGroups);
            context.ExecuteQuery();

            var currentGroup = FindSecurityGroupByTitle(web.SiteGroups, securityGroupModel.Name);

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] list view:[{1}]", securityGroupModel, currentGroup));
                Assert.IsNotNull(currentGroup);

                traceScope.WithTraceIndent(trace =>
                {
                    traceScope.WriteLine(string.Format("Validate Title: model:[{0}] list view:[{1}]", securityGroupModel.Name, currentGroup.Title));
                    Assert.AreEqual(securityGroupModel.Name, currentGroup.Title);

                    traceScope.WriteLine(string.Format("Validate Description: model:[{0}] list view:[{1}]", securityGroupModel.Description, currentGroup.Description));
                    Assert.AreEqual(securityGroupModel.Description, currentGroup.Description);
                });
            });
        }
    }
}
