using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ContentTypeFieldLinkDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ContentTypeFieldLinkDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FieldId = BuiltInFieldId.CellPhone;

                if (Rnd.Bool())
                    def.DisplayName = Rnd.String();

                if (Rnd.Bool())
                    def.Hidden = Rnd.Bool();

                if (Rnd.Bool())
                    def.Required = Rnd.Bool();
            });
        }
    }
}
