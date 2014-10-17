using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHandlers.ContentTypes;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.ContentTypes
{
    public class UniqueContentTypeOrderDefinitionValidator : UniqueContentTypeOrderModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<UniqueContentTypeOrderDefinition>("model", value => value.RequireNotNull());
            var spObject = ExtractFolderFromHost(modelHost);

            ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);
        }
    }
}
