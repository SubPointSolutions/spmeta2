using SPMeta2.CSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientListItemFieldValueDefinitionValidator : ListItemFieldValueModelHandler
    {
        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemFieldValueModelHost>("modelHost", value => value.RequireNotNull());
            var fieldValue = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            ValidateFieldValue(listItemModelHost.CurrentItem, fieldValue);
        }

        private void ValidateFieldValue(Microsoft.SharePoint.Client.ListItem listItem, ListItemFieldValueDefinition fieldValue)
        {
            // TODO
        }
             
    }
}
