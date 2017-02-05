using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;
using UrlUtility = SPMeta2.Utils.UrlUtility;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ModuleFileModelHandler : CSOMModelHandlerBase
    {
        #region static

        static ModuleFileModelHandler()
        {
            MaxMinorVersionCount = 50;
        }

        #endregion

        #region properties

        private double ContentStreamFileSize = 1024 * 1024 * 1.5;
        private static int MaxMinorVersionCount { get; set; }

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
            var web = folderHost.HostWeb;
            var folder = folderHost.CurrentWebFolder;

            var context = web.Context;

            if (!folder.IsPropertyAvailable("ServerRelativeUrl"))
            {
                context.Load(folder, f => f.ServerRelativeUrl);
                context.ExecuteQueryWithTrace();
            }

            var currentFile = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, moduleFile));

            context.Load(currentFile, f => f.Exists);
            context.ExecuteQueryWithTrace();

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
#if NET35
                    throw new SPMeta2Exception(string.Format("SP2010 CSOM implementation does no support file more than {0}. Checkout FileCreationInformation and avialabe Content size.", ContentStreamFileSize));
#endif

#if !NET35
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.ContentStream for big file more than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.ContentStream = new System.IO.MemoryStream(moduleFile.Content);
#endif
                }



                var file = folder.Files.Add(fileCreatingInfo);

                folder.Context.ExecuteQueryWithTrace();

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
            context.ExecuteQueryWithTrace();

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
#if NET35
                    throw new SPMeta2Exception(string.Format("SP2010 CSOM implementation does no support file more than {0}. Checkout FileCreationInformation and avialabe Content size.", ContentStreamFileSize));
#endif

#if !NET35
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.ContentStream for big file more than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.ContentStream = new System.IO.MemoryStream(moduleFile.Content);
#endif

                }

                var file = folder.Files.Add(fileCreatingInfo);

                folder.Context.ExecuteQueryWithTrace();

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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


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

                var listItemPropertyHost = new ListItemModelHost
                {
                    HostListItem = item
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
            WithSafeFileOperation(list, file, action, null, true);
        }

        public static void WithSafeFileOperation(List list, File file, Func<File, File> action, bool doesFileHasListItem)
        {
            WithSafeFileOperation(list, file, action, null, doesFileHasListItem);
        }

        public static void WithSafeFileOperation(List list, File file, Func<File, File> action, Action<File> onCreated)
        {
            WithSafeFileOperation(list, file, action, onCreated, true);
        }

        public static void WithSafeFileOperation(List list, File file,
            Func<File, File> action, Action<File> onCreated,
            bool doesFileHasListItem)
        {
            var context = list.Context;

            context.Load(list, l => l.EnableMinorVersions);
            context.Load(list, l => l.EnableModeration);
            context.Load(list, l => l.BaseType);

            context.ExecuteQueryWithTrace();

            if (file != null)
            {
                context.Load(file, f => f.Exists);
                context.ExecuteQueryWithTrace();

                if (file.Exists)
                {
                    context.Load(file);

                    context.Load(file, f => f.CheckOutType);
                    context.Load(file, f => f.CheckedOutByUser);
                    context.Load(file, f => f.Level);

                    context.ExecuteQueryWithTrace();
                }
            }

            // are we inside ocument libary, so that check in stuff is needed?
            var isDocumentLibrary = list != null && list.BaseType == BaseType.DocumentLibrary;

            if (isDocumentLibrary && doesFileHasListItem)
            {
                if (list != null && file != null && (file.Exists && file.CheckOutType != CheckOutType.None))
                {
                    file.UndoCheckOut();
                    file.RefreshLoad();

                    context.ExecuteQueryWithTrace();
                }

                if (list != null && file != null && (list.EnableMinorVersions) &&
                    (file.Exists && file.Level == FileLevel.Published))
                {
                    file.UnPublish("Provision");
                    file.RefreshLoad();

                    context.ExecuteQueryWithTrace();

                    // Module file provision fails at minor version 511 #930
                    // https://github.com/SubPointSolutions/spmeta2/issues/930

                    // checking out .511 version will result in an exception
                    // can be cause by multiple provisions of the same file (such as on dev/test environment)
                    if (file.MinorVersion >= MaxMinorVersionCount)
                    {
                        file.Publish("Provision");
                        file.RefreshLoad();

                        if (list.EnableModeration)
                        {
                            // this is gonna be ugly for SP2010, sorry pals
#if !NET35
                            file.Approve("Provision");
                            file.RefreshLoad();
#endif
                        }

                        context.ExecuteQueryWithTrace();
                    }
                }

                if (list != null && file != null && (file.Exists && file.CheckOutType == CheckOutType.None))
                {
                    file.CheckOut();
                    file.RefreshLoad();

                    context.ExecuteQueryWithTrace();
                }
            }

            var spFile = action(file);
            context.ExecuteQueryWithTrace();

            context.Load(spFile);
            context.Load(spFile, f => f.Exists);

            context.ExecuteQueryWithTrace();

            if (spFile.Exists)
            {
                // super hack with doesFileHasListItem
                // once filed under Forms folder
                // CanDeploy_WebpartTo_ListForm_InLibrary / CanDeploy_WebpartTo_ListForm_InLibrary
                if (isDocumentLibrary && doesFileHasListItem)
                {
                    if (spFile.ListItemAllFields != null)
                    {
                        // weird issues while deployin in a row for wiki
                        if (list.BaseTemplate != 119)
                        {
                            spFile.ListItemAllFields.Update();
                        }
                    }
                }

                context.ExecuteQueryWithTrace();

                if (onCreated != null)
                    onCreated(spFile);

                context.Load(spFile);

                context.Load(spFile, f => f.CheckOutType);
                context.Load(spFile, f => f.Level);

                context.ExecuteQueryWithTrace();
            }

            // super hack with doesFileHasListItem
            // once filed under Forms folder
            // CanDeploy_WebpartTo_ListForm_InLibrary / CanDeploy_WebpartTo_ListForm_InLibrary
            if (isDocumentLibrary && doesFileHasListItem)
            {
                if (list != null && spFile != null && (spFile.Exists && spFile.CheckOutType != CheckOutType.None))
                {
                    spFile.CheckIn("Provision", CheckinType.MajorCheckIn);
                }

                if (list != null && spFile != null && (list.EnableMinorVersions))
                    spFile.Publish("Provision");


                if (list != null && spFile != null && (list.EnableModeration))
                {

#if NET35
                    // TODO, Approve() method is not exposed
                    throw new SPMeta2NotImplementedException("Not implemented for SP2010 - https://github.com/SubPointSolutions/spmeta2/issues/771");
#endif

#if !NET35
                    spFile.Approve("Provision");
#endif

                }

            }

            context.ExecuteQueryWithTrace();
        }

        protected File GetFile(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            Folder folder = null;

            if (folderHost.CurrentList != null)
                folder = folderHost.CurrentListFolder;
            else if (folderHost.CurrentContentType != null)
                folder = folderHost.CurrentContentTypeFolder;
            else if (folderHost.CurrentWebFolder != null)
                folder = folderHost.CurrentWebFolder;

            var web = folderHost.HostWeb;
            var context = web.Context;

            var file = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, moduleFile));

            context.Load(file, f => f.Exists);
            context.ExecuteQueryWithTrace();

            return file;
        }

        protected string ResolveContentTypeId(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            var context = folderHost.CurrentListFolder.Context;
            var list = folderHost.CurrentList;
            var stringCustomContentType = string.Empty;

            if (!string.IsNullOrEmpty(moduleFile.ContentTypeId))
            {
                stringCustomContentType = moduleFile.ContentTypeId;
            }
            else if (!string.IsNullOrEmpty(moduleFile.ContentTypeName))
            {
                stringCustomContentType = ContentTypeLookupService.LookupContentTypeByName(list, moduleFile.ContentTypeName).Id.ToString();
            }

            return stringCustomContentType;
        }

        private File ProcessFile(FolderModelHost folderHost, ModuleFileDefinition definition)
        {
            var context = folderHost.CurrentListFolder.Context;

            var web = folderHost.CurrentWeb;
            var list = folderHost.CurrentList;
            var folder = folderHost.CurrentListFolder;

            context.Load(folder, f => f.ServerRelativeUrl);

#if !NET35
            context.Load(folder, f => f.Properties);
#endif

            context.ExecuteQueryWithTrace();

            var stringCustomContentType = ResolveContentTypeId(folderHost, definition);

            if (list != null)
            {
                context.Load(list, l => l.EnableMinorVersions);
                context.Load(list, l => l.EnableVersioning);
                context.Load(list, l => l.EnableModeration);

                context.ExecuteQueryWithTrace();
            }

            var file = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, definition));

            context.Load(file, f => f.Exists);
            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = file.Exists ? file : null,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = folderHost
            });

#if !NET35
            var doesFileHasListItem =
                //Forms folders
             !(folder != null
              &&
              (folder.Properties.FieldValues.ContainsKey("vti_winfileattribs")
               && folder.Properties.FieldValues["vti_winfileattribs"].ToString() == "00000012"));

#endif

#if NET35
            var doesFileHasListItem = true;
#endif

            WithSafeFileOperation(list, file, f =>
            {
                var fileName = definition.FileName;
                var fileContent = definition.Content;

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
#if NET35
                    throw new SPMeta2Exception(string.Format("SP2010 CSOM implementation does no support file more than {0}. Checkout FileCreationInformation and avialabe Content size.", ContentStreamFileSize));
#endif

#if !NET35
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Using fileCreatingInfo.ContentStream for big file more than: [{0}]", ContentStreamFileSize);
                    fileCreatingInfo.ContentStream = new System.IO.MemoryStream(fileContent);
#endif
                }

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Overwriting file");
                var updatedFile = folder.Files.Add(fileCreatingInfo);

                FieldLookupService.EnsureDefaultValues(updatedFile.ListItemAllFields, definition.DefaultValues);


                if (!string.IsNullOrEmpty(stringCustomContentType))
                    updatedFile.ListItemAllFields[BuiltInInternalFieldNames.ContentTypeId] = stringCustomContentType;

                if (!string.IsNullOrEmpty(definition.Title))
                    updatedFile.ListItemAllFields[BuiltInInternalFieldNames.Title] = definition.Title;


                FieldLookupService.EnsureValues(updatedFile.ListItemAllFields, definition.Values, true);

                if (!string.IsNullOrEmpty(stringCustomContentType)
                    || definition.DefaultValues.Count > 0
                    || definition.Values.Count > 0
                    || !string.IsNullOrEmpty(definition.Title))
                {
                    updatedFile.ListItemAllFields.Update();
                }


                return updatedFile;
            }, doesFileHasListItem);

            var resultFile = web.GetFileByServerRelativeUrl(GetSafeFileUrl(folder, definition));

            context.Load(resultFile, f => f.Exists);
            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = resultFile,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = folderHost
            });

            return resultFile;
        }

        #endregion
    }
}
