using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;

namespace SPMeta2.Containers.DefinitionGenerators.Webparts
{
    public class ClientWebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ClientWebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "FullPage";
                def.ZoneIndex = Rnd.Int(100);

                // hardcoded yet
                // app needs to be deployed along with test process

                def.WebPartName = "NewTeamSiteForm";
                def.ProductId = new Guid("6d97b0a9-84a5-4bf6-a831-99c5f83f9686");
                def.FeatureId = new Guid("6d97b0a9-84a5-4bf6-a831-99c5f83f9687");
                //def.ProductWebId = new Guid("fae61eea-5448-4428-94b9-baf82ac3e818");
            });
        }
    }
}
