using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Collections;

namespace SPMeta2.SSOM.ModelHandlers.Base
{
    public abstract class ContentFileModelHandlerBase : SSOMModelHandlerBase
    {
        #region properties

        public abstract string FileExtension { get; set; }

        #endregion

        #region methods
        protected string GetSafePageFileName(ContentPageDefinitionBase page)
        {
            var fileName = page.FileName;
            if (!fileName.EndsWith("." + FileExtension)) fileName += "." + FileExtension;

            return fileName;
        }

        public ContentPageDefinitionBase CurrentModel { get; set; }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;
            var templateModel = model.WithAssertAndCast<ContentPageDefinitionBase>("model", value => value.RequireNotNull());

            CurrentModel = templateModel;

            DeployPage(modelHost, listModelHost.CurrentLibrary, folder, templateModel);
        }

        private void DeployPage(object modelHost, SPList list, SPFolder folder, ContentPageDefinitionBase definition)
        {
            var web = list.ParentWeb;
            var targetPage = GetCurrentObject(folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = targetPage == null ? null : targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (targetPage == null)
                targetPage = CreateObject(modelHost, folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ModuleFileModelHandler.WithSafeFileOperation(list, folder,
                targetPage.Url,
                GetSafePageFileName(definition),
                definition.Content,
                definition.NeedOverride,
                null,
                afterFile =>
                {
                    var pageItem = afterFile.Properties;

                    pageItem["vti_title"] = definition.Title;

                    MapProperties(modelHost, pageItem, definition);

                    //pageItem.SystemUpdate();
                });
        }

        private SPListItem CreateObject(object modelHost, SPFolder folder, ContentPageDefinitionBase definition)
        {
            var pageName = GetSafePageFileName(definition);
            var fileContent = definition.Content;

            ModuleFileModelHandler.DeployModuleFile(folder,
                  SPUrlUtility.CombineUrl(folder.ServerRelativeUrl, pageName),
                  pageName,
                  fileContent,
                  true,
                  null,
                  null);

            return GetCurrentObject(folder, definition);
        }


        protected SPListItem GetCurrentObject(SPFolder folder, ContentPageDefinitionBase definition)
        {
            // TODO, CAML query
            var pageName = GetSafePageFileName(definition);

            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == pageName.ToUpper())
                    return file.Item;

            return null;
        }

        protected abstract void MapProperties(object modelHost, Hashtable fileProperties, ContentPageDefinitionBase definition);

        #endregion
    }
}
