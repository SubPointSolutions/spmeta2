using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Utils;
using EventReceiverDefinition = SPMeta2.Definitions.EventReceiverDefinition;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class EventReceiverModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(EventReceiverDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<EventReceiverDefinition>("model", value => value.RequireNotNull());

            if (modelHost is ListModelHost)
                DeployListEventReceiver(modelHost, modelHost as ListModelHost, definition);
            else if (modelHost is WebModelHost)
                DeployWebEventReceiver(modelHost, modelHost as WebModelHost, definition);
            else if (modelHost is SiteModelHost)
                DeploySiteEventReceiver(modelHost, modelHost as SiteModelHost, definition);
            else if (modelHost is ContentType)
            {
                //https://officespdev.uservoice.com/forums/224641-general/suggestions/6825755-add-contenttype-eventreceivers-proprety-similar-to

                throw new SPMeta2UnsupportedModelHostException("Adding event receiver to content types via CSOM is not supported by CSOM: https://officespdev.uservoice.com/forums/224641-general/suggestions/6825755-add-contenttype-eventreceivers-proprety-similar-to");
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException("model host should be ListModelHost or WebModelHost");
            }
        }

        private void DeploySiteEventReceiver(object modelHost, SiteModelHost siteModelHost, EventReceiverDefinition definition)
        {
            DeployEventReceiver(modelHost, siteModelHost.HostSite.EventReceivers, definition);
        }

        private void DeployListEventReceiver(object modelHost, ListModelHost listModelHost, EventReceiverDefinition definition)
        {
            DeployEventReceiver(modelHost, listModelHost.HostList.EventReceivers, definition);
        }

        private void DeployWebEventReceiver(object modelHost, WebModelHost listModelHost, EventReceiverDefinition definition)
        {
            DeployEventReceiver(modelHost, listModelHost.HostWeb.EventReceivers, definition);
        }

        protected Microsoft.SharePoint.Client.EventReceiverDefinition FindEventReceiverDefinition(EventReceiverDefinitionCollection receivers, EventReceiverDefinition definition)
        {
            var receiverName = definition.Name.ToUpper();

            receivers.Context.Load(receivers);
            receivers.Context.ExecuteQuery();

            return receivers.OfType<Microsoft.SharePoint.Client.EventReceiverDefinition>()
                            .FirstOrDefault(r =>
                                !string.IsNullOrEmpty(r.ReceiverName) &&
                                r.ReceiverName.ToUpper() == receiverName);
        }

        private void DeployEventReceiver(object modelHost, EventReceiverDefinitionCollection eventReceivers,
            EventReceiverDefinition definition)
        {
            var context = eventReceivers.Context;

            var existingReceiver = FindEventReceiverDefinition(eventReceivers, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingReceiver,
                ObjectType = typeof(Microsoft.SharePoint.Client.EventReceiverDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            bool isNew = false;

            if (existingReceiver == null)
            {
                isNew = true;

                existingReceiver = eventReceivers.Add(new EventReceiverDefinitionCreationInformation
                {
                    ReceiverName = definition.Name,
                    EventType = (EventReceiverType)Enum.Parse(typeof(EventReceiverType), definition.Type),
                    Synchronization = (EventReceiverSynchronization)Enum.Parse(typeof(EventReceiverSynchronization), definition.Synchronization),
                    ReceiverAssembly = definition.Assembly,
                    ReceiverClass = definition.Class,
                    SequenceNumber = definition.SequenceNumber,
                });
            }

            MapEventReceiverProperties(definition, existingReceiver, isNew);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingReceiver,
                ObjectType = typeof(Microsoft.SharePoint.Client.EventReceiverDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            existingReceiver.Update();
            context.ExecuteQuery();
        }

        private static void MapEventReceiverProperties(EventReceiverDefinition definition,
            Microsoft.SharePoint.Client.EventReceiverDefinition existingReceiver,
            bool isNew)
        {
            // nothing can be updated, shame :(
        }

        #endregion
    }
}
