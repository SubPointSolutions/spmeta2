using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class WelcomePageDefinitionValidator : WelcomePageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<WelcomePageDefinition>("model", value => value.RequireNotNull());

            var spObject = ExtractFolderFromModelHost(modelHost);

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject);
        }

        #endregion
    }
}
