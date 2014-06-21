using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListItemDefinitionValidator : ListItemModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var listItemModel = model.WithAssertAndCast<ListItemDefinition>("model", value => value.RequireNotNull());

            ValidateListItem(list, listItemModel);
        }

        private void ValidateListItem(SPList list, ListItemDefinition listItemModel)
        {
            // TODO
        }
    }
}
