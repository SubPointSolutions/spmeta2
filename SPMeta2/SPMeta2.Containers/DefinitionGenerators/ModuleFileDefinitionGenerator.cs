using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ModuleFileDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ModuleFileDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = Rnd.String() + ".txt";
                def.Content = Rnd.Content();
            });
        }

        public override ModelNode GetCustomParenHost()
        {
            var listDefinitionGenerator = new ListDefinitionGenerator();
            var listDefinition = listDefinitionGenerator.GenerateRandomDefinition() as ListDefinition;

            listDefinition.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;

            var node = new ListModelNode
            {
                Value = listDefinition,
                //Options = { RequireSelfProcessing = false }
            };

            return node;
        }
    }
}
