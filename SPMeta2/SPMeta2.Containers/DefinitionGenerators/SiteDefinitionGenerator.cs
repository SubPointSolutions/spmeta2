using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SiteDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SiteDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Url = Rnd.String();

                def.Name = Rnd.String();
                def.Description = Rnd.String();

                def.OwnerLogin = Rnd.UserName();
                def.OwnerName = Rnd.UserName();
                def.OwnerEmail = Rnd.UserEmail();

                def.SecondaryContactLogin = Rnd.UserName();
                def.SecondaryContactName = Rnd.UserName();
                def.SecondaryContactEmail = Rnd.UserEmail();

                def.SiteTemplate = BuiltInWebTemplates.Collaboration.TeamSite;

                def.PrefixName = Rnd.ManagedPath();
            });
        }
    }
}
