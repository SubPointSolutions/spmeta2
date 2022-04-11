using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.Base
{
    public abstract class MasterPageModelHandlerBase : SSOMModelHandlerBase
    {
        #region properties

        public abstract string PageContentTypeId { get; set; }
        public abstract string PageFileExtension { get; set; }

        #endregion

        #region methods

        protected virtual void DeployPage(object modelHost, SPList list, SPFolder folder, MasterPageDefinitionBase definition)
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

                    //pageItem[BuiltInInternalFieldNames.Title] = definition.Title;
                    pageItem["vti_title"] = definition.Title;
                    pageItem["MasterPageDescription"] = definition.Description;
                    pageItem[BuiltInInternalFieldNames.ContentTypeId] = PageContentTypeId;

                    if (definition.UIVersion.Count > 0)
                    {
                        var value = new SPFieldMultiChoiceValue();

                        foreach (var v in definition.UIVersion)
                            value.Add(v);

                        pageItem["UIVersion"] = value.ToString();
                    }

                    pageItem["DefaultCssFile"] = definition.DefaultCSSFile;

                    //pageItem.Update();

                    //pageItem.SystemUpdate();
                });
        }

        private SPListItem CreateObject(object modelHost, SPFolder folder, MasterPageDefinitionBase definition)
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

        protected string GetSafePageFileName(PageDefinitionBase pageModel)
        {
            var pageName = pageModel.FileName;

            if (!pageName.ToLower().EndsWith(PageFileExtension.ToLower()))
                pageName += PageFileExtension.ToLower();

            return pageName;
        }

        protected SPListItem GetCurrentObject(SPFolder folder, PageDefinitionBase definition)
        {
            // TODO, CAML query
            var pageName = GetSafePageFileName(definition);

            return GetCurrentObjectByPageName(folder, pageName);
        }

        protected SPListItem GetCurrentObjectByPageName(SPFolder folder, string pageName)
        {
            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == pageName.ToUpper())
                    return file.Item;

            return null;
        }

        #endregion
    }
}
