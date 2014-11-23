using SPMeta2.Common;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SandboxSolutionModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SandboxSolutionDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var sandboxSolutionDefinition = model.WithAssertAndCast<SandboxSolutionDefinition>("model", value => value.RequireNotNull());

            DeploySandboxSolution(modelHost, siteModelHost, sandboxSolutionDefinition);
        }

        private void DeploySandboxSolution(object modelHost, SiteModelHost siteModelHost, SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            var existingSolutions = FindExistingSolutionFile(siteModelHost, sandboxSolutionDefinition);
            var sandboxSolution = FindExistingSolution(siteModelHost, sandboxSolutionDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = sandboxSolution,
                ObjectType = typeof(SPUserSolution),
                ObjectDefinition = sandboxSolutionDefinition,
                ModelHost = modelHost
            });

            if (sandboxSolution != null)
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing sandbox solution");

            if (sandboxSolution != null && sandboxSolution.Status == SPUserSolutionStatus.Activated)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deleting existing sandbox solution");
                siteModelHost.HostSite.Solutions.Remove(sandboxSolution);
            }

            var solutionGallery = (SPDocumentLibrary)siteModelHost.HostSite.GetCatalog(SPListTemplateType.SolutionCatalog);
            var file = solutionGallery.RootFolder.Files.Add(sandboxSolutionDefinition.FileName, sandboxSolutionDefinition.Content, true);

            if (sandboxSolutionDefinition.Activate)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Activating sandbox solution");

                var userSolution = siteModelHost.HostSite.Solutions.Add(file.Item.ID);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = userSolution,
                    ObjectType = typeof(SPUserSolution),
                    ObjectDefinition = sandboxSolutionDefinition,
                    ModelHost = modelHost
                });
            }
            else
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Activate = false. Continue provision");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = sandboxSolution,
                    ObjectType = typeof(SPUserSolution),
                    ObjectDefinition = sandboxSolutionDefinition,
                    ModelHost = modelHost
                });
            }
        }

        protected SPUserSolution FindExistingSolution(SiteModelHost siteModelHost,
            SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving sandbox solution by SolutionId: [{0}]", sandboxSolutionDefinition.SolutionId);

            return siteModelHost.HostSite.Solutions.OfType<SPUserSolution>()
                                                                      .FirstOrDefault(f => f.SolutionId == sandboxSolutionDefinition.SolutionId);
        }

        protected SPFile FindExistingSolutionFile(SiteModelHost siteModelHost, SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving sandbox solution by FileName: [{0}]", sandboxSolutionDefinition.FileName);


            var site = siteModelHost.HostSite;
            var solutionGallery = (SPDocumentLibrary)site.GetCatalog(SPListTemplateType.SolutionCatalog);

            return solutionGallery.RootFolder
                .Files
                .OfType<SPFile>()
                .FirstOrDefault(f => f.Name.ToUpper() == sandboxSolutionDefinition.FileName.ToUpper());
        }

        #endregion
    }
}
