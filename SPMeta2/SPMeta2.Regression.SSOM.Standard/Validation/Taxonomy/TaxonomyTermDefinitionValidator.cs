using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class TaxonomyTermDefinitionValidator : TaxonomyTermModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termSetModelHost = modelHost.WithAssertAndCast<TermSetModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            var spObject = FindTermInTermSet(termSetModelHost.HostTermSet, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Name)
                                 .ShouldBeEqual(m => m.Description, o => o.GetDescription());

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
