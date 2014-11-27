using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListItemFieldValueDefinitionValidator : ListItemFieldValueModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var spObject = modelHost.WithAssertAndCast<SPListItem>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject);

            if (definition.FieldId.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.FieldId);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = object.Equals(s.Value, d[s.FieldId.Value])
                    };
                });

                assert.SkipProperty(m => m.Value, string.Format("Value validated with FieldId and actual value:[{0}]", spObject[definition.FieldId.Value]));
            }
            else
            {
                assert.SkipProperty(m => m.FieldId, "FieldId is null. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.FieldName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.FieldName);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = object.Equals(s.Value, d[s.FieldName])
                    };
                });

                assert.SkipProperty(m => m.Value, string.Format("Value validated with FieldName and actual value:[{0}]", spObject[definition.FieldName]));
            }
            else
            {
                assert.SkipProperty(m => m.FieldName, "FieldName is null. Skipping.");
            }
        }
    }
}
