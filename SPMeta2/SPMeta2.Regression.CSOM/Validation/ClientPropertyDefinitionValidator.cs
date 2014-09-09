using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientPropertyDefinitionValidator : PropertyModelHandler
    {
        #region properties

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var properties = ExtractProperties(modelHost);
            var definition = model.WithAssertAndCast<PropertyDefinition>("model", value => value.RequireNotNull());

            var propertyValue = properties[definition.Key];

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, propertyValue)
                                            .ShouldNotBeNull(propertyValue);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Key);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = properties.FieldValues.ContainsKey(s.Key)
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Value);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = properties[s.Key] != null
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Overwrite);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = object.Equals(properties[s.Key], s.Value)
                };
            });
        }


        #endregion
    }
}
