using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WikiPageModelHandler : ModelHandlerBase
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
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
                throw new Exception("FolderUrl property is not supported yet!");

            var pageItem = FindWikiPage(list, wikiPageModel);

            if (pageItem == null)
            {
                var newWikiPageUrl = GetSafeWikiPageUrl(list, wikiPageModel);
                var newpage = list.RootFolder.Files.Add(newWikiPageUrl, SPTemplateFileType.WikiPage);
            }
            else
            {
                // TODO
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
