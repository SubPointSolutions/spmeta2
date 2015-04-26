using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using Microsoft.SharePoint;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class EventReceiverDefinitionValidator : EventReceiverModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<EventReceiverDefinition>("model", value => value.RequireNotNull());
            SPEventReceiverDefinition spObject = null;

            if (modelHost is ListModelHost)
                spObject = FindEventReceiverDefinition((modelHost as ListModelHost).HostList.EventReceivers, definition);
            else if (modelHost is WebModelHost)
                spObject = FindEventReceiverDefinition((modelHost as WebModelHost).HostWeb.EventReceivers, definition);
            else if (modelHost is SiteModelHost)
                spObject = FindEventReceiverDefinition((modelHost as SiteModelHost).HostSite.EventReceivers, definition);
            else if (modelHost is SPContentType)
                spObject = FindEventReceiverDefinition((modelHost as SPContentType).EventReceivers, definition);
            else
            {
                throw new SPMeta2UnsupportedModelHostException("model host should be ListModelHost or WebModelHost");
            }

            var assert = ServiceFactory.AssertService
                               .NewAssert(definition, spObject)
                                     .ShouldNotBeNull(spObject)
                                     .ShouldBeEqual(m => m.Name, o => o.Name)
                                     .ShouldBeEqual(m => m.Class, o => o.Class)
                                     .ShouldBeEqual(m => m.Assembly, o => o.Assembly)
                                     //.ShouldBeEqual(m => m.Data, o => o.Data)
                                     .ShouldBeEqual(m => m.SequenceNumber, o => o.SequenceNumber)
                                     .ShouldBeEqual(m => m.Synchronization, o => o.GetSynchronization())
                                     .ShouldBeEqual(m => m.Type, o => o.GetEventReceiverType());

            if (!string.IsNullOrEmpty(definition.Data))
                assert.ShouldBeEqual(m => m.Data, o => o.Data);
            else
                assert.SkipProperty(m => m.Data);
        }
    }

    internal static class EventReceiverDefinitionExtension
    {
        public static string GetSynchronization(this SPEventReceiverDefinition def)
        {
            return def.Synchronization.ToString();
        }

        public static string GetEventReceiverType(this SPEventReceiverDefinition def)
        {
            return def.Type.ToString();
        }
    }
}
