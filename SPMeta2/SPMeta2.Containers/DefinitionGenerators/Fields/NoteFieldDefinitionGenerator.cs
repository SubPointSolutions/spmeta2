using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class NoteFieldDefinitionGenerator : TypedDefinitionGeneratorServiceBase<NoteFieldDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.InternalName = Rnd.String(32);

                def.Description = Rnd.String();
                //def.FieldType = BuiltInFieldTypes.Text;

                def.Required = Rnd.Bool();

                def.Group = Rnd.String();
                def.Title = Rnd.String(32);

                def.NumberOfLines = Rnd.Int(20) + 1;
                def.RichText = Rnd.Bool();
                def.UnlimitedLengthInDocumentLibrary = Rnd.Bool();
            });
        }
    }
}
