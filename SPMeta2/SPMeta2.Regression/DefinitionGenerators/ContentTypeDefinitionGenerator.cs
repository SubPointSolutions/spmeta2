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
    public class ContentTypeDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ContentTypeDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.Name = Rnd.String(32);

                def.Description = Rnd.String();
                def.Group = Rnd.String();

                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });
        }
    }
}
