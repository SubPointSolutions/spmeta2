using System;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        protected SPContentType GetListContentType(SPList list, ContentTypeLinkDefinition definition)
        {
            SPContentType result = null;

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
                result = list.ContentTypes[definition.ContentTypeName];

            if (result == null && !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                var linkContenType = new SPContentTypeId(definition.ContentTypeId);
                var bestMatch = list.ContentTypes.BestMatch(linkContenType);

                if (bestMatch.IsChildOf(linkContenType))
                    result = list.ContentTypes[bestMatch];
            }

            return result;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            if (!list.ContentTypesEnabled)
                throw new ArgumentException(string.Format("List [{0}] does not allow content types.", list.RootFolder.ServerRelativeUrl));

            var rootWeb = list.ParentWeb.Site.RootWeb;

            var contentTypeId = new SPContentTypeId(contentTypeLinkModel.ContentTypeId);
            var targetContentType = rootWeb.ContentTypes[contentTypeId];

            if (targetContentType == null)
                throw new ArgumentException(string.Format("Cannot find site content type with ID [{0}].", contentTypeId));

            var currentListContentType = GetListContentType(list, contentTypeLinkModel);

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

                //list.Update();
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
