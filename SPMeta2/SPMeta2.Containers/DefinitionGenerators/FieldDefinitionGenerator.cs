using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class FieldDefinitionGenerator : TypedDefinitionGeneratorServiceBase<FieldDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.InternalName = Rnd.String(32);

                def.Description = Rnd.String();
                def.FieldType = BuiltInFieldTypes.Text;

                def.Required = Rnd.Bool();

                def.Group = Rnd.String();
                def.Title = Rnd.String(32);


                def.JSLink = Rnd.String(32);
                def.DefaultValue = Rnd.String(32);
                def.Hidden = Rnd.Bool();

                def.ShowInDisplayForm = Rnd.Bool();
                def.ShowInEditForm = Rnd.Bool();
                def.ShowInListSettings = Rnd.Bool();
                def.ShowInNewForm = Rnd.Bool();
                def.ShowInVersionHistory = Rnd.Bool();
                def.ShowInViewForms = Rnd.Bool();

                def.AllowDeletion = Rnd.Bool();
                def.Indexed = Rnd.Bool();
            });
        }
    }
}
