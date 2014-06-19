using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityGroupDefinitionValidator : SecurityGroupModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var site = modelHost.WithAssertAndCast<SPSite>("modelHost", value => value.RequireNotNull());
            var securityGroupModel = model.WithAssertAndCast<SecurityGroupDefinition>("model", value => value.RequireNotNull());

            var web = site.RootWeb;

            TraceUtils.WithScope(traceScope =>
            {
                var securityGroups = web.SiteGroups;
                var spGroup = securityGroups[securityGroupModel.Name];

                traceScope.WriteLine(string.Format("Validate model:[{0}] field:[{1}]", securityGroupModel, spGroup));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate Name: model:[{0}] field:[{1}]", securityGroupModel.Name, spGroup.Name));
                    Assert.AreEqual(securityGroupModel.Name, spGroup.Name);

                    trace.WriteLine(string.Format("Validate Description: model:[{0}] field:[{1}]", securityGroupModel.Description, spGroup.Description));
                    Assert.AreEqual(securityGroupModel.Description, spGroup.Description);
                });
            });
        }
    }
}
