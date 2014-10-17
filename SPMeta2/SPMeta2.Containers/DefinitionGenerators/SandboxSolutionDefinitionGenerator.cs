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
                def.FileName = Rnd.String();
                def.Activate = Rnd.Bool();

                def.Content = ModuleFileUtils.FromResource(
                        System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("SPMeta2.SSOM")),
                        "SPMeta2.SSOM.SandboxApps.SPMeta2.Sandbox.TestApp.wsp");


            });
        }
    }
}
