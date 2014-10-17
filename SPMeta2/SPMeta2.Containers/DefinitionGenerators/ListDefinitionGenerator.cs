using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ListDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.Description = Rnd.String();

                def.Url = Rnd.String(16);

                def.TemplateType = BuiltInListTemplateTypeId.GenericList;

                def.ContentTypesEnabled = Rnd.Bool();
            });
        }
    }
}
