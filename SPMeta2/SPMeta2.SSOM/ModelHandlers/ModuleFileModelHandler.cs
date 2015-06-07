using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Exceptions;
using System.Web.UI.WebControls.WebParts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ModuleFileModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ModuleFileDefinition); }
        }

        #endregion

        #region methods

        protected SPFile GetFile(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            if (folderHost.CurrentWebFolder != null)
                return folderHost.CurrentWebFolder.ParentWeb.GetFile(GetSafeFileUrl(folderHost.CurrentWebFolder, moduleFile));

            if (folderHost.CurrentContentType != null)
                return folderHost.CurrentContentTypeFolder.ParentWeb.GetFile(GetSafeFileUrl(folderHost.CurrentContentTypeFolder, moduleFile));

            if (folderHost.CurrentLibraryFolder != null)
                return folderHost.CurrentLibraryFolder.ParentWeb.GetFile(GetSafeFileUrl(folderHost.CurrentLibraryFolder, moduleFile));

            throw new ArgumentException("CurrentWebFolder/CurrentContentType/CurrentLibraryFolder should not be null");

        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var moduleFile = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            var folder = folderHost.CurrentLibraryFolder;
            var file = GetFile(folderHost, moduleFile);

            if (childModelType == typeof(WebPartDefinition))
            {
                using (var webPartManager = file.GetLimitedWebPartManager(PersonalizationScope.Shared))
                {
                    var webpartPageHost = new WebpartPageModelHost
                    {
                        HostFile = file,
                        PageListItem = file.Item,
                        SPLimitedWebPartManager = webPartManager
                    };

                    action(webpartPageHost);
                }
            }
            else
            {
                action(file);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var moduleFile = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            if (folderHost.CurrentWebFolder != null)
                ProcessWebModuleFile(folderHost, moduleFile);
            else if (folderHost.CurrentContentType != null)
                ProcessContentTypeModuleFile(folderHost, moduleFile);
            else if (folderHost.CurrentLibraryFolder != null)
            {
                ProcessFile(modelHost, folderHost.CurrentLibraryFolder, moduleFile);
            }
            else
            {
                throw new ArgumentException("CurrentWebFolder/CurrentContentType/CurrentLibraryFolder should not be null");
            }
        }

        private void ProcessContentTypeModuleFile(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            var folder = folderHost.CurrentContentTypeFolder;

            var currentFile = folder.ParentWeb.GetFile(GetSafeFileUrl(folder, moduleFile));

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFile.Exists ? currentFile : null,
                ObjectType = typeof(SPFile),
                ObjectDefinition = moduleFile,
                ModelHost = folderHost
            });

            if (moduleFile.Overwrite)
            {
                var file = folder.Files.Add(moduleFile.FileName, moduleFile.Content, moduleFile.Overwrite);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = file,
                    ObjectType = typeof(SPFile),
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
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = moduleFile,
                    ModelHost = folderHost
                });
            }

            folder.Update();
        }

        private void ProcessWebModuleFile(FolderModelHost folderHost, ModuleFileDefinition moduleFile)
        {
            var folder = folderHost.CurrentWebFolder;

            var currentFile = folder.ParentWeb.GetFile(GetSafeFileUrl(folder, moduleFile));

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFile.Exists ? currentFile : null,
                ObjectType = typeof(SPFile),
                ObjectDefinition = moduleFile,
                ModelHost = folderHost
            });

            if (moduleFile.Overwrite)
            {
                var file = folder.Files.Add(moduleFile.FileName, moduleFile.Content, moduleFile.Overwrite);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = file,
                    ObjectType = typeof(SPFile),
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
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = moduleFile,
                    ModelHost = folderHost
                });
            }

            folder.Update();
        }

        private string GetSafeFileUrl(SPFolder folder, ModuleFileDefinition moduleFile)
        {
            if (folder.ServerRelativeUrl != "/")
                return folder.ServerRelativeUrl + "/" + moduleFile.FileName;

            return moduleFile.FileName;
        }

        public static void WithSafeFileOperation(
            SPList list,
            SPFolder folder,
            string fileUrl,
            string fileName,
            byte[] fileContent,
            bool overide,
            Action<SPFile> onBeforeAction,
            Action<SPFile> onAction)
        {
            var file = list.ParentWeb.GetFile(fileUrl);

            if (onBeforeAction != null)
                onBeforeAction(file);

            // are we inside ocument libary, so that check in stuff is needed?
            var isDocumentLibrary = list != null && list.BaseType == SPBaseType.DocumentLibrary;

            if (isDocumentLibrary)
            {
                if (list != null && (file.Exists && file.CheckOutType != SPFile.SPCheckOutType.None))
                    file.UndoCheckOut();

                if (list != null && (list.EnableMinorVersions && file.Exists && file.Level == SPFileLevel.Published))
                    file.UnPublish("Provision");

                if (list != null && (file.Exists && file.CheckOutType == SPFile.SPCheckOutType.None))
                    file.CheckOut();
            }

            SPFile newFile;

            if (overide)
                newFile = folder.Files.Add(fileName, fileContent, file.Exists);
            else
                newFile = file;

            if (onAction != null)
                onAction(newFile);

            newFile.Update();

            if (isDocumentLibrary)
            {
                if (list != null && (file.Exists && file.CheckOutType != SPFile.SPCheckOutType.None))
                    newFile.CheckIn("Provision");

                if (list != null && (list.EnableMinorVersions))
                    newFile.Publish("Provision");

                if (list != null && list.EnableModeration)
                    newFile.Approve("Provision");
            }
        }

        public static void DeployModuleFile(SPFolder folder,
            string fileUrl,
            string fileName,
            byte[] fileContent,
            bool overwrite,
            Action<SPFile> beforeProvision,
            Action<SPFile> afterProvision)
        {
            // doc libs
            SPList list = folder.DocumentLibrary;

            // fallback for the lists assuming deployment to Forms or other places
            if (list == null)
            {
                list = folder.ParentWeb.Lists[folder.ParentListId];
            }

            WithSafeFileOperation(list, folder, fileUrl, fileName, fileContent,
                overwrite,
                onBeforeFile =>
                {
                    if (beforeProvision != null)
                        beforeProvision(onBeforeFile);
                },
                onActionFile =>
                {
                    if (afterProvision != null)
                        afterProvision(onActionFile);
                });
        }

        private void ProcessFile(
            object modelHost,
            SPFolder folder,
            ModuleFileDefinition moduleFile)
        {
            DeployModuleFile(
                folder,
                GetSafeFileUrl(folder, moduleFile),
                moduleFile.FileName,
                moduleFile.Content,
                moduleFile.Overwrite,
                before =>
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioning,
                        Object = before.Exists ? before : null,
                        ObjectType = typeof(SPFile),
                        ObjectDefinition = moduleFile,
                        ModelHost = modelHost
                    });

                },
                after =>
                {
                    if (!string.IsNullOrEmpty(moduleFile.ContentTypeId) ||
                        !string.IsNullOrEmpty(moduleFile.ContentTypeName))
                    {
                        var list = folder.ParentWeb.Lists[folder.ParentListId];

                        if (!string.IsNullOrEmpty(moduleFile.ContentTypeId))
                            after.ListItemAllFields["ContentTypeId"] = LookupListContentTypeById(list, moduleFile.ContentTypeId);

                        if (!string.IsNullOrEmpty(moduleFile.ContentTypeName))
                            after.ListItemAllFields["ContentTypeId"] = LookupListContentTypeByName(list, moduleFile.ContentTypeName);

                        after.ListItemAllFields.Update();
                    }

                    if (moduleFile.DefaultValues.Count > 0)
                    {
                        EnsureDefaultValues(after.ListItemAllFields, moduleFile);
                        after.ListItemAllFields.Update();
                    }

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = after,
                        ObjectType = typeof(SPFile),
                        ObjectDefinition = moduleFile,
                        ModelHost = modelHost
                    });
                });
        }

        private void EnsureDefaultValues(SPListItem newFileItem, ModuleFileDefinition definition)
        {
            foreach (var defaultValue in definition.DefaultValues)
            {
                if (!string.IsNullOrEmpty(defaultValue.FieldName))
                {
                    if (newFileItem.Fields.ContainsFieldWithStaticName(defaultValue.FieldName))
                    {
                        if (newFileItem[defaultValue.FieldName] == null)
                            newFileItem[defaultValue.FieldName] = defaultValue.Value;
                    }
                }
                else if (defaultValue.FieldId.HasValue && defaultValue.FieldId != default(Guid))
                {
                    if (newFileItem.Fields.OfType<SPField>().Any(f => f.Id == defaultValue.FieldId.Value))
                    {
                        if (newFileItem[defaultValue.FieldId.Value] == null)
                            newFileItem[defaultValue.FieldId.Value] = defaultValue.Value;
                    }
                }
            }
        }

        protected SPContentTypeId LookupListContentTypeByName(SPList targetList, string name)
        {
            var targetContentType = targetList.ContentTypes
                   .OfType<SPContentType>()
                   .FirstOrDefault(ct => ct.Name.ToUpper() == name.ToUpper());

            if (targetContentType == null)
                throw new SPMeta2Exception(string.Format("Cannot find content type by name ['{0}'] in list: [{1}]",
                    name, targetList.Title));

            return targetContentType.Id;
        }

        protected SPContentTypeId LookupListContentTypeById(SPList targetList, string contentTypeId)
        {
            return targetList.ContentTypes.BestMatch(new SPContentTypeId(contentTypeId));
        }

        #endregion
    }
}
