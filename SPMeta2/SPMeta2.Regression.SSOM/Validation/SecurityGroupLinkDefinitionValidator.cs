using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityGroupLinkDefinitionValidator : SecurityGroupLinkModelHandler
    {
        #region methods

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);
            var spObject = ResolveSecurityGroup(web, definition);

            var assert = ServiceFactory.AssertService
                    .NewAssert(definition, spObject)
                          .ShouldNotBeNull(spObject)
                          .ShouldBeEqual(m => m.SecurityGroupName, o => o.Name);

            //TraceUtils.WithScope(traceScope =>
            //{
            //    traceScope.WriteLine(string.Format("Validate model:[{0}] securableObject:[{1}]", definition, spObject));

            //    traceScope.WithTraceIndent(trace =>
            //    {
            //        // asserting it exists
            //        trace.WriteLine(string.Format("Validating existance..."));

            //        Assert.IsTrue(securableObject
            //                                .RoleAssignments
            //                                .OfType<SPRoleAssignment>()
            //                                .FirstOrDefault(a => a.Member.ID == spObject.ID) != null);

            //        trace.WriteLine(string.Format("RoleAssignments for security group link [{0}] exists.", definition.SecurityGroupName));
            //    });
            //});
        }

        #endregion
    }
}
