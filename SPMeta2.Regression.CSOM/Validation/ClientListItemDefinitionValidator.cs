using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            ValidateModel(list, listItemModel);
        }

        private void ValidateModel(List list, ListItemDefinition listItemModel)
        {

        }
    }
}
