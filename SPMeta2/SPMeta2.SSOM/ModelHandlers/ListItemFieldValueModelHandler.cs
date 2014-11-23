using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListItemFieldValueModelHandler : SSOMModelHandlerBase
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
            var list = modelHost.WithAssertAndCast<SPListItem>("modelHost", value => value.RequireNotNull());
            var fieldValue = model.WithAssertAndCast<ListItemFieldValueDefinition>("model", value => value.RequireNotNull());

            ProcessFieldValue(list, list, fieldValue);
        }

        private void ProcessFieldValue(object modelHost, SPListItem listItem, ListItemFieldValueDefinition fieldValue)
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

            if (fieldValue.FieldId.HasValue)
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Processing field value with ID: [{0}] and value: [{1}]",
                   new object[]
                    {
                        fieldValue.FieldId,
                        fieldValue.Value
                    });

                listItem[fieldValue.FieldId.Value] = fieldValue.Value;
            }
            else if (!string.IsNullOrEmpty(fieldValue.FieldName))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Processing field value with Name: [{0}] and value: [{1}]",
                   new object[]
                    {
                        fieldValue.FieldName,
                        fieldValue.Value
                    });

                listItem[fieldValue.FieldName] = fieldValue.Value;
            }
            else
            {
                throw new SPMeta2Exception("Either FieldId or FieldName must be provided.");
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
