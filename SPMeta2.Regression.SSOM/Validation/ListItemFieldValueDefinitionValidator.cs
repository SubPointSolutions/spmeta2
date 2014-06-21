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
    public class ListItemFieldValueDefinitionValidator : ListItemFieldValueModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<SPListItem>("modelHost", value => value.RequireNotNull());
            var fieldValue = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            ValidateFieldValue(list, list, fieldValue);
        }

        private void ValidateFieldValue(SPListItem host, SPListItem list, ListItemFieldValueDefinition fieldValueModel)
        {
            // TODO
        }
    }
}
