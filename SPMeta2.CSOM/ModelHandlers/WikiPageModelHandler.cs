using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
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

            DeployWikiPage(list, wikiPageModel);


        }

        private void DeployWikiPage(List list, WikiPageDefinition wikiPageModel)
        {
            var context = list.Context;

            var newWikiPageUrl = string.Empty;
            var file = GetWikiPageFile(list, wikiPageModel, out newWikiPageUrl);

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = file,
                ObjectType = typeof(File),
                ObjectDefinition = wikiPageModel,
                ModelHost = list
            });

            if (file == null)
            {
                var newPageFile = list.RootFolder.Files.AddTemplateFile(newWikiPageUrl, TemplateFileType.WikiPage);

                context.Load(newPageFile);
                
                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newPageFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = wikiPageModel,
                    ModelHost = list
                });

                context.ExecuteQuery();

            }
            else
            {
                // TODO,override if force

                InvokeOnModelEvents(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = file,
                    ObjectType = typeof(File),
                    ObjectDefinition = wikiPageModel,
                    ModelHost = list
                });
            }
        }

        protected File GetWikiPageFile(List list, WikiPageDefinition wikiPageModel)
        {
            var newWikiPageUrl = string.Empty;
            var result = GetWikiPageFile(list, wikiPageModel, out newWikiPageUrl);

            return result;
        }

        protected File GetWikiPageFile(List list, WikiPageDefinition wikiPageModel, out string newWikiPageUrl)
        {
            var context = list.Context;

            if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
                throw new Exception("FolderUrl property is not supported yet!");

            var pageName = GetSafeWikiPageFileName(wikiPageModel);

            context.Load(list, l => l.RootFolder.ServerRelativeUrl);
            context.ExecuteQuery();

            newWikiPageUrl = list.RootFolder.ServerRelativeUrl + "/" + pageName;
            var file = list.ParentWeb.GetFileByServerRelativeUrl(newWikiPageUrl);

            context.Load(file, f => f.Exists);
            context.ExecuteQuery();

            if (file.Exists)
                return file;

            return null;
        }

        #endregion
    }
}
