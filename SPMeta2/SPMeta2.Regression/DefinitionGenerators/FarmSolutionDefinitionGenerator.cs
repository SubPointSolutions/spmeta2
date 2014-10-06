using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Regression.Services.Base;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.DefinitionGenerators
{
    public class FarmSolutionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<FarmSolutionDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = Rnd.String();

                def.Content = ModuleFileUtils.FromResource(
                        System.AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("SPMeta2.SSOM")),
                        "SPMeta2.SSOM.SandboxApps.SPMeta2.Sandbox.TestApp.wsp");


            });
        }
    }
}
