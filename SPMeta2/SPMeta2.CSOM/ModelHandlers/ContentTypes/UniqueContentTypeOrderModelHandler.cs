using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.ContentTypes.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.ContentTypes
{
    public class UniqueContentTypeOrderModelHandler : ContentTypeLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UniqueContentTypeOrderDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = ExtractListFromHost(modelHost);
            var folder = ExtractFolderFromHost(modelHost);

            var contentTypeOrderDefinition = model.WithAssertAndCast<UniqueContentTypeOrderDefinition>("model", value => value.RequireNotNull());

            DeployContentTypeOrder(modelHost, list, folder, contentTypeOrderDefinition);
        }

        private void DeployContentTypeOrder(object modelHost, List list, Folder folder, UniqueContentTypeOrderDefinition contentTypeOrderDefinition)
        {
            var context = folder.Context;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching list content types and the order");

            context.Load(list, l => l.ContentTypes);
            context.Load(folder, f => f.ContentTypeOrder);

            context.ExecuteQueryWithTrace();

            var oldContentTypeOrder = folder.ContentTypeOrder;
            var newContentTypeOrder = new List<ContentTypeId>();

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
                {
                    listContentType = listContentTypes.FirstOrDefault(c => c.Name.ToUpper() == srcContentTypeDef.ContentTypeName.ToUpper());

                    if (listContentType != null)
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                            string.Format("Found content type by name:[{0}]", srcContentTypeDef.ContentTypeName));
                    }
                }

#if !NET35
                if (listContentType == null && !string.IsNullOrEmpty(srcContentTypeDef.ContentTypeId))
                {
                    listContentType = listContentTypes.FirstOrDefault(c => c.Id.ToString().ToUpper().StartsWith(srcContentTypeDef.ContentTypeId.ToUpper()));

                    if (listContentType != null)
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                            string.Format("Found content type by matching ID start:[{0}]", srcContentTypeDef.ContentTypeId));
                    }
                }
#endif

                if (listContentType != null && !newContentTypeOrder.Contains(listContentType.Id))
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, string.Format("Adding content type to new ordering"));

                    newContentTypeOrder.Add(listContentType.Id);
                }
            }

            // filling up gapes
            foreach (var oldCt in oldContentTypeOrder)
            {
#if !NET35
                if (newContentTypeOrder.Count(c =>
                    c.StringValue.ToString().ToUpper().StartsWith(oldCt.StringValue.ToUpper())) == 0)
                {
                    newContentTypeOrder.Add(oldCt);
                }
#endif

#if NET35
                // .ToString() should return .StringValue of the content type ID
                if (newContentTypeOrder.Count(c =>
                                   c.ToString().ToUpper().StartsWith(oldCt.ToString().ToUpper())) == 0)
                {
                    newContentTypeOrder.Add(oldCt);
                }
#endif

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
