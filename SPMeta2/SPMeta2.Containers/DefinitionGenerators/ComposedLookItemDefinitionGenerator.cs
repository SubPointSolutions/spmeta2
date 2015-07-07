using System;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ComposedLookItemDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ComposedLookItemDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.Name = Rnd.String();
            });
        }

        public override ModelNode GetCustomParenHost()
        {
            var definition = BuiltInListDefinitions.Catalogs.Design.Inherit<ListDefinition>(def =>
            {

            });

            var node = new ListModelNode
            {
                Value = definition,
                Options = { RequireSelfProcessing = false }
            };

            return node;
        }
    }
}
