using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client.DocumentSet;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using File = Microsoft.SharePoint.Client.File;

namespace SPMeta2.CSOM.Standard.ModelHandlers
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

            DeployArtifact(siteModelHost, definition);
        }

        private void DeployArtifact(SiteModelHost modelHost, DesignPackageDefinition definition)
        {
            var site = modelHost.HostSite;
            var context = site.Context;

            //var existingSolutionFile = FindExistingSolutionFile(siteModelHost, definition);
            var existingSolution = FindExistingSolutionById(modelHost, definition.SolutionId);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingSolution,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            // uninstalling existing one
            if (existingSolution != null && definition.Install)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                    "Processing existing sandbox solution");

                var fileItem = existingSolution.ListItemAllFields;
                context.Load(fileItem);
                context.ExecuteQueryWithTrace();

                var currentFileName = fileItem["FileLeafRef"].ToString();

                //if (!currentFileName.Contains(definition.FileName))
                //    currentFileName = currentFileName.Replace(string.Format("-v{0}.{1}.wsp", 1, 0), string.Empty);
                //else
                //    currentFileName = definition.FileName;

                var packageFileName = Path.GetFileNameWithoutExtension(currentFileName);
                var packageFileNameParts = packageFileName.Split('-');

                var packageName = packageFileNameParts[0];

                var versionParts = packageFileNameParts[1].Replace("v", string.Empty)
                    .Split('.');

                var packageMajorVersion = int.Parse(versionParts[0]);
                var packageMinorVersion = int.Parse(versionParts[1]);

                var info = new DesignPackageInfo
                {
                    PackageGuid = definition.SolutionId,
                    MajorVersion = packageMajorVersion,
                    MinorVersion = packageMinorVersion,
                    PackageName = packageName,

                };

                // cleaning up AppliedDesignGuid at root web to enable 'force' uninstall
                var rootWeb = site.RootWeb;
                rootWeb.AllProperties["AppliedDesignGuid"] = null;
                rootWeb.Update();

                context.ExecuteQueryWithTrace();

                // deactivate and remove
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                    "Deleting existing sandbox solution via DesignPackage.UnInstall()");
                DesignPackage.UnInstall(context, site, info);

                try
                {
                    context.ExecuteQueryWithTrace();
                }
                catch (Exception uninstallException)
                {
                    // pass this one
                    // bug in SharePoint's DesignPackage.UnInstall method
                    if (!uninstallException.ToString().Contains("33e33eca-7712-4f3d-ab83-6848789fc9b6"))
                    {
                        throw;
                    }
                }
            }

            // installing new
            if (definition.Install)
            {
                var folder = site.RootWeb.RootFolder;

                context.Load(folder);
                context.ExecuteQueryWithTrace();

                var fileCreatingInfo = new FileCreationInformation
                {
                    Url = definition.FileName,
                    Overwrite = true
                };

                if (definition.Content.Length < 1024 * 1024 * 1.5)
                {
                    fileCreatingInfo.Content = definition.Content;
                }
                else
                {
                    fileCreatingInfo.ContentStream = new MemoryStream(definition.Content);
                }


                var newFile = folder.Files.Add(fileCreatingInfo);

                context.Load(newFile);
                context.Load(newFile, f => f.ServerRelativeUrl);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Uploading new sandbox solution to root site folder");
                context.ExecuteQueryWithTrace();

                var info = new DesignPackageInfo
                {
                    PackageGuid = definition.SolutionId,
                    MajorVersion = definition.MajorVersion,
                    MinorVersion = definition.MinorVersion,
                    PackageName = Path.GetFileNameWithoutExtension(definition.FileName)
                };

                // activate and remove
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "DesignPackage.Install()");
                DesignPackage.Install(context, site, info, newFile.ServerRelativeUrl);
                context.ExecuteQueryWithTrace();

                // clean up the file
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deleting sandbox file from root site folder");
                newFile.DeleteObject();
                context.ExecuteQueryWithTrace();

                existingSolution = FindExistingSolutionById(modelHost, definition.SolutionId);
            }

            if (definition.Apply)
            {
                var info = new DesignPackageInfo
                {
                    PackageGuid = definition.SolutionId,
                    MajorVersion = definition.MajorVersion,
                    MinorVersion = definition.MinorVersion,
                    PackageName = Path.GetFileNameWithoutExtension(definition.FileName)
                };

                // apply
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "DesignPackage.Apply()");
                DesignPackage.Apply(context, site, info);
                context.ExecuteQueryWithTrace();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingSolution,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
