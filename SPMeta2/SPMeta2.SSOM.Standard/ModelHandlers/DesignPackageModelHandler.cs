using System;
using System.Linq;
using Microsoft.Office.DocumentManagement;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.Office.Server.UserProfiles;
using SPMeta2.Services;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class DesignPackageModelHandler : SandboxSolutionModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DesignPackageDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DesignPackageDefinition>("model", value => value.RequireNotNull());

            DeployDesignPackage(modelHost, siteModelHost, definition);
        }

        private void DeployDesignPackage(object modelHost, SiteModelHost siteModelHost, DesignPackageDefinition definition)
        {
            var site = siteModelHost.HostSite;
            var sandboxSolution = FindExistingSolutionById(siteModelHost, definition.SolutionId);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = sandboxSolution,
                ObjectType = typeof(SPUserSolution),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (sandboxSolution != null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                    "Processing existing design package");
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                    "Processing new design package");
            }

            var designPackageInfo = new DesignPackageInfo(definition.FileName,
                       definition.SolutionId,
                       definition.MajorVersion,
                       definition.MinorVersion);

            if (definition.Install)
            {
                // removing first
                if (sandboxSolution != null)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deleting existing sandbox solution");
                    siteModelHost.HostSite.Solutions.Remove(sandboxSolution);

                    var catalog = siteModelHost.HostSite.RootWeb.GetCatalog(SPListTemplateType.SolutionCatalog);

                    try
                    {
                        var solutionFile = catalog.RootFolder.Files[sandboxSolution.Name];

                        if (solutionFile.Exists)
                            solutionFile.Delete();
                    }
                    catch
                    {
                        TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                        "Error while deleting design package");
                    }
                }

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying design package file to the root folder of the root web");

                var rootFolder = site.RootWeb.RootFolder;
                var tmpDesignPackageFile = rootFolder.Files.Add(definition.FileName, definition.Content, true);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                    string.Format("Installing design package from URL:[{0}]", tmpDesignPackageFile.ServerRelativeUrl));

                DesignPackage.Install(site, designPackageInfo, tmpDesignPackageFile.ServerRelativeUrl);
                sandboxSolution = FindExistingSolutionById(siteModelHost, definition.SolutionId);

                if (definition.Apply)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Applying design package");
                    DesignPackage.Apply(site, designPackageInfo);
                }
                else
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Apply == false. Skipping design package activation");
                }

                // cleanup
                if (tmpDesignPackageFile.Exists)
                {
                    tmpDesignPackageFile.Delete();
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = sandboxSolution,
                    ObjectType = typeof(SPUserSolution),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });
            }
            else
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Activate = false. Continue provision");

                if (sandboxSolution != null)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Solution is NOT NULL. Checking Apply status");

                    if (definition.Apply)
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Applying design package");
                        DesignPackage.Apply(site, designPackageInfo);
                    }
                    else
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                            "Apply == false. Skipping design package activation");
                    }
                }
                else
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Solution is NULL. Skipping Apply status");
                }

                sandboxSolution = FindExistingSolutionById(siteModelHost, definition.SolutionId);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = sandboxSolution,
                    ObjectType = typeof(SPUserSolution),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });
            }
        }

        #endregion
    }
}
