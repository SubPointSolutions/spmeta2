using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var spObject = GetListItem(list, definition);

            var assert = ServiceFactory.AssertService
                             .NewAssert(definition, spObject)
                                   .ShouldNotBeNull(spObject)
                                   .ShouldBeEqual(m => m.Title, o => o.Title);
        }
    }
}
