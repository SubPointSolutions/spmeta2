using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class EventReceiverDefinitionGenerator : TypedDefinitionGeneratorServiceBase<EventReceiverDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();

                def.Data = Rnd.String();
                def.SequenceNumber = Rnd.Int(10000 + 1000);

                if (Rnd.Bool())
                {
                    def.Synchronization = BuiltInEventReceiverSynchronization.Synchronous;
                    def.Type = BuiltInEventReceiverType.ItemAdded;
                }
                else
                {
                    def.Synchronization = BuiltInEventReceiverSynchronization.Asynchronous;
                    def.Type = BuiltInEventReceiverType.ItemAdded;
                }

                // OOTB event receiver for the test purposes
                def.Assembly = "Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";
                def.Class = "Microsoft.SharePoint.Help.HelpLibraryEventReceiver";
            });
        }
    }
}
