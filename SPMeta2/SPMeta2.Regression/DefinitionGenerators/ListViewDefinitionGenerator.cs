using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
{
    public class ListViewDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListViewDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.RowLimit = Rnd.Int(30);

                def.IsDefault = Rnd.Bool();
                def.IsPaged = Rnd.Bool();
            });
        }
    }
}
