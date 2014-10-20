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
                def.Activate = Rnd.Bool();

                def.SolutionId = new Guid("e34d1ce3-62da-4a73-a382-a49af8e5dde0");
                def.Content = ResourceReaderUtils.ReadFromResourceName(typeof(AppDefinitionGenerator).Assembly, "SPMeta2.Containers.Templates.Apps.SPMeta2.Sandbox.TestSandboxApp.wsp");
            });
        }
    }
}
