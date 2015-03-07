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
    public class TaxonomyTermLabelDefinitionValidator : TaxonomyTermLabelModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termModelHost = modelHost.WithAssertAndCast<TermModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermLabelDefinition>("model", value => value.RequireNotNull());

            var spObject = FindLabelInTerm(termModelHost.HostTerm, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Value)
                                 .ShouldBeEqual(m => m.LCID, o => o.Language)
                                 .ShouldBeEqual(m => m.IsDefault, o => o.IsDefaultForLanguage);
        }
    }
}
