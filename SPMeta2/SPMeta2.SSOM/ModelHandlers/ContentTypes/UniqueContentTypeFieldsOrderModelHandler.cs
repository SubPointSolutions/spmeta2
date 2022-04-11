﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.SSOM.ModelHandlers.ContentTypes.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers.ContentTypes
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
            var typedModelHost = modelHost.WithAssertAndCast<ContentTypeModelHost>("modelHost", value => value.RequireNotNull()); ;
            var contentTypeOrderDefinition = model.WithAssertAndCast<UniqueContentTypeFieldsOrderDefinition>("model", value => value.RequireNotNull());

            DeployContentTypeOrder(modelHost, typedModelHost, contentTypeOrderDefinition);
        }

        private void DeployContentTypeOrder(object modelHost, ContentTypeModelHost contentTypeModellHost, UniqueContentTypeFieldsOrderDefinition reorderFieldLinksModel)
        {
            var contentType = contentTypeModellHost.HostContentType;

            var fieldLinks = contentType.FieldLinks.OfType<SPFieldLink>().ToList();
            var fields = contentType.Fields.OfType<SPField>().ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = contentType,
                ObjectType = typeof(SPContentType),
                ObjectDefinition = reorderFieldLinksModel,
                ModelHost = modelHost
            });

            var newOrder = new List<string>();

            // re-order
            foreach (var srcFieldLink in reorderFieldLinksModel.Fields)
            {
                SPField currentField = null;

                if (!string.IsNullOrEmpty(srcFieldLink.InternalName))
                    currentField = fields.FirstOrDefault(c => c.InternalName == srcFieldLink.InternalName);

                if (currentField == null && srcFieldLink.Id.HasValue)
                    currentField = fields.FirstOrDefault(c => c.Id == srcFieldLink.Id.Value);

                if (currentField != null)
                {
                    var ctField = contentType.Fields[currentField.Id];

                    // must always be internal name of the field
                    if (!newOrder.Contains(ctField.InternalName))
                        newOrder.Add(ctField.InternalName);
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
                ObjectType = typeof(SPContentType),
                ObjectDefinition = reorderFieldLinksModel,
                ModelHost = modelHost
            });

            contentTypeModellHost.ShouldUpdateHost = true;
        }

        #endregion
    }
}
