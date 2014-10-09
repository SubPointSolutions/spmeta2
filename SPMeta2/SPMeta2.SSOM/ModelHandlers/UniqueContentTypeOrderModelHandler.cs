using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class UniqueContentTypeOrderModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UniqueContentTypeOrderDefinition); }
        }

        #endregion

        #region methods

        protected SPFolder ExtractFolderFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.RootFolder;

            if (modelHost is FolderModelHost)
                return (modelHost as FolderModelHost).CurrentLibraryFolder;

            throw new ArgumentException("modelHost needs to be ListModelHost or FolderModelHost");
        }

        protected SPList ExtractListFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            if (modelHost is FolderModelHost)
            {
                var host = modelHost as FolderModelHost;

                return host.CurrentLibrary ?? host.CurrentList;
            }

            throw new ArgumentException("modelHost needs to be ListModelHost or FolderModelHost");
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = ExtractListFromHost(modelHost);
            var folder = ExtractFolderFromHost(modelHost);

            var contentTypeOrderDefinition = model.WithAssertAndCast<UniqueContentTypeOrderDefinition>("model", value => value.RequireNotNull());

            DeployContentTypeOrder(modelHost, list, folder, contentTypeOrderDefinition);
        }

        private void DeployContentTypeOrder(object modelHost, SPList list, SPFolder folder, UniqueContentTypeOrderDefinition contentTypeOrderDefinition)
        {
            var oldContentTypeOrder = folder.ContentTypeOrder;
            var newContentTypeOrder = new List<SPContentType>();

            var listContentTypes = list.ContentTypes.OfType<SPContentType>().ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = folder,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });

            // re-order
            foreach (var srcContentTypeDef in contentTypeOrderDefinition.ContentTypes)
            {
                SPContentType listContentType = null;

                if (!string.IsNullOrEmpty(srcContentTypeDef.ContentTypeName))
                    listContentType = listContentTypes.FirstOrDefault(c => c.Name == srcContentTypeDef.ContentTypeName);

                if (listContentType == null && !string.IsNullOrEmpty(srcContentTypeDef.ContentTypeId))
                    listContentType = listContentTypes.FirstOrDefault(c => c.Id.ToString().ToUpper().StartsWith(srcContentTypeDef.ContentTypeId.ToUpper()));

                if (listContentType != null && !newContentTypeOrder.Contains(listContentType))
                    newContentTypeOrder.Add(listContentType);
            }

            // filling up gapes
            foreach (var oldCt in oldContentTypeOrder)
            {
                if (newContentTypeOrder.Count(c =>
                    c.Name == oldCt.Name ||
                    c.Id.ToString().ToUpper().StartsWith(oldCt.Id.ToString().ToUpper())) == 0)
                    newContentTypeOrder.Add(oldCt);
            }

            folder.UniqueContentTypeOrder = newContentTypeOrder;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = folder,
                ObjectType = typeof(SPFolder),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });

            folder.Update();
        }

        #endregion
    }
}
