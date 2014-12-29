using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class TargetApplicationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TargetApplicationDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.ApplicationId = Rnd.String(8);
                def.FriendlyName = Rnd.String(8);
                def.Name = Rnd.String(8);

                def.ContactEmail = Rnd.UserEmail();
                def.TicketTimeout = Rnd.Int(120);
                def.TargetApplicationClams.Add(Rnd.UserLogin());

                def.CredentialManagementUrl = Rnd.HttpUrl();

                def.Type = BuiltInTargetApplicationType.Individual;

                def.Fields = new Collection<TargetApplicationFieldValue>
                {
                    new TargetApplicationFieldValue
                    {
                        Name = Rnd.String(),
                        IsMasked = false,
                        CredentialType = BuiltInSecureStoreCredentialType.UserName
                    },
                    new TargetApplicationFieldValue
                    {
                        Name = Rnd.String(),
                        IsMasked = true,
                        CredentialType = BuiltInSecureStoreCredentialType.Password
                    }
                };
            });
        }
    }
}
