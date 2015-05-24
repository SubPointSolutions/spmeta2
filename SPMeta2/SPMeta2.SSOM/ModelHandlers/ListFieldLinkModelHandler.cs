using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListFieldLinkModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListFieldLinkDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var listFieldLinkModel = model.WithAssertAndCast<ListFieldLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            DeployListFieldLink(modelHost, list, listFieldLinkModel);
        }

        private void DeployListFieldLink(object modelHost, SPList list, ListFieldLinkDefinition listFieldLinkModel)
        {
            var existingListField = list.Fields
                                        .OfType<SPField>()
                                        .FirstOrDefault(f => f.Id == listFieldLinkModel.FieldId);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingListField,
                ObjectType = typeof(SPField),
                ObjectDefinition = listFieldLinkModel,
                ModelHost = modelHost
            });

            if (existingListField == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list field");

                var siteField = list.ParentWeb.AvailableFields[listFieldLinkModel.FieldId];
                //list.Fields.Add(siteField);

                var addFieldOptions = (SPAddFieldOptions)(int)listFieldLinkModel.AddFieldOptions;

                if ((siteField is SPFieldLookup) &&
                    (siteField as SPFieldLookup).IsDependentLookup)
                {
                    list.Fields.Add(siteField);
                }
                else
                {
                    list.Fields.AddFieldAsXml(siteField.SchemaXmlWithResourceTokens, listFieldLinkModel.AddToDefaultView, addFieldOptions);
                }

                existingListField = list.Fields[siteField.Id];
                ProcessListFieldLinkProperties(existingListField, listFieldLinkModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingListField,
                    ObjectType = typeof(SPField),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });

                existingListField.Update(false);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list field");

                ProcessListFieldLinkProperties(existingListField, listFieldLinkModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingListField,
                    ObjectType = typeof(SPField),
                    ObjectDefinition = listFieldLinkModel,
                    ModelHost = modelHost
                });

                existingListField.Update(false);
            }
        }

        protected virtual void ProcessListFieldLinkProperties(SPField existingListField, ListFieldLinkDefinition listFieldLinkModel)
        {
            if (!string.IsNullOrEmpty(listFieldLinkModel.DisplayName))
                existingListField.Title = listFieldLinkModel.DisplayName;

            if (listFieldLinkModel.Hidden.HasValue)
                existingListField.Hidden = listFieldLinkModel.Hidden.Value;

            if (listFieldLinkModel.Required.HasValue)
                existingListField.Required = listFieldLinkModel.Required.Value;

        }

        #endregion
    }
}
