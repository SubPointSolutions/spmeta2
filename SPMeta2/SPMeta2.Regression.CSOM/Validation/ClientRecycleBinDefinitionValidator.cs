using System.Linq;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientRecycleBinDefinitionValidator : ClearRecycleBinModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ClearRecycleBinDefinition>("model", value => value.RequireNotNull());

            var spObject = webHost.HostWeb;

            var context = spObject.Context;
            context.Load(spObject, s => s.RecycleBin);
            context.ExecuteQueryWithTrace();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);

            if (definition.DeleteAll)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.DeleteAll);
                    var isValid = true;

                    isValid = spObject.RecycleBin.Count == 0;

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
                assert.SkipProperty(o => o.DeleteAll, "DeleteAll is false");
            }
        }
    }

}
