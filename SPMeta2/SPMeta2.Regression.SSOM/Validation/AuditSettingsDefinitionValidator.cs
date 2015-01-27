using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class AuditSettingsDefinitionValidator : AuditSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<AuditSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentAuditObject(modelHost);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.AuditFlags);
                var isValid = true;

                foreach (var auditFlag in s.AuditFlags)
                {
                    var flag = (SPAuditMaskType)Enum.Parse(typeof(SPAuditMaskType), auditFlag);

                    if (!d.AuditFlags.HasFlag(flag))
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });

        }
    }

}
