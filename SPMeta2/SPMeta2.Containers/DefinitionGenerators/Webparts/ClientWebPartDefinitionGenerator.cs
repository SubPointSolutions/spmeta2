using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Containers.Consts;
using System.Collections.Generic;

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

                def.WebPartName = DefaultContainers.Apps.M2ClientWebPart1Name;

                def.ProductId = DefaultContainers.Apps.ProductId;
                def.FeatureId = DefaultContainers.Apps.FeatureId;
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            // return defaul app
            return new[] { new AppDefinitionGenerator().GenerateRandomDefinition() };
        }
    }
}
