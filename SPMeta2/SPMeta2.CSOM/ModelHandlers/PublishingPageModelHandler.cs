using System;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class PublishingPageModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PublishingPageDefinition); }
        }

        #endregion

        #region methods

        protected string GetSafePageFileName(PageDefinitionBase page)
        {
            var fileName = page.FileName;
            if (!fileName.EndsWith(".aspx")) fileName += ".aspx";

            return fileName;
        }


        protected File GetCurrentPage(Folder folder, string pageName)
        {
            var context = folder.Context;

            var files = folder.Files;
            context.Load(files);
            context.ExecuteQuery();

            foreach (var file in files)
            {
                if (file.Name.ToUpper() == pageName.ToUpper())
                    return file;
            }

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var context = folder.Context;

            var pageName = GetSafePageFileName(publishingPageModel);
            var currentPageFile = GetCurrentPage(folder, pageName);

            // #SPBug
            // it turns out that there is no support for the web part page creating via CMOM
            // we we need to get a byte array to 'hack' this pages out..
            // http://stackoverflow.com/questions/6199990/creating-a-sharepoint-2010-page-via-the-client-object-model
            // http://social.technet.microsoft.com/forums/en-US/sharepointgeneralprevious/thread/6565bac1-daf0-4215-96b2-c3b64270ec08

            var currentPageFiles = GetCurrentPage(folder, pageName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentPageFile,
                ObjectType = typeof(File),
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            if ((currentPageFile == null) || (currentPageFile != null && publishingPageModel.NeedOverride))
            {
                var file = new FileCreationInformation();
                var pageContent = PublishingPageTemplates.RedirectionPageMarkup;

                // TODO, need to be fixed
                // add new page
                var fileName = publishingPageModel.FileName;
                if (!fileName.EndsWith(".aspx")) fileName += ".aspx";

                file.Url = fileName;
                file.Content = Encoding.UTF8.GetBytes(pageContent);
                file.Overwrite = publishingPageModel.NeedOverride;

                // just root folder is supported yet
                //if (!string.IsNullOrEmpty(publishingPageModel.FolderUrl))
                //    throw new NotImplementedException("FolderUrl for the web part page model is not supported yet");

                context.Load(folder.Files.Add(file));
                context.ExecuteQuery();

                // TODO, setup publishing page properties

            }

            // TODO
            // gosh, how we are supposed to get Master Page gallery with publishing template having just list here?
            // no SPWeb.ParentSite/Site -> RootWeb..

            //newPage["PublishingPageContent"] = "Yea!";
            //newPage["PublishingPageLayout"] = "/auto-tests/csom-application/_catalogs/masterpage/ArticleLinks.aspx";

            currentPageFile = GetCurrentPage(folder, pageName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentPageFile,
                ObjectType = typeof(File),
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            //currentPageFile..ite();//

            context.ExecuteQuery();
        }

        #endregion
    }
}
