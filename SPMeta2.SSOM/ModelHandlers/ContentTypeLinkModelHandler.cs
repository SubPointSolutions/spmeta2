using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            if (!list.ContentTypesEnabled)
                throw new ArgumentException(string.Format("List [{0}] does not allow content types.", list.RootFolder.ServerRelativeUrl));

            var rootWeb = list.ParentWeb.Site.RootWeb;

            var contentTypeId = new SPContentTypeId(contentTypeLinkModel.ContentTypeId);
            var targetContentType = rootWeb.ContentTypes[contentTypeId];

            if (targetContentType == null)
                throw new ArgumentException(string.Format("Cannot find site content type with ID [{0}].", contentTypeId));

            var currentListContentType = list.ContentTypes[targetContentType.Name];

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentListContentType,
                ObjectType = typeof(SPContentType),
                ObjectDefinition = contentTypeLinkModel,
                ModelHost = modelHost
            });

            if (currentListContentType == null)
            {
                var listCt = list.ContentTypes.Add(targetContentType);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = listCt,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeLinkModel,
                    ModelHost = modelHost
                });

                list.Update();
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentListContentType,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeLinkModel,
                    ModelHost = modelHost
                });

            }
        }

        #endregion
    }
}
