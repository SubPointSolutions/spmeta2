using System;
using System.Linq;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Containers.Consts;
using System.IO;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class FarmSolutionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<FarmSolutionDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = string.Format("{0}.wsp", Rnd.String());

                def.SolutionId = DefaultContainers.FarmSolution.SolutionId;
                def.Content = File.ReadAllBytes(DefaultContainers.FarmSolution.FilePath);
            });
        }
    }
}
