using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ListViewDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListViewDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.RowLimit = Rnd.Int(30);

                def.IsDefault = Rnd.Bool();
                def.IsPaged = Rnd.Bool();
            });
        }
    }
}
