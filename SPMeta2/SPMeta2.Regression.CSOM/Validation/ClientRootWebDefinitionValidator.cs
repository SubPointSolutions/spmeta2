using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientRootWebDefinitionValidator : RootWebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var spObject = GetCurrentObject(siteModelHost, definition);

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

            if (string.IsNullOrEmpty(definition.Title))
                assert.SkipProperty(m => m.Title, "Title is null or empty");
            else
                assert.ShouldBeEqual(m => m.Title, o => o.Title);

            if (string.IsNullOrEmpty(definition.Description))
                assert.SkipProperty(m => m.Description, "Description is null or empty");
            else
                assert.ShouldBeEqual(m => m.Description, o => o.Description);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var context = d.Context;

                if (!d.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    context.Load(d, o => o.ServerRelativeUrl);
                    context.ExecuteQuery();
                }

                if (!site.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    site.Context.Load(site, o => o.ServerRelativeUrl);
                    site.Context.ExecuteQuery();
                }

                var isValid = d.ServerRelativeUrl.ToUpper() == site.ServerRelativeUrl.ToUpper();

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = null,
                    Dst = null,
                    IsValid = isValid,
                    Message = "Checking if IsRootWeb == TRUE"
                };
            });
        }
    }

}
