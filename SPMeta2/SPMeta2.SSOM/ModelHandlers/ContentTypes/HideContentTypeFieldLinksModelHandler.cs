using System;
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
    public class HideContentTypeFieldLinksModelHandler : ContentTypeFieldLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(HideContentTypeFieldLinksDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var contentType = modelHost.WithAssertAndCast<SPContentType>("model", value => value.RequireNotNull());
            var hideContentTypeFieldLinksDefinition = model.WithAssertAndCast<HideContentTypeFieldLinksDefinition>("model", value => value.RequireNotNull());

            DeployHideContentTypeLinks(modelHost, contentType, hideContentTypeFieldLinksDefinition);
        }

        private void DeployHideContentTypeLinks(object modelHost, SPContentType contentType, HideContentTypeFieldLinksDefinition hideFieldLinksModel)
        {
            var fieldLinks = contentType.FieldLinks.OfType<SPFieldLink>().ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = contentType,
                ObjectType = typeof(SPContentType),
                ObjectDefinition = hideFieldLinksModel,
                ModelHost = modelHost
            });

            // re-order
            foreach (var srcFieldLink in hideFieldLinksModel.Fields)
            {
                SPFieldLink currentFieldLink = null;

                if (!string.IsNullOrEmpty(srcFieldLink.InternalName))
                    currentFieldLink = fieldLinks.FirstOrDefault(c => c.Name == srcFieldLink.InternalName);

                if (currentFieldLink == null && srcFieldLink.Id.HasValue)
                    currentFieldLink = fieldLinks.FirstOrDefault(c => c.Id == srcFieldLink.Id.Value);

                if (currentFieldLink != null)
                {
                    currentFieldLink.ReadOnly = false;
                    currentFieldLink.Required = false;
                    currentFieldLink.Hidden = true;
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = contentType,
                ObjectType = typeof(SPContentType),
                ObjectDefinition = hideFieldLinksModel,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
