using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class BreakRoleInheritanceDefinitionValidator : BreakRoleInheritanceModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var breakRoleInheritanceModel = model.WithAssertAndCast<BreakRoleInheritanceDefinition>("model", value => value.RequireNotNull());

            ValidateRoleInheritance(modelHost, securableObject, breakRoleInheritanceModel);
        }

        private void ValidateRoleInheritance(object modelHost, Microsoft.SharePoint.SPSecurableObject securableObject, BreakRoleInheritanceDefinition breakRoleInheritanceModel)
        {
            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} BreakRoleInheritanceModel:{1}", securableObject, breakRoleInheritanceModel));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine("Validating HasUniqueRoleAssignments. Should be true.");
                    trace.WriteLine("SPSecurableObject.HasUniqueRoleAssignments:" + securableObject.HasUniqueRoleAssignments);

                    Assert.AreEqual(securableObject.HasUniqueRoleAssignments, true);
                });
            });
        }
    }
}
