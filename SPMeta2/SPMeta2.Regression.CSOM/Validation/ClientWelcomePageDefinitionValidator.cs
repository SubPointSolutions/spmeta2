using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWelcomePageDefinitionValidator : WelcomePageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<WelcomePageDefinition>("model", value => value.RequireNotNull());

            var spObject = ExtractFolderFromModelHost(modelHost);
            var context = spObject.Context;

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Url);

                var src = UrlUtility.RemoveStartingSlash(s.Url).ToUpper();
                var dst = UrlUtility.RemoveStartingSlash(d.WelcomePage).ToUpper();

                var isValid = dst.EndsWith(src);

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
