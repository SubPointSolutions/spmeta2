using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class BusinessDataFieldDefinitionValidator : FieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(BusinessDataFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition) as SPBusinessDataField;


            var assert = ServiceFactory.AssertService
                         .NewAssert(definition, spObject)
                               .ShouldBeEqual(m => m.SystemInstanceName, o => o.SystemInstanceName)
                               .ShouldBeEqual(m => m.EntityNamespace, o => o.EntityNamespace)
                               .ShouldBeEqual(m => m.EntityName, o => o.EntityName)
                               .ShouldBeEqual(m => m.BdcFieldName, o => o.BdcFieldName);
        }
    }
}
