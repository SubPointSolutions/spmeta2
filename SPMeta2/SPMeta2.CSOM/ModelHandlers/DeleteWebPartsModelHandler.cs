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
            if (listItemModelHost.HostFile != null)
                return listItemModelHost.HostFile;

            return listItemModelHost.HostListItem.File;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var webPartModel = model.WithAssertAndCast<DeleteWebPartsDefinition>("model", value => value.RequireNotNull());

            //var listItem = listItemModelHost.HostListItem;
            var list = listItemModelHost.HostList;

            var context = list.Context;
            var currentPageFile = GetCurrentPageFile(listItemModelHost);

            ModuleFileModelHandler.WithSafeFileOperation(list, currentPageFile, pageFile =>
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

                ProcessWebPartDeletes(webPartDefenitions, webPartModel);

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

        protected virtual Microsoft.SharePoint.Client.WebParts.WebPartDefinition FindWebPartMatch(
            IEnumerable<Microsoft.SharePoint.Client.WebParts.WebPartDefinition> spWebPartDefenitions,
            WebPartMatch wpMatch)
        {
            // by title?
            if (!string.IsNullOrEmpty(wpMatch.Title))
            {
                return spWebPartDefenitions.FirstOrDefault(w => w.WebPart.Title.ToUpper() == wpMatch.Title.ToUpper());
            }
            else
            {
                // TODO, more support by ID/Type later
                // https://github.com/SubPointSolutions/spmeta2/issues/432
            }

            return null;
        }

        protected virtual void ProcessWebPartDeletes(
            IEnumerable<Microsoft.SharePoint.Client.WebParts.WebPartDefinition> spWebPartDefenitions,
            DeleteWebPartsDefinition definition)
        {
            var webParts2Delete = new List<Microsoft.SharePoint.Client.WebParts.WebPartDefinition>();

            if (definition.WebParts.Any())
            {
                foreach (var webPartMatch in definition.WebParts)
                {
                    var currentWebPartMatch = FindWebPartMatch(spWebPartDefenitions, webPartMatch);

                    if (currentWebPartMatch != null && !webParts2Delete.Contains(currentWebPartMatch))
                        webParts2Delete.Add(currentWebPartMatch);
                }
            }
            else
            {
                webParts2Delete.AddRange(spWebPartDefenitions);
            }

            // clean up
            foreach (var wp in webParts2Delete)
                wp.DeleteWebPart();
        }

        #endregion
    }
}
