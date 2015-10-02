using System;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WebPartPageDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebPartPageDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = Rnd.String() + ".aspx";
                def.NeedOverride = true;
                def.PageLayoutTemplate = BuiltInWebpartPageTemplateId.spstd1;
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
            };

            return node;
        }
    }
}
