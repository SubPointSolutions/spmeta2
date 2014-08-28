using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class BusinessDataFieldDefinitionValidator : BusinessDataFieldModelHandler
    {
        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var fieldModel = model.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());

            ValidateBusineddDataField(modelHost, siteModelHost.HostSite, fieldModel);

        }

        private void ValidateBusineddDataField(object modelHost, Site site, BusinessDataFieldDefinition fieldModel)
        {
            // TODO
        }
    }
}
