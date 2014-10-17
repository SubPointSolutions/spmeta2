using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers.ContentTypes.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes
{
    public class RemoveContentTypeLinksModelHandler : ContentTypeLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(RemoveContentTypeLinksDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = ExtractListFromHost(modelHost);
            var hideContentTypeLinksDefinition = model.WithAssertAndCast<RemoveContentTypeLinksDefinition>("model", value => value.RequireNotNull());

            DeployHideContentTypeLinks(modelHost, list, hideContentTypeLinksDefinition);
        }

        private void DeployHideContentTypeLinks(object modelHost, List list, RemoveContentTypeLinksDefinition contentTypeOrderDefinition)
        {
            var context = list.Context;

            context.Load(list, l => l.ContentTypes);
            context.ExecuteQuery();

            var listContentTypes = list.ContentTypes.ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = list,
                ObjectType = typeof(List),
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
                    try
                    {
                        listContentType.DeleteObject();
                    }
                    catch (Exception)
                    {
                        // TODO

                    }
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = list,
                ObjectType = typeof(List),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });

            context.ExecuteQuery();
        }

        #endregion
    }
}
