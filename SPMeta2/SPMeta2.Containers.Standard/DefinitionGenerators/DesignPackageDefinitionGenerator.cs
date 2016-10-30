using System;
using System.Collections.Generic;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using System.IO;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class DesignPackageDefinitionGenerator : TypedDefinitionGeneratorServiceBase<DesignPackageDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = string.Format("{0}.wsp", Rnd.String());

                def.Install = true;
                def.Apply = false;

                def.SolutionId = DefaultContainers.DesignPackage.SolutionId;
                def.Content = File.ReadAllBytes(DefaultContainers.DesignPackage.Package_v10_FilePath);
            });
        }
    }
}
