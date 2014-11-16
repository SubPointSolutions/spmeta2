using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class UserFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(UserFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new UserFieldDefinition();

            if (Rnd.Bool())
                def.SelectionMode = BuiltInFieldUserSelectionMode.PeopleAndGroups;
            else
                def.SelectionMode = BuiltInFieldUserSelectionMode.PeopleOnly;

            def.Presence = Rnd.Bool();
            def.AllowDisplay = Rnd.Bool();

            return def;
        }
    }
}
