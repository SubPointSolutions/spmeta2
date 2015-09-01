using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers.Assertion;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SupportedUICultureDefinitionValidator : SupportedUICultureModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedHost = modelHost.WithAssertAndCast<WebModelHost>("model", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SupportedUICultureDefinition>("model", value => value.RequireNotNull());

            var spObject = typedHost.HostWeb;

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                       .ShouldNotBeNull(spObject);

            // LCID
            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.LCID);
                var supportedLanguages = spObject.SupportedUICultures;

                var isValid = supportedLanguages.Any(l => l.LCID == s.LCID);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });
        }

        #endregion
    }
}
