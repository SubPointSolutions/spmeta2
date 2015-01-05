using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class TreeViewSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TreeViewSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                if (Rnd.Bool())
                    def.QuickLaunchEnabled = null;
                else
                    def.QuickLaunchEnabled = Rnd.Bool();


                if (Rnd.Bool())
                    def.TreeViewEnabled = null;
                else
                    def.TreeViewEnabled = Rnd.Bool();
            });
        }
    }
}
