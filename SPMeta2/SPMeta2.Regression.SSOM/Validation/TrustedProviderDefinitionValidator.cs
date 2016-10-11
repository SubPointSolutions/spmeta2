using System.Security.Cryptography.X509Certificates;
using Microsoft.SharePoint.Administration.Claims;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class TrustedAccessProviderDefinitionValidator : TrustedAccessProviderModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<TrustedAccessProviderDefinition>("model", value => value.RequireNotNull());

            SPTrustedAccessProvider spObject = GetCurrentTrustedAccessProvider(modelHost, definition);

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject);


            assert.ShouldBeEqual(m => m.Name, o => o.Name);

            if (!string.IsNullOrEmpty(definition.Description))
            {
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            }
            else
            {
                assert.SkipProperty(m => m.Description);
            }

            if (!string.IsNullOrEmpty(definition.MetadataEndPoint))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.MetadataEndPoint);

                    var isValid = s.MetadataEndPoint.ToUpper() == d.MetadataEndPoint.ToString().ToUpper();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.MetadataEndPoint);
            }

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Certificate);

                var isValid = new X509Certificate2(s.Certificate).Thumbprint == d.SigningCertificate.Thumbprint;

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
