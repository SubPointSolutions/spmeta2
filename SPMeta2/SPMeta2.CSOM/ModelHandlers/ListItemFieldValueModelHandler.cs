using Microsoft.SharePoint.Client;
using System;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;
using SPMeta2.Exceptions;

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
            var fieldValue = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            if (fieldValue.FieldId.HasValue)
                throw new SPMeta2NotSupportedException("ListItemFieldValueDefinition.FieldId. CSOM API does not support FieldId based values. Please use FieldName (IntrnalFieldName) instaed");

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            ProcessFieldValue(listItemModelHost, listItemModelHost.HostListItem, fieldValue);
        }

        private void ProcessFieldValue(ListItemModelHost modelHost, ListItem listItem, ListItemFieldValueDefinition fieldValue)
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
