using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHandlers.ContentTypes.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes
{
    public class RemoveContentTypeFieldLinksModelHandler : ContentTypeFieldLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(RemoveContentTypeFieldLinksDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var contentTypeHost = modelHost.WithAssertAndCast<ModelHostContext>("model", value => value.RequireNotNull());
            var hideContentTypeFieldLinksDefinition = model.WithAssertAndCast<RemoveContentTypeFieldLinksDefinition>("model", value => value.RequireNotNull());

            var contentType = ExtractContentTypeFromHost(contentTypeHost);

            DeployHideContentTypeLinks(modelHost, contentType, hideContentTypeFieldLinksDefinition);
        }

        private void DeployHideContentTypeLinks(object modelHost, ContentType contentType, RemoveContentTypeFieldLinksDefinition hideFieldLinksModel)
        {
            var context = contentType.Context;

            context.Load(contentType, c => c.FieldLinks);
            context.ExecuteQuery();

            var fieldLinks = contentType.FieldLinks.ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = contentType,
                ObjectType = typeof(ContentType),
                ObjectDefinition = hideFieldLinksModel,
                ModelHost = modelHost
            });

            // re-order
            foreach (var srcFieldLink in hideFieldLinksModel.Fields)
            {
                FieldLink currentFieldLink = null;

                if (!string.IsNullOrEmpty(srcFieldLink.InternalName))
                    currentFieldLink = fieldLinks.FirstOrDefault(c => c.Name == srcFieldLink.InternalName);

                if (currentFieldLink == null && srcFieldLink.Id.HasValue)
                    currentFieldLink = fieldLinks.FirstOrDefault(c => c.Id == srcFieldLink.Id.Value);

                if (currentFieldLink != null)
                {
                    currentFieldLink.DeleteObject();
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = contentType,
                ObjectType = typeof(ContentType),
                ObjectDefinition = hideFieldLinksModel,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
