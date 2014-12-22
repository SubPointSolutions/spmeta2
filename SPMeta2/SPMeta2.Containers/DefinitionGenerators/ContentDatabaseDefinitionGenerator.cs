using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ContentDatabaseDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ContentDatabaseDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.ServerName = Rnd.DbServerName();
                def.DbName = Rnd.String(16);

                def.WarningSiteCollectionNumber = Rnd.Int(1000) + 10;
                def.MaximumSiteCollectionNumber = def.WarningSiteCollectionNumber + 10;
            });
        }
    }
}
