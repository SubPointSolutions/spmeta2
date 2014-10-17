using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModeHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModeHost.HostList;
            var spObject = GetListItem(list, definition);

            if (!spObject.IsPropertyAvailable("Title"))
            {
                var context = spObject.Context;

                context.Load(spObject, o => o.DisplayName);
                context.ExecuteQuery();
            }

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject)
                                   .ShouldBeEqual(m => m.Title, o => o.DisplayName);
        }
    }
}
