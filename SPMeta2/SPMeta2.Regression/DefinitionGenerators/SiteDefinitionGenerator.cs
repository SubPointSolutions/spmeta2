using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
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
