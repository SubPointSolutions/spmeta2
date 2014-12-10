using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Utils;
using EventReceiverDefinition = SPMeta2.Definitions.EventReceiverDefinition;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientEventReceiverDefinitionValidator : EventReceiverModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<EventReceiverDefinition>("model", value => value.RequireNotNull());
            Microsoft.SharePoint.Client.EventReceiverDefinition spObject = null;

            if (modelHost is ListModelHost)
                spObject = FindEventReceiverDefinition((modelHost as ListModelHost).HostList.EventReceivers, definition);
            else if (modelHost is WebModelHost)
                spObject = FindEventReceiverDefinition((modelHost as WebModelHost).HostWeb.EventReceivers, definition);
            else if (modelHost is SiteModelHost)
                spObject = FindEventReceiverDefinition((modelHost as SiteModelHost).HostSite.EventReceivers, definition);
            else
            {
                throw new SPMeta2UnsupportedModelHostException("model host should be ListModelHost or WebModelHost");
            }

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldNotBeNull(spObject)
                                     .ShouldBeEqual(m => m.Name, o => o.ReceiverName)
                                     .ShouldBeEqual(m => m.Class, o => o.ReceiverClass)
                                     .ShouldBeEqual(m => m.Assembly, o => o.ReceiverAssembly)
                                     .SkipProperty(m => m.Data, "Data property is not supported by CSOM. SKipping.")
                                     .ShouldBeEqual(m => m.SequenceNumber, o => o.SequenceNumber)
                                     .ShouldBeEqual(m => m.Synchronization, o => o.GetSynchronization())
                                     .ShouldBeEqual(m => m.Type, o => o.GetEventReceiverType());
        }
    }


    internal static class EventReceiverDefinitionExtension
    {
        public static string GetSynchronization(this Microsoft.SharePoint.Client.EventReceiverDefinition def)
        {
            return def.Synchronization.ToString();
        }

        public static string GetEventReceiverType(this Microsoft.SharePoint.Client.EventReceiverDefinition def)
        {
            return def.EventType.ToString();
        }
    }
}
