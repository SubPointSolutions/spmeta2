﻿using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class ClientTaxonomyStoreDefinitionValidator : TaxonomyTermStoreModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermStoreDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var spObject = FindTermStore(siteModelHost, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);

            if (definition.Id.HasValue)
                assert.ShouldBeEqual(m => m.Id, o => o.Id);
            else
                assert.SkipProperty(m => m.Id, "ID is NULL. Skipping.");

            if (!string.IsNullOrEmpty(definition.Name))
                assert.ShouldBeEqual(m => m.Name, o => o.Name);
            else
                assert.SkipProperty(m => m.Name, "Name is NULL. Skipping.");

            if (definition.UseDefaultSiteCollectionTermStore.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.UseDefaultSiteCollectionTermStore);
                    var session = TaxonomySession.GetTaxonomySession(siteModelHost.HostClientContext);

                    var client = siteModelHost.HostClientContext;
                    var termStore = session.GetDefaultSiteCollectionTermStore();

                    client.Load(termStore, t => t.Id);
                    client.Load(termStore, t => t.Name);

                    siteModelHost.HostClientContext.ExecuteQueryWithTrace();

                    var isValid = termStore.Id == spObject.Id;

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
                assert.SkipProperty(m => m.UseDefaultSiteCollectionTermStore, "UseDefaultSiteCollectionTermStore is NULL. Skipping.");


        }
    }
}
