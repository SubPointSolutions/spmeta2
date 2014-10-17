using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;

using SPMeta2.Regression.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListFieldLinkDefinitionValidator : ListFieldLinkModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListFieldLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var context = list.Context;

            var spObject = list.Fields.GetById(definition.FieldId);
            context.Load(spObject, o => o.Id);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                            .ShouldNotBeNull(spObject)
                                            .ShouldBeEqual(m => m.FieldId, o => o.Id);
        }
    }
}
