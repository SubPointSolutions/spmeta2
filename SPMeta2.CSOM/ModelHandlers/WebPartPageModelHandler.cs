using System;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WebPartPageModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebPartPageDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var list = modelHost as List;
            var webPartPageDefinition = model as WebPartPageDefinition;

            if (list != null && webPartPageDefinition != null)
            {
                var context = list.Context;

                var pageFileName = webPartPageDefinition.FileName;
                if (!pageFileName.EndsWith(".aspx")) pageFileName += ".aspx";

                var page = list.QueryAndGetItemByFileName(pageFileName);

                action(page);

                page.Update();
                context.ExecuteQuery();
            }
            else
            {
                action(modelHost);
            }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var webPartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(webPartPageModel.FolderUrl))
                throw new NotImplementedException("FolderUrl for the web part page model is not supported yet");

            var context = list.Context;

            // #SPBug
            // it turns out that there is no support for the web part page creating via CMOM
            // we we need to get a byte array to 'hack' this pages out..
            // http://stackoverflow.com/questions/6199990/creating-a-sharepoint-2010-page-via-the-client-object-model
            // http://social.technet.microsoft.com/forums/en-US/sharepointgeneralprevious/thread/6565bac1-daf0-4215-96b2-c3b64270ec08

            var file = new FileCreationInformation();

            var pageContent = string.Empty;

            if (!string.IsNullOrEmpty(webPartPageModel.CustomPageLayout))
                pageContent = webPartPageModel.CustomPageLayout;
            else
                pageContent = GetWebPartTemplateContent(webPartPageModel);

            var fileName = GetSafeWebPartPageFileName(webPartPageModel);

            file.Url = fileName;
            file.Content = Encoding.UTF8.GetBytes(pageContent);
            file.Overwrite = webPartPageModel.NeedOverride;

            var newFile = list.RootFolder.Files.Add(file);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = newFile,
                ObjectType = typeof(File),
                ObjectDefinition = webPartPageModel,
                ModelHost = list
            });

            context.Load(newFile);
            context.ExecuteQuery();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = newFile,
                ObjectType = typeof(File),
                ObjectDefinition = webPartPageModel,
                ModelHost = list
            });

        }

        protected string GetSafeWebPartPageFileName(WebPartPageDefinition webPartPageModel)
        {
            var fileName = webPartPageModel.FileName;
            if (!fileName.EndsWith(".aspx")) fileName += ".aspx";

            return fileName;
        }

        protected virtual string GetWebPartTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            // gosh! would u like to offer a better way?
            switch (webPartPageModel.PageLayoutTemplate)
            {
                case 1:
                    return WebPartPageTemplates.spstd1;
                case 2:
                    return WebPartPageTemplates.spstd2;
                case 3:
                    return WebPartPageTemplates.spstd3;
                case 4:
                    return WebPartPageTemplates.spstd4;
                case 5:
                    return WebPartPageTemplates.spstd5;
                case 6:
                    return WebPartPageTemplates.spstd6;
                case 7:
                    return WebPartPageTemplates.spstd7;
                case 8:
                    return WebPartPageTemplates.spstd8;
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        #endregion
    }
}
