using System;
using System.Linq;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeFieldLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeFieldLinkDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var contentType = modelHost.WithAssertAndCast<SPContentType>("modelHost", value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var rootWeb = contentType.ParentWeb;

            var currentFieldLink = contentType.FieldLinks
                .OfType<SPFieldLink>()
                .FirstOrDefault(l => l.Id == contentTypeFieldLinkModel.FieldId);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFieldLink,
                ObjectType = typeof(SPFieldLink),
                ObjectDefinition = contentTypeFieldLinkModel,
                ModelHost = modelHost
            });

            if (currentFieldLink == null)
            {
                contentType.FieldLinks.Add(new SPFieldLink(rootWeb.AvailableFields[contentTypeFieldLinkModel.FieldId]));

                currentFieldLink = contentType.FieldLinks
                .OfType<SPFieldLink>()
                .FirstOrDefault(l => l.Id == contentTypeFieldLinkModel.FieldId);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentFieldLink,
                ObjectType = typeof(SPFieldLink),
                ObjectDefinition = contentTypeFieldLinkModel,
                ModelHost = modelHost
            });

        }

        #endregion
    }
}
