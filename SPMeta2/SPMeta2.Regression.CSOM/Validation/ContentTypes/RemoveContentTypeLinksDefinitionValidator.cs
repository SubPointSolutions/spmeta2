using SPMeta2.CSOM.ModelHandlers.ContentTypes;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.ContentTypes
{
    public class RemoveContentTypeLinksDefinitionValidator : RemoveContentTypeLinksModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<RemoveContentTypeLinksDefinition>("model", value => value.RequireNotNull());
            var spObject = ExtractListFromHost(modelHost);

            ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);
        }
    }
}
