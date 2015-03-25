using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListItemFieldValuesModelHandler : SSOMModelHandlerBase
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
            var list = modelHost.WithAssertAndCast<SPListItem>("modelHost", value => value.RequireNotNull());
            var fieldValue = model.WithAssertAndCast<ListItemFieldValuesDefinition>("model", value => value.RequireNotNull());

            ProcessFieldValue(list, list, fieldValue);
        }

        private void ProcessFieldValue(object modelHost, SPListItem listItem, ListItemFieldValuesDefinition fieldValue)
        {
            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = listItem,
                ObjectType = typeof(SPListItem),
                ObjectDefinition = fieldValue,
                ModelHost = modelHost
            });

            foreach (var value in fieldValue.Values)
            {
                if (value.FieldId.HasGuidValue())
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Processing field value with ID: [{0}] and value: [{1}]",
                       new object[]
                    {
                        value.FieldId,
                        value.Value
                    });

                    listItem[value.FieldId.Value] = value.Value;
                }
                else if (!string.IsNullOrEmpty(value.FieldName))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Processing field value with Name: [{0}] and value: [{1}]",
                       new object[]
                    {
                        value.FieldName,
                        value.Value
                    });

                    listItem[value.FieldName] = value.Value;
                }
                else
                {
                    throw new SPMeta2Exception("Either FieldId or FieldName must be provided.");
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = listItem,
                ObjectType = typeof(SPListItem),
                ObjectDefinition = fieldValue,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
