using Microsoft.SharePoint.Client;
using System;
using System.Linq;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListItemFieldValuesModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListItemFieldValuesDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var fieldValues = model.WithAssertAndCast<ListItemFieldValuesDefinition>("model", value => value.RequireNotNull());

            if (fieldValues.Values.Any(v => v.FieldId.HasValue))
                throw new SPMeta2NotSupportedException("FieldId is defines. CSOM API does not support FieldId based values. Please use FieldName (InternalFieldName) instead.");

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            ProcessFieldValues(listItemModelHost, listItemModelHost.HostListItem, fieldValues);
        }

        private void ProcessFieldValues(ListItemModelHost modelHost, ListItem listItem, ListItemFieldValuesDefinition fieldValues)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = listItem,
                ObjectType = typeof(ListItem),
                ObjectDefinition = fieldValues,
                ModelHost = modelHost
            });

            foreach (var fieldValue in fieldValues.Values)
            {
                if (!string.IsNullOrEmpty(fieldValue.FieldName))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                        "Processing field value with Name: [{0}] and value: [{1}]",
                        new object[]
                        {
                            fieldValue.FieldName,
                            fieldValue.Value
                        });

                    listItem[fieldValue.FieldName] = fieldValue.Value;
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = listItem,
                ObjectType = typeof(ListItem),
                ObjectDefinition = fieldValues,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
