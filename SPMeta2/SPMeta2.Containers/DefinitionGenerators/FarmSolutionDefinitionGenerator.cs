using System;
using System.Linq;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class FarmSolutionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<FarmSolutionDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = string.Format("{0}.wsp", Rnd.String());
                def.SolutionId = new Guid("9591a597-da94-47b4-a3c6-2f8703d4de2b");

                def.Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Containers.Templates.Apps.SPMeta2.Sandbox.TestFarmApp.wsp");
            });
        }
    }
}
