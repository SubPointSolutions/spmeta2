using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListItemFieldValuesDefinitionValidator : ListItemFieldValuesModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var spObject = modelHost.WithAssertAndCast<SPListItem>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListItemFieldValuesDefinition>("model", value => value.RequireNotNull());

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject);

            if (definition.Values.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Values);

                    var isValid = true;

                    foreach (var value in definition.Values)
                    {
                        if (!string.IsNullOrEmpty(value.FieldName))
                        {
                            if (spObject[value.FieldName].ToString() != value.Value.ToString())
                            {
                                isValid = false;
                                break;
                            }
                        }
                        else if (value.FieldId.HasGuidValue())
                        {
                            if (spObject[value.FieldId.Value].ToString() != value.Value.ToString())
                            {
                                isValid = false;
                                break;
                            }
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
            else
            {
                assert.SkipProperty(m => m.Values, "Values.Count == 0. Skipping");
            }
        }
    }
}
