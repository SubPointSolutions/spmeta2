using SPMeta2.Definitions;
using SPMeta2.Diagnostic;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SPMeta2.Extensions;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelStatInfoService : ModelStatInfoServiceBase
    {
        public override ModelStatInfo GetModelStat(ModelNode modelNode)
        {
            return GetModelStat(modelNode, new ModelStatInfoServiceOptions());
        }

        public override ModelStatInfo GetModelStat(ModelNode modelNode, ModelStatInfoServiceOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            var result = new ModelStatInfo();

            var allModelNodes = modelNode.Flatten();

            result.TotalModelNodeCount = allModelNodes.Count;

            foreach (var node in allModelNodes)
            {
                var def = node.Value;

                var defAssemblyTypeName = def.GetType().AssemblyQualifiedName;
                var defAssemblyTypeShortName = def.GetType().Name;

                var nodeStat = result.NodeStat.FirstOrDefault(s => s.DefinitionAssemblyQualifiedName == defAssemblyTypeName);

                if (nodeStat == null)
                {
                    nodeStat = new ModelNodeStatInfo
                    {
                        DefinitionAssemblyQualifiedName = defAssemblyTypeName,
                        DefinitionName = defAssemblyTypeShortName
                    };

                    result.NodeStat.Add(nodeStat);
                }

                nodeStat.Count++;

                if (options.IncludeModelNodes)
                {
                    nodeStat.ModelNodes.Add(modelNode);
                }
            }

            return result;
        }
    }
}
