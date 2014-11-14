using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class NoteFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(NoteFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new NoteFieldDefinition
            {
                NumberOfLines = Rnd.Int(20) + 1,
                RichText = Rnd.Bool(),
                UnlimitedLengthInDocumentLibrary = Rnd.Bool()
            };

            return def;
        }

    }
}
