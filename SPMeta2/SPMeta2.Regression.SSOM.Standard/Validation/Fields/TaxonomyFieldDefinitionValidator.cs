using System;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Fields
{
    public class TaxonomyFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(TaxonomyFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyFieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition) as TaxonomyField;


            // TODO
        }
    }
}
