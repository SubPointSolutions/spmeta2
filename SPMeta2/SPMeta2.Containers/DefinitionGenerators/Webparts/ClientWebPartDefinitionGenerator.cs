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
               
                def.ProductId = new Guid("1633e035-551d-48ab-96ed-3e0f3aa1bd3b");
                def.FeatureId = new Guid("1633e035-551d-48ab-96ed-3e0f3aa1bd3c");
                //def.ProductWebId = new Guid("55649e29-8e7e-47db-85ce-b6ed42f3327d");
            });
        }
    }
}
