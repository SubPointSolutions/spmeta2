using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WikiPageDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WikiPageDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = Rnd.String() + ".aspx";
                def.NeedOverride = true;

                def.Content = Rnd.String();
            });
        }

        public override DefinitionBase GetCustomParenHost()
        {
            var listDefinitionGenerator = new ListDefinitionGenerator();
            var listDefinition = listDefinitionGenerator.GenerateRandomDefinition() as ListDefinition;

            listDefinition.TemplateType = BuiltInListTemplateTypeId.WebPageLibrary;

            return listDefinition;
        }
    }
}
