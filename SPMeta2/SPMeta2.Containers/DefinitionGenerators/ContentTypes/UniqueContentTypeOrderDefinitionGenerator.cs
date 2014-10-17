using System;
using System.Collections.Generic;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class UniqueContentTypeOrderDefinitionGenerator : TypedDefinitionGeneratorServiceBase<UniqueContentTypeOrderDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                //def.Title = Rnd.String();
                //def.Description = Rnd.String();

                //def.Url = Rnd.String(16);

                //def.TemplateType = BuiltInListTemplateTypeId.GenericList;

                //def.ContentTypesEnabled = Rnd.Bool();
            });
        }
    }
}
