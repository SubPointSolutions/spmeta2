using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var listModeHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            var list = listModeHost.HostList;

            ValidateModel(list, listItemModel);
        }

        private void ValidateModel(List list, ListItemDefinition listItemModel)
        {

        }
    }
}
