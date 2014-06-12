using Microsoft.SharePoint.Client;
using System;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;


namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListItemFieldValueModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListItemFieldValueDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemFieldValueModelHost>("modelHost", value => value.RequireNotNull());
            var fieldValue = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            ProcessFieldValue(listItemModelHost.CurrentItem, fieldValue);
        }

        private void ProcessFieldValue(ListItem listItem, ListItemFieldValueDefinition fieldValue)
        {
            if (!string.IsNullOrEmpty(fieldValue.FieldName))
            {
                listItem[fieldValue.FieldName] = fieldValue.Value;
            }
        }

        #endregion
    }
}
