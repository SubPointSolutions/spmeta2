using System;
using System.Collections.ObjectModel;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ListViewDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListViewDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.RowLimit = Rnd.Int(30);

                if (Rnd.Bool())
                    def.Url = Rnd.String();

                def.IsDefault = Rnd.Bool();
                def.IsPaged = Rnd.Bool();

                def.Query = string.Format("<Where><Eq><FieldRef Name=\"{0}\" /><Value Type=\"Text\">{1}</Value></Eq></Where>", BuiltInInternalFieldNames.Title, Rnd.String());

                def.Fields = new Collection<string>
                {
                    BuiltInInternalFieldNames.ID,
                    BuiltInInternalFieldNames.FileLeafRef
                };

                def.JSLink = string.Format("~sitecollection/style library/{0}.js", Rnd.String());
            });
        }
    }
}
