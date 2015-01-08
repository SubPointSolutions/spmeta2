using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Utils;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ModuleFileModelHandler : CSOMModelHandlerBase
    {
        #region properties

        private double ContentStreamFileSize = 1024 * 1024 * 1.5;

        public override Type TargetType
        {
            get { return typeof(ModuleFileDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var moduleFile = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            if (folderHost.CurrentList != null)
                ProcessFile(folderHost, moduleFile);
            else if (folderHost.CurrentWeb != null)
                ProcessWebFolder(folderHost, moduleFile);
            else if (folderHost.CurrentContentType != null)
                ProcessContentTypeFolder(folderHost, moduleFile);
            else
            {
                throw new ArgumentException("CurrentContentType/CurrentWeb/CurrentLibrary");
            }
        }

        private void ProcessWebFolder(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            throw new NotImplementedException();
        }

        private void ProcessContentTypeFolder(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            var web = folderHost.HostWeb;
            var folder = folderHost.CurrentContentTypeFolder;

            var context = web.Context;

            if (!folder.IsPropertyAvailable("ServerRelativeUrl"))
            {
                context.Load(folder, f => f.ServerRelativeUrl);
                context.ExecuteQueryWithTrace();
            }

            var currentFile = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, moduleFile));

            context.Load(currentFile, f => f.Exists);
            context.ExecuteQuery();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFile.Exists ? currentFile : null,
                ObjectType = typeof(File),
                ObjectDefinition = moduleFile,
                ModelHost = folderHost
            });

            if (moduleFile.Overwrite)
            {
                var fileCreatingInfo = new FileCreationInformation
                {
                    Url = moduleFile.FileName,
                    Overwrite = true
                };

                if (moduleFile.Content.Length < ContentStreamFileSize)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.Content for small file less than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.Content = moduleFile.Content;
                }
                else
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.ContentStream for big file more than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.ContentStream = new System.IO.MemoryStream(moduleFile.Content);
                }

                var file = folder.Files.Add(fileCreatingInfo);

                folder.Context.ExecuteQuery();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = file,
                    ObjectType = typeof(File),
                    ObjectDefinition = moduleFile,
                    ModelHost = folderHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentFile.Exists ? currentFile : null,
                    ObjectType = typeof(File),
                    ObjectDefinition = moduleFile,
                    ModelHost = folderHost
                });
            }

            folder.Update();
            folder.Context.ExecuteQueryWithTrace();
        }

        private string GetSafeFileUrl(Folder folder, ModuleFileDefinition moduleFile)
        {
            return UrlUtility.CombineUrl(folder.ServerRelativeUrl, moduleFile.FileName);
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var moduleFile = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            var web = folderHost.CurrentWeb;
            var file = ProcessFile(folderHost, moduleFile);
            var context = file.Context;

            if (childModelType == typeof(ListItemFieldValueDefinition))
            {
                var fileListItem = file.ListItemAllFields;

                context.Load(fileListItem, i => i.Id, i => i.ParentList);
                context.ExecuteQueryWithTrace();

                var list = fileListItem.ParentList;
                var item = list.GetItemById(fileListItem.Id);

                context.ExecuteQueryWithTrace();

                var listItemPropertyHost = new ListItemFieldValueModelHost
                {
                    CurrentItem = item
                };

                action(listItemPropertyHost);

                item.Update();

                context.ExecuteQueryWithTrace();
            }
            else if (childModelType == typeof(PropertyDefinition))
            {
                var propModelHost = new PropertyModelHost
                {
                    CurrentFile = file
                };

                action(propModelHost);

                context.ExecuteQueryWithTrace();
            }
            else
            {
                action(file);

                context.ExecuteQueryWithTrace();
            }
        }

        public static void WithSafeFileOperation(List list, File file, Func<File, File> action)
        {
            WithSafeFileOperation(list, file, action, null);
        }

        public static void WithSafeFileOperation(List list, File file, Func<File, File> action, Action<File> onCreated)
        {
            var context = list.Context;

            context.Load(list, l => l.EnableMinorVersions);
            context.Load(list, l => l.EnableModeration);
            context.ExecuteQueryWithTrace();

            if (file != null)
            {
                context.Load(file, f => f.Exists);
                context.ExecuteQueryWithTrace();

                if (file.Exists)
                {
                    context.Load(file, f => f.CheckOutType);
                    context.Load(file, f => f.CheckedOutByUser);
                    context.Load(file, f => f.Level);

                    context.ExecuteQueryWithTrace();
                }
            }

            var shouldRefreshLoad = false;

            if (list != null && file != null && (file.Exists && file.CheckOutType != CheckOutType.None))
            {
                file.UndoCheckOut();
                file.RefreshLoad();

                context.ExecuteQueryWithTrace();
            }

            if (list != null && file != null && (list.EnableMinorVersions) && (file.Exists && file.Level == FileLevel.Published))
            {
                file.UnPublish("Provision");
                file.RefreshLoad();

                context.ExecuteQueryWithTrace();
            }

            if (list != null && file != null && (file.Exists && file.CheckOutType == CheckOutType.None))
            {
                file.CheckOut();
                file.RefreshLoad();

                context.ExecuteQueryWithTrace();
            }

            var spFile = action(file);
            context.ExecuteQueryWithTrace();

            context.Load(spFile, f => f.Exists);
            context.ExecuteQueryWithTrace();

            if (spFile.Exists)
            {
                if (onCreated != null)
                    onCreated(spFile);

                context.Load(spFile, f => f.CheckOutType);
                context.Load(spFile, f => f.Level);

                context.ExecuteQueryWithTrace();
            }

            if (list != null && spFile != null && (spFile.Exists && spFile.CheckOutType != CheckOutType.None))
                spFile.CheckIn("Provision", CheckinType.MajorCheckIn);

            if (list != null && spFile != null && (list.EnableMinorVersions))
                spFile.Publish("Provision");

            if (list != null && spFile != null && (list.EnableModeration))
                spFile.Approve("Provision");

            context.ExecuteQueryWithTrace();
        }

        protected File GetFile(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            Folder folder = null;

            if(folderHost.CurrentList != null)
                 folder = folderHost.CurrentLibraryFolder;
            if (folderHost.CurrentContentType != null)
                folder = folderHost.CurrentContentTypeFolder;

            var web = folderHost.HostWeb;
            var context = web.Context;

            var file = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, moduleFile));

            context.Load(file, f => f.Exists);
            context.ExecuteQueryWithTrace();

            return file;
        }

        private File ProcessFile(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            var context = folderHost.CurrentLibraryFolder.Context;

            var web = folderHost.CurrentWeb;
            var list = folderHost.CurrentList;
            var folder = folderHost.CurrentLibraryFolder;

            context.Load(folder, f => f.ServerRelativeUrl);
            context.ExecuteQueryWithTrace();

            if (list != null)
            {
                context.Load(list, l => l.EnableMinorVersions);
                context.Load(list, l => l.EnableVersioning);
                context.Load(list, l => l.EnableModeration);

                context.ExecuteQueryWithTrace();
            }

            var file = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, moduleFile));

            context.Load(file, f => f.Exists);
            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent<ModuleFileDefinition, File>(file, ModelEventType.OnUpdating);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = file.Exists ? file : null,
                ObjectType = typeof(File),
                ObjectDefinition = moduleFile,
                ModelHost = folderHost
            });

            WithSafeFileOperation(list, file, f =>
            {
                var fileName = moduleFile.FileName;
                var fileContent = moduleFile.Content;

                var fileCreatingInfo = new FileCreationInformation
                {
                    Url = fileName,
                    Overwrite = file.Exists
                };

                if (fileContent.Length < ContentStreamFileSize)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.Content for small file less than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.Content = fileContent;
                }
                else
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.ContentStream for big file more than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.ContentStream = new System.IO.MemoryStream(fileContent);
                }

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Overwriting file");
                return folder.Files.Add(fileCreatingInfo);
            });

            var resultFile = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, moduleFile));

            context.Load(resultFile, f => f.Exists);
            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = resultFile,
                ObjectType = typeof(File),
                ObjectDefinition = moduleFile,
                ModelHost = folderHost
            });
            InvokeOnModelEvent<ModuleFileDefinition, File>(resultFile, ModelEventType.OnUpdated);

            return resultFile;
        }

        #endregion
    }
}
