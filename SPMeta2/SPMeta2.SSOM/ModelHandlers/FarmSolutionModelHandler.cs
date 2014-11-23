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
using SPMeta2.Services;
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

        protected SPSolution FindExistingSolution(FarmModelHost modelHost, FarmSolutionDefinition definition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                "Resolving farm solution by SolutionId: [{0}] and Name: [{1}]",
                 new object[]
                    {
                        definition.SolutionId,
                        definition.FileName
                    });

            var farm = modelHost.HostFarm;

            return farm.Solutions.FirstOrDefault(s =>
                s.Name.ToUpper() == definition.FileName.ToUpper() ||
                definition.SolutionId != Guid.Empty && s.SolutionId == definition.SolutionId);
        }

        private void DeploySolution(FarmModelHost modelHost, FarmSolutionDefinition definition)
        {
            var farm = modelHost.HostFarm;
            var existringSolution = FindExistingSolution(modelHost, definition);

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

            var tmpWspDirectory = string.Format("{0}_{1}", Path.GetFileNameWithoutExtension(definition.FileName), Guid.NewGuid().ToString("N"));
            var tmpWspDirectoryPath = Path.Combine(Path.GetTempPath(), tmpWspDirectory);

            Directory.CreateDirectory(tmpWspDirectoryPath);

            var tmpWspFilPath = Path.Combine(tmpWspDirectoryPath, definition.FileName);
            File.WriteAllBytes(tmpWspFilPath, definition.Content);

            if (existringSolution == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new farm solution");
                existringSolution = farm.Solutions.Add(tmpWspFilPath, (uint)definition.LCID);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Upgrading existing farm solution");
                existringSolution.Upgrade(tmpWspFilPath);
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
