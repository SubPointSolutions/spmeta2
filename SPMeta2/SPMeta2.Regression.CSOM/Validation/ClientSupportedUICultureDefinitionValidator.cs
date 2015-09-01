using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientSupportedUICultureDefinitionValidator : SupportedUICultureModelHandler
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

            var context = spObject.Context;

            context.Load(spObject, w => w.SupportedUILanguageIds);
            context.ExecuteQuery();

            // LCID
            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.LCID);
                var supportedLanguages = spObject.SupportedUILanguageIds;

                var isValid = supportedLanguages.Contains(s.LCID);

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
