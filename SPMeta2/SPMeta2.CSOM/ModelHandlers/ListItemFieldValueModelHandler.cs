using Microsoft.SharePoint.Client;
using System;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;

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

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemFieldValueModelHost>("modelHost", value => value.RequireNotNull());
            var fieldValue = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            ProcessFieldValue(listItemModelHost, listItemModelHost.CurrentItem, fieldValue);
        }

        private void ProcessFieldValue(ListItemFieldValueModelHost modelHost, ListItem listItem, ListItemFieldValueDefinition fieldValue)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = listItem,
                ObjectType = typeof(ListItem),
                ObjectDefinition = fieldValue,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(fieldValue.FieldName))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Processing field value with Name: [{0}] and value: [{1}]",
                    new object[]
                    {
                        fieldValue.FieldName,
                        fieldValue.Value
                    });

                listItem[fieldValue.FieldName] = fieldValue.Value;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = listItem,
                ObjectType = typeof(ListItem),
                ObjectDefinition = fieldValue,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
