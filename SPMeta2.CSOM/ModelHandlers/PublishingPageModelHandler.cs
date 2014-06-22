using System;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

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

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var context = list.Context;

            // #SPBug
            // it turns out that there is no support for the web part page creating via CMOM
            // we we need to get a byte array to 'hack' this pages out..
            // http://stackoverflow.com/questions/6199990/creating-a-sharepoint-2010-page-via-the-client-object-model
            // http://social.technet.microsoft.com/forums/en-US/sharepointgeneralprevious/thread/6565bac1-daf0-4215-96b2-c3b64270ec08

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

            context.Load(list.RootFolder.Files.Add(file));
            context.ExecuteQuery();

            // update properties
            var newPage = list.QueryAndGetItemByFileName(fileName);

            // TODO
            // gosh, how we are supposed to get Master Page gallery with publishing template having just list here?
            // no SPWeb.ParentSite/Site -> RootWeb..

            //newPage["PublishingPageContent"] = "Yea!";
            //newPage["PublishingPageLayout"] = "/auto-tests/csom-application/_catalogs/masterpage/ArticleLinks.aspx";

            newPage.Update();

            context.ExecuteQuery();
        }

        #endregion
    }
}
