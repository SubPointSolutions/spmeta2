using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ContentTypeDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ContentTypeDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.Name = Rnd.String(32);

                def.Description = Rnd.String();
                def.Group = Rnd.String();

                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });
        }
    }
}
