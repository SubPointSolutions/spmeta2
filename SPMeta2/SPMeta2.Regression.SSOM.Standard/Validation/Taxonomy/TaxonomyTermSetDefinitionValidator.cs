using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.SSOM.Standard.ModelHosts;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Taxonomy
{
    public class TaxonomyTermSetDefinitionValidator : TaxonomyTermSetModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termGroupModelHost = modelHost.WithAssertAndCast<TermGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermSetDefinition>("model", value => value.RequireNotNull());

            var spObject = FindTermSet(termGroupModelHost.HostGroup, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                //.ShouldBeEqual(m => m.IsAvailableForTagging, o => o.IsAvailableForTagging)
                //.ShouldBeEqual(m => m.IsOpenForTermCreation, o => o.IsOpenForTermCreation)
                .ShouldBeEqual(m => m.Name, o => o.Name);

            if (definition.IsAvailableForTagging.HasValue)
                assert.ShouldBeEqual(m => m.IsAvailableForTagging, o => o.IsAvailableForTagging);
            else
                assert.SkipProperty(m => m.IsAvailableForTagging, "IsAvailableForTagging is null. Skipping property.");

            if (definition.IsOpenForTermCreation.HasValue)
                assert.ShouldBeEqual(m => m.IsOpenForTermCreation, o => o.IsOpenForTermCreation);
            else
                assert.SkipProperty(m => m.IsOpenForTermCreation, "IsOpenForTermCreation is null. Skipping property.");

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.Description);
            else
                assert.SkipProperty(m => m.Description, "Description is null. Skipping property.");

            assert.SkipProperty(m => m.LCID, "Can't get LCID withon OM. Should be set while provision.");

            if (definition.Id.HasValue)
            {
                assert.ShouldBeEqual(m => m.Id, o => o.Id);
            }
            else
            {
                assert.SkipProperty(m => m.Id, "Id is null. Skipping property.");
            }
        }
    }
}
