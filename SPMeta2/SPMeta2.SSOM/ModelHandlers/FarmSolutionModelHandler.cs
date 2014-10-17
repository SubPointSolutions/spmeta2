using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FarmSolutionModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(FarmSolutionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var solutionModel = model.WithAssertAndCast<FarmSolutionDefinition>("model", value => value.RequireNotNull());

            DeploySolution(farmModelHost, solutionModel);
        }

        private void DeploySolution(FarmModelHost modelHost, FarmSolutionDefinition definition)
        {
            var farm = modelHost.HostFarm;

            var existringSolution = farm.Solutions.FirstOrDefault(s => s.Name.ToLower() == definition.FileName.ToLower());

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existringSolution,
                ObjectType = typeof(SPSolution),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existringSolution == null)
            {
                var tmpWspDirectory = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(definition.FileName), Guid.NewGuid().ToString("N"));
                var tmpWspDirectoryPath = Path.Combine(Path.GetTempPath(), tmpWspDirectory);

                Directory.CreateDirectory(tmpWspDirectoryPath);

                var tmpWspFilPath = Path.Combine(tmpWspDirectoryPath, definition.FileName);
                File.WriteAllBytes(tmpWspFilPath, definition.Content);

                existringSolution = farm.Solutions.Add(tmpWspFilPath, (uint)definition.LCID);
            }
            else
            {

            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existringSolution,
                ObjectType = typeof(SPSolution),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
