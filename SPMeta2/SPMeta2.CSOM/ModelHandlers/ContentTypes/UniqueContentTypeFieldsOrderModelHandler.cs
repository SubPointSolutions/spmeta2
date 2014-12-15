using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.ContentTypes.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes
{
    public class UniqueContentTypeFieldsOrderModelHandler : ContentTypeFieldLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UniqueContentTypeFieldsOrderDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var contentTypeHost = modelHost.WithAssertAndCast<ModelHostContext>("model", value => value.RequireNotNull());
            var contentTypeOrderDefinition = model.WithAssertAndCast<UniqueContentTypeFieldsOrderDefinition>("model", value => value.RequireNotNull());

            var contentType = ExtractContentTypeFromHost(modelHost);

            DeployContentTypeOrder(modelHost, contentType, contentTypeOrderDefinition);
        }

        private void DeployContentTypeOrder(object modelHost, ContentType contentType, UniqueContentTypeFieldsOrderDefinition reorderFieldLinksModel)
        {
            var context = contentType.Context;

            context.Load(contentType, c => c.FieldLinks);
            context.Load(contentType, c => c.Fields);

            context.ExecuteQueryWithTrace();

            var fieldLinks = contentType.FieldLinks.ToList();
            var fields = contentType.Fields.ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = contentType,
                ObjectType = typeof(ContentType),
                ObjectDefinition = reorderFieldLinksModel,
                ModelHost = modelHost
            });

            var newOrder = new List<string>();

            // re-order
            foreach (var srcFieldLink in reorderFieldLinksModel.Fields)
            {
                Field currentField = null;

                if (!string.IsNullOrEmpty(srcFieldLink.InternalName))
                    currentField = fields.FirstOrDefault(c => c.InternalName == srcFieldLink.InternalName);

                if (currentField == null && srcFieldLink.Id.HasValue)
                    currentField = fields.FirstOrDefault(c => c.Id == srcFieldLink.Id.Value);

                if (currentField != null)
                {
                    // must always be internal name of the field
                    if (!newOrder.Contains(currentField.InternalName))
                        newOrder.Add(currentField.InternalName);
                }
            }

            if (newOrder.Count > 0)
                contentType.FieldLinks.Reorder(newOrder.ToArray());

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = contentType,
                ObjectType = typeof(ContentType),
                ObjectDefinition = reorderFieldLinksModel,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
