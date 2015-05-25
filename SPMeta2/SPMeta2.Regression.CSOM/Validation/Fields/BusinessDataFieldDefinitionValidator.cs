using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using System.Xml.Linq;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class BusinessDataFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(BusinessDataFieldDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var definition = model.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());
            var spObject = FindField(modelHost, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.SystemInstanceName, o => o.GetSystemInstanceName())
                .ShouldBeEqual(m => m.EntityNamespace, o => o.GetEntityNamespace())
                .ShouldBeEqual(m => m.EntityName, o => o.GetEntityName())
                .ShouldBeEqual(m => m.BdcFieldName, o => o.GetBdcFieldName());
        }
    }
}
