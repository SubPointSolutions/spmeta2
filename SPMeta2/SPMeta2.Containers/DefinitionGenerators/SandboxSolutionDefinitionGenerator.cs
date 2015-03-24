using System;
using System.Linq;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Syntax.Default.Utils;

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

                def.SolutionId = new Guid("e9a61998-07f2-45e9-ae43-9e93fa6b11bb");
                def.Content = ModuleFileUtils.FromResource(GetType().Assembly, "SPMeta2.Containers.Templates.Apps.SPMeta2.Containers.SandboxSolutionContainer.wsp");
            });
        }
    }
}
