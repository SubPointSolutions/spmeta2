using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class MasterPageSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<MasterPageSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.SiteMasterPageUrl = "/_catalogs/masterpage/seattle.master";
                def.SystemMasterPageUrl = "/_catalogs/masterpage/seattle.master";
            });
        }
    }
}
