using System;
using System.Linq;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default.Utils;
using System.IO;
using SPMeta2.Containers.Consts;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SandboxSolutionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SandboxSolutionDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = string.Format("{0}.wsp", Rnd.String());
                def.Activate = true;

                def.SolutionId = DefaultContainers.Sandbox.SolutionId;
                def.Content = File.ReadAllBytes(DefaultContainers.Sandbox.FilePath);
            });
        }
    }
}
