using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WikiPageModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(WikiPageDefinition); }
        }

        protected string FindWikiPartPage(WikiPageDefinition wikiPageModel)
        {
            var pageName = wikiPageModel.FileName;
            if (!pageName.EndsWith(".aspx")) pageName += ".aspx";

            return pageName;
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.CurrentList;

            //if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
            //    throw new Exception("FolderUrl property is not supported yet!");

            var pageItem = FindWikiPage(list, wikiPageModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = pageItem == null ? null : pageItem.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (pageItem == null)
            {
                var newWikiPageUrl = GetSafeWikiPageUrl(list, wikiPageModel);
                var newpage = list.RootFolder.Files.Add(newWikiPageUrl, SPTemplateFileType.WikiPage);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newpage,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                newpage.Update();
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = pageItem.File,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                pageItem.File.Update();
            }
        }

        protected string GetSafeWikiPageUrl(SPList pageList, WikiPageDefinition wikiPageModel)
        {
            // TODO, fix +/ to SPUtility
            return pageList.RootFolder.ServerRelativeUrl + "/" + GetSafeWikiPageFileName(wikiPageModel);
        }

        protected string GetSafeWikiPageFileName(WikiPageDefinition wikiPageModel)
        {
            var wikiPageName = wikiPageModel.FileName;

            if (!wikiPageName.EndsWith(".aspx"))
                wikiPageName += ".aspx";

            return wikiPageName;
        }

        protected SPListItem FindWikiPage(SPList pageList, WikiPageDefinition wikiPageModel)
        {
            var file = pageList.ParentWeb.GetFile(GetSafeWikiPageUrl(pageList, wikiPageModel));

            return file.Exists ? file.Item : null;
        }

        #endregion
    }
}
