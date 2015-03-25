using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemFieldValuesDefinitionValidator : ListItemFieldValuesModelHandler
    {
        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListItemFieldValuesDefinition>("model", value => value.RequireNotNull());

            var spObject = listItemModelHost.HostListItem;

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
                        if (spObject[value.FieldName].ToString() != value.Value.ToString())
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
            else
            {
                assert.SkipProperty(m => m.Values, "Values.Count == 0. Skipping");
            }
        }
    }
}
