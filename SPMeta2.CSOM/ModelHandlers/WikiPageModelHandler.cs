using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WikiPageModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WikiPageDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            action(modelHost);

            // TODO
            // modelHost change should be implemented later to allow web part provision on the wiki page
        }

        protected string GetSafeWikiPageFileName(WikiPageDefinition wikiPageModel)
        {
            var pageName = wikiPageModel.FileName;
            if (!pageName.EndsWith(".aspx")) pageName += ".aspx";

            return pageName;
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var context = list.Context;

            if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
                throw new Exception("FolderUrl property is not supported yet!");

            var pageName = GetSafeWikiPageFileName(wikiPageModel);

            context.Load(list, l => l.RootFolder.ServerRelativeUrl);
            context.ExecuteQuery();

            var newWikiPageUrl = list.RootFolder.ServerRelativeUrl + "/" + pageName;
            var file = list.ParentWeb.GetFileByServerRelativeUrl(newWikiPageUrl);

            context.Load(file, f => f.Exists);
            context.ExecuteQuery();

            if (!file.Exists)
            {
                var newpage = list.RootFolder.Files.AddTemplateFile(newWikiPageUrl, TemplateFileType.WikiPage);

                context.Load(newpage);
                context.ExecuteQuery();
            }
            else
            {
                // TODO
            }
        }

        #endregion
    }
}
