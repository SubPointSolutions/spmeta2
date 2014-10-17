using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class FeatureDefinitionGenerator : TypedDefinitionGeneratorServiceBase<FeatureDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Enable = Rnd.Bool();
                def.ForceActivate = Rnd.Bool();

                def.Scope = FeatureDefinitionScope.Web; 
                def.Id = BuiltInWebFeatures.TeamCollaborationLists.Id;
            });
        }
    }
}
