using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.ContentTypes.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes
{
    public class HideContentTypeLinksModelHandler : ContentTypeLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(HideContentTypeLinksDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = ExtractListFromHost(modelHost);
            var folder = ExtractFolderFromHost(modelHost);

            var hideContentTypeLinksDefinition = model.WithAssertAndCast<HideContentTypeLinksDefinition>("model", value => value.RequireNotNull());

            DeployHideContentTypeLinks(modelHost, list, folder, hideContentTypeLinksDefinition);
        }

        private void DeployHideContentTypeLinks(object modelHost, List list, Folder folder, HideContentTypeLinksDefinition contentTypeOrderDefinition)
        {
            var context = folder.Context;

            context.Load(list, l => l.ContentTypes);
            context.Load(folder, f => f.ContentTypeOrder);

            context.ExecuteQueryWithTrace();

            var oldContentTypeOrder = folder.ContentTypeOrder;
            var newContentTypeOrder = oldContentTypeOrder;

            var listContentTypes = list.ContentTypes.ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = folder,
                ObjectType = typeof(Folder),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });

            // re-order
            foreach (var srcContentTypeDef in contentTypeOrderDefinition.ContentTypes)
            {
                ContentType listContentType = null;

                if (!string.IsNullOrEmpty(srcContentTypeDef.ContentTypeName))
                    listContentType = listContentTypes.FirstOrDefault(c => c.Name == srcContentTypeDef.ContentTypeName);

                if (listContentType == null && !string.IsNullOrEmpty(srcContentTypeDef.ContentTypeId))
                    listContentType = listContentTypes.FirstOrDefault(c => c.Id.ToString().ToUpper().StartsWith(srcContentTypeDef.ContentTypeId.ToUpper()));

                if (listContentType != null)
                {
                    var existingCt = newContentTypeOrder.FirstOrDefault(ct => ct.StringValue == listContentType.StringId);

                    if (existingCt != null && newContentTypeOrder.Contains(existingCt))
                        newContentTypeOrder.Remove(existingCt);
                }
            }

            if (newContentTypeOrder.Count() > 0)
                folder.UniqueContentTypeOrder = newContentTypeOrder;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = folder,
                ObjectType = typeof(Folder),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });

            folder.Update();

            context.ExecuteQueryWithTrace();
        }

        #endregion
    }
}
