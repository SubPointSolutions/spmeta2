using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WebParts;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class DeleteWebPartsModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DeleteWebPartsDefinition); }
        }

        #endregion

        #region methods

        protected File GetCurrentPageFile(ListItemModelHost listItemModelHost)
        {
            var listItem = listItemModelHost.HostListItem;
            var filePath = listItem["FileRef"].ToString();

            var web = listItem.ParentList.ParentWeb;
            return web.GetFileByServerRelativeUrl(filePath);
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var webPartModel = model.WithAssertAndCast<DeleteWebPartsDefinition>("model", value => value.RequireNotNull());

            var listItem = listItemModelHost.HostListItem;

            var context = listItem.Context;
            var currentPageFile = GetCurrentPageFile(listItemModelHost);

            ModuleFileModelHandler.WithSafeFileOperation(listItem.ParentList, currentPageFile, pageFile =>
            {
                var fileListItem = pageFile.ListItemAllFields;
                var fileContext = pageFile.Context;

                fileContext.Load(fileListItem);
                fileContext.ExecuteQueryWithTrace();

                var webPartManager = pageFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

                // web part on the page
                var webpartOnPage = webPartManager.WebParts.Include(wp => wp.Id, wp => wp.WebPart);
                var webPartDefenitions = context.LoadQuery(webpartOnPage);

                context.ExecuteQueryWithTrace();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = pageFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = webPartModel,
                    ModelHost = modelHost
                });

                // clean up
                foreach (var wp in webPartDefenitions)
                    wp.DeleteWebPart();

                context.ExecuteQueryWithTrace();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = pageFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = webPartModel,
                    ModelHost = modelHost
                });

                return pageFile;
            });
        }

        #endregion
    }
}
