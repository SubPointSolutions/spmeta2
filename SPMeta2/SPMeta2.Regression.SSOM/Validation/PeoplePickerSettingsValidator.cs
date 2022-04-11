using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class PeoplePickerSettingsValidator : PeoplePickerSettingsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PeoplePickerSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentPeoplePickerSettings(webAppModelHost.HostWebApplication);

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                            .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.ActiveDirectoryCustomFilter))
                assert.ShouldBeEqual(m => m.ActiveDirectoryCustomFilter, o => o.ActiveDirectoryCustomFilter);
            else
                assert.SkipProperty(m => m.ActiveDirectoryCustomFilter, "ActiveDirectoryCustomFilter is NULL or empty. Skipping.");

            if (!string.IsNullOrEmpty(definition.ActiveDirectoryCustomQuery))
                assert.ShouldBeEqual(m => m.ActiveDirectoryCustomQuery, o => o.ActiveDirectoryCustomQuery);
            else
                assert.SkipProperty(m => m.ActiveDirectoryCustomQuery, "ActiveDirectoryCustomQuery is NULL or empty. Skipping.");

            if (definition.ActiveDirectoryRestrictIsolatedNameLevel.HasValue)
                assert.ShouldBeEqual(m => m.ActiveDirectoryRestrictIsolatedNameLevel, o => o.ActiveDirectoryRestrictIsolatedNameLevel);
            else
                assert.SkipProperty(m => m.ActiveDirectoryRestrictIsolatedNameLevel, "ActiveDirectoryRestrictIsolatedNameLevel is not set. Skipping.");

            if (definition.AllowLocalAccount.HasValue)
                assert.ShouldBeEqual(m => m.AllowLocalAccount, o => o.AllowLocalAccount);
            else
                assert.SkipProperty(m => m.AllowLocalAccount, "AllowLocalAccount is not set. Skipping.");

            if (definition.NoWindowsAccountsForNonWindowsAuthenticationMode.HasValue)
                assert.ShouldBeEqual(m => m.NoWindowsAccountsForNonWindowsAuthenticationMode, o => o.NoWindowsAccountsForNonWindowsAuthenticationMode);
            else
                assert.SkipProperty(m => m.NoWindowsAccountsForNonWindowsAuthenticationMode, "NoWindowsAccountsForNonWindowsAuthenticationMode is not set. Skipping.");

            if (definition.OnlySearchWithinSiteCollection.HasValue)
                assert.ShouldBeEqual(m => m.OnlySearchWithinSiteCollection, o => o.OnlySearchWithinSiteCollection);
            else
                assert.SkipProperty(m => m.OnlySearchWithinSiteCollection, "OnlySearchWithinSiteCollection is not set. Skipping.");

            if (definition.PeopleEditorOnlyResolveWithinSiteCollection.HasValue)
                assert.ShouldBeEqual(m => m.PeopleEditorOnlyResolveWithinSiteCollection, o => o.PeopleEditorOnlyResolveWithinSiteCollection);
            else
                assert.SkipProperty(m => m.PeopleEditorOnlyResolveWithinSiteCollection, "PeopleEditorOnlyResolveWithinSiteCollection is not set. Skipping.");

            if (definition.ActiveDirectorySearchTimeout.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.ActiveDirectorySearchTimeout);
                    var dsrProp = d.GetExpressionValue(m => m.ActiveDirectorySearchTimeout);

                    var isValid = s.ActiveDirectorySearchTimeout.Value.TotalSeconds ==
                                  d.ActiveDirectorySearchTimeout.TotalSeconds;

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
                assert.SkipProperty(m => m.ActiveDirectorySearchTimeout, "ActiveDirectorySearchTimeout is not set. Skipping.");
        }
    }
}
