using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class EventReceiverModelHandler : SSOMModelHandlerBase
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
            else if (modelHost is SPContentType)
                DeployContentTypeEventReceiver(modelHost, modelHost as SPContentType, definition);
            else
            {
                throw new SPMeta2UnsupportedModelHostException("model host should be ListModelHost/WebModelHost/SiteModelHost");
            }
        }

        private void DeployContentTypeEventReceiver(object modelHost, SPContentType contentType, EventReceiverDefinition definition)
        {
            DeployEventReceiver(modelHost, contentType.EventReceivers, definition);
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

        protected SPEventReceiverDefinition FindEventReceiverDefinition(SPEventReceiverDefinitionCollection receivers, EventReceiverDefinition definition)
        {
            var receiverName = definition.Name.ToUpper();

            return receivers.OfType<SPEventReceiverDefinition>()
                            .FirstOrDefault(r =>
                                !string.IsNullOrEmpty(r.Name) &&
                                r.Name.ToUpper() == receiverName);
        }

        protected SPEventReceiverDefinition CreateNewEventReceiverDefinition(
            object modelHost,
            SPEventReceiverDefinitionCollection eventReceivers,
            out bool isNew)
        {
            var result = eventReceivers.Add();

            isNew = true;

            if (modelHost is WebModelHost)
                result.HostType = SPEventHostType.Web;
            else if (modelHost is ListModelHost)
                result.HostType = SPEventHostType.List;
            else if (modelHost is SiteModelHost)
                result.HostType = SPEventHostType.Site;
            else if (modelHost is SPContentType)
                result.HostType = SPEventHostType.ContentType;
            else
            {
                throw new SPMeta2UnsupportedModelHostException("model host should be ListModelHost/WebModelHost/SiteModelHost");
            }

            return result;
        }

        private void DeployEventReceiver(object modelHost, SPEventReceiverDefinitionCollection eventReceivers,
            EventReceiverDefinition definition)
        {
            var existingReceiver = FindEventReceiverDefinition(eventReceivers, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingReceiver,
                ObjectType = typeof(SPEventReceiverDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            bool isNew = false;

            if (existingReceiver == null)
                existingReceiver = CreateNewEventReceiverDefinition(modelHost, eventReceivers, out isNew);

            MapEventReceiverProperties(definition, existingReceiver, isNew);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingReceiver,
                ObjectType = typeof(SPEventReceiverDefinition),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            existingReceiver.Update();
        }

        private static void MapEventReceiverProperties(
            EventReceiverDefinition definition,
            SPEventReceiverDefinition existingReceiver,
            bool isNew)
        {
            existingReceiver.Name = definition.Name;
            existingReceiver.Data = definition.Data;

            if (isNew)
                existingReceiver.Type = (SPEventReceiverType)Enum.Parse(typeof(SPEventReceiverType), definition.Type);

            existingReceiver.Assembly = definition.Assembly;
            existingReceiver.Class = definition.Class;
            existingReceiver.SequenceNumber = definition.SequenceNumber;
            existingReceiver.Synchronization = (SPEventReceiverSynchronization)Enum.Parse(typeof(SPEventReceiverSynchronization), definition.Synchronization);
        }

        #endregion
    }
}
