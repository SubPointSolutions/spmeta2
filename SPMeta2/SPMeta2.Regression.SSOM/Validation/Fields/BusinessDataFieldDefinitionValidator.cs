using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation.Fields
{
    public class BusinessDataFieldDefinitionValidator : BusinessDataFieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());

            var site = typedModelHost.HostSite;
            var spObject = GetField(modelHost, definition) as SPBusinessDataField;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldBeEqual(m => m.Title, o => o.Title)
                                 .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                                 .ShouldBeEqual(m => m.Group, o => o.Group)
                                 .ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                                 .ShouldBeEqual(m => m.Id, o => o.Id)
                                 .ShouldBeEqual(m => m.Description, o => o.Description)
                                 .ShouldBeEqual(m => m.Required, o => o.Required)

                                 .ShouldBeEqual(m => m.SystemInstanceName, o => o.SystemInstanceName)
                                 .ShouldBeEqual(m => m.EntityNamespace, o => o.EntityNamespace)
                                 .ShouldBeEqual(m => m.EntityName, o => o.EntityName)
                                 .ShouldBeEqual(m => m.BdcFieldName, o => o.BdcFieldName);

        }
    }
}
