﻿using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;
using File = Microsoft.SharePoint.Client.File;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class SandboxSolutionModelHandler : CSOMModelHandlerBase
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
            var sandboxSolutionDefinition = model.WithAssertAndCast<SandboxSolutionDefinition>("model", value => value.RequireNotNull());

            ValidateDefinition(sandboxSolutionDefinition);

            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            DeploySandboxSolution(modelHost, siteModelHost, sandboxSolutionDefinition);
        }

        private void ValidateDefinition(SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            if (!sandboxSolutionDefinition.Activate)
                throw new SPMeta2NotSupportedException("SandboxSolutionDefinition.Activate must be true. (DesignPackage API requires it).");

            if (sandboxSolutionDefinition.SolutionId == default(Guid))
                throw new SPMeta2NotSupportedException("SandboxSolutionDefinition.SolutionId must be defined for CSOM based provision (DesignPackage API requires it).");

            var fileName = Path.GetFileNameWithoutExtension(sandboxSolutionDefinition.FileName);

            if (fileName.Contains("."))
                throw new SPMeta2NotSupportedException("SandboxSolutionDefinition.FileName must not contain dots. (DesignPackage API requires it).");
        }

        private void DeploySandboxSolution(object modelHost, SiteModelHost siteModelHost, SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            var site = siteModelHost.HostSite;
            var context = site.Context;

            var existingSolutionFile = FindExistingSolutionFile(siteModelHost, sandboxSolutionDefinition);
            var existingSolution = FindExistingSolution(siteModelHost, sandboxSolutionDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingSolutionFile,
                ObjectType = typeof(File),
                ObjectDefinition = sandboxSolutionDefinition,
                ModelHost = modelHost
            });

            if (existingSolution != null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing sandbox solution");

                var fileItem = existingSolution.ListItemAllFields;
                context.Load(fileItem);
                context.ExecuteQueryWithTrace();

                var currentFileName = fileItem["FileLeafRef"].ToString();

                if (!currentFileName.Contains(sandboxSolutionDefinition.FileName))
                {
                    currentFileName = currentFileName.Replace(string.Format("-v{0}.{1}.wsp", 1, 0), string.Empty);
                }
                else
                {
                    currentFileName = sandboxSolutionDefinition.FileName;
                }

                var info = new DesignPackageInfo
                {
                    PackageGuid = sandboxSolutionDefinition.SolutionId,
                    MajorVersion = 1,
                    MinorVersion = 0,
                    PackageName = Path.GetFileNameWithoutExtension(currentFileName)
                };

                // deactivate and remove
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deleting existing sandbox solution via DesignPackage.UnInstall()");
                DesignPackage.UnInstall(context, site, info);

                context.ExecuteQueryWithTrace();
            }

            //var solutionGallery = site.RootWeb.Lists.GetByTitle("Site Assets");
            //var folder = solutionGallery.RootFolder;
            var folder = site.RootWeb.RootFolder;

            context.Load(folder);
            context.ExecuteQueryWithTrace();

            var fileCreatingInfo = new FileCreationInformation
            {
                Url = sandboxSolutionDefinition.FileName,
                Overwrite = true
            };

            if (sandboxSolutionDefinition.Content.Length < 1024 * 1024 * 1.5)
            {
                fileCreatingInfo.Content = sandboxSolutionDefinition.Content;
            }
            else
            {
                fileCreatingInfo.ContentStream = new System.IO.MemoryStream(sandboxSolutionDefinition.Content);
            }


            var newFile = folder.Files.Add(fileCreatingInfo);

            context.Load(newFile);
            context.Load(newFile, f => f.ServerRelativeUrl);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Uploading new sandbox solution to root site folder");
            context.ExecuteQueryWithTrace();

            if (sandboxSolutionDefinition.Activate)
            {
                var info = new DesignPackageInfo
                {
                    PackageGuid = sandboxSolutionDefinition.SolutionId,
                    MajorVersion = 1,
                    MinorVersion = 0,
                    PackageName = Path.GetFileNameWithoutExtension(sandboxSolutionDefinition.FileName)
                };

                // activate and remove
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Activating sandbox solution via DesignPackage.Install()");
                DesignPackage.Install(context, site, info, newFile.ServerRelativeUrl);
                context.ExecuteQueryWithTrace();

                // clean up the file
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deleting sandbox file from root site folder");
                newFile.DeleteObject();
                context.ExecuteQueryWithTrace();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = newFile,
                ObjectType = typeof(File),
                ObjectDefinition = sandboxSolutionDefinition,
                ModelHost = modelHost
            });
        }

        protected File FindExistingSolution(SiteModelHost siteModelHost,
            SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            return FindExistingSolutionById(siteModelHost, sandboxSolutionDefinition.SolutionId);
        }

        protected File FindExistingSolutionById(SiteModelHost siteModelHost, Guid solutionId)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving sandbox solution by SolutionId: [{0}]", solutionId);

            var site = siteModelHost.HostSite;
            var context = site.Context;

            var solutionGallery = site.GetCatalog((int)ListTemplateType.SolutionCatalog);
            var files = solutionGallery.GetItems(CamlQuery.CreateAllItemsQuery());

            context.Load(files);
            context.ExecuteQueryWithTrace();

            var fileItem = files.ToList().FirstOrDefault(f => new Guid(f["SolutionId"].ToString()) == solutionId);
            return fileItem != null ? fileItem.File : null;
        }


        protected File FindExistingSolutionFile(SiteModelHost siteModelHost,
            SandboxSolutionDefinition sandboxSolutionDefinition)
        {
            return FindExistingSolutionFileByName(siteModelHost, sandboxSolutionDefinition.FileName);
        }

        protected File FindExistingSolutionFileByName(SiteModelHost siteModelHost, string fileName)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving sandbox solution by FileName: [{0}]", fileName);

            var site = siteModelHost.HostSite;
            var context = site.Context;

            var solutionGallery = site.GetCatalog((int)ListTemplateType.SolutionCatalog);
            var files = solutionGallery.GetItems(CamlQuery.CreateAllItemsQuery());

            context.Load(files);
            context.ExecuteQueryWithTrace();

            var fileItem = files.ToList().FirstOrDefault(f => f["FileLeafRef"].ToString() == fileName);

            return fileItem != null ? fileItem.File : null;
        }

        #endregion
    }
}
