using SPMeta2.Definitions;
using SPMeta2.Syntax.Default.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegModuleFiles
    {
        public static ModuleFileDefinition HelloSharePoint = new ModuleFileDefinition
        {
            FileName = "HelloSharePoint.txt",
            Content = ModuleFileUtils.FromResource(typeof(RegModuleFiles).Assembly, "SPMeta2.Regression.Model.Modules.HelloSharePoint.txt")
        };
    }
}
