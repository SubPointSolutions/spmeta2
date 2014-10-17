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

        private void DeployHideContentTypeLinks(object modelHost, SPList list, RemoveContentTypeLinksDefinition contentTypeOrderDefinition)
        {
            var listContentTypes = list.ContentTypes.OfType<SPContentType>().ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = list,
                ObjectType = typeof(SPList),
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

                if (listContentType != null)
                {
                    try
                    {
                        list.ContentTypes.Delete(listContentType.Id);
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
                ObjectType = typeof(SPList),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });
        }

        #endregion
    }
}
