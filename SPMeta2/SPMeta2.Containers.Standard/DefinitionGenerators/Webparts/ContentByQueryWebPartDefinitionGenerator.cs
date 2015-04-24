using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Webparts;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Webparts
{
    public class ContentBySearchWebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ContentBySearchWebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "FullPage";
                def.ZoneIndex = Rnd.Int(100);

                // TODO


            });
        }
    }
}
