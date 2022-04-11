using System;
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
    public class HideContentTypeLinksModelHandler : ContentTypeLinksModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(HideContentTypeLinksDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var list = ExtractListFromHost(modelHost);
            var folder = ExtractFolderFromHost(modelHost);

            var hideContentTypeLinksDefinition = model.WithAssertAndCast<HideContentTypeLinksDefinition>("model", value => value.RequireNotNull());

            DeployHideContentTypeLinks(modelHost, list, folder, hideContentTypeLinksDefinition);
        }

        private void DeployHideContentTypeLinks(object modelHost, List list, Folder folder, HideContentTypeLinksDefinition contentTypeOrderDefinition)
        {
            var context = folder.Context;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching list content types and the order");

            context.Load(list, l => l.ContentTypes.Include(
               ct => ct.Id,
               ct => ct.Name,
               ct => ct.ReadOnly,

               ct => ct.Parent.Id
               ));

            context.Load(folder, f => f.ContentTypeOrder);

            context.ExecuteQueryWithTrace();

            var oldContentTypeOrder = folder.ContentTypeOrder;
            var newContentTypeOrder = oldContentTypeOrder;

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

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Reordeging list content types");

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

                if (listContentType == null && !string.IsNullOrEmpty(srcContentTypeDef.ContentTypeId))
                {
                    foreach (var contentType in list.ContentTypes)
                    {
                        if (contentType.Parent.Id.ToString().ToUpper() == srcContentTypeDef.ContentTypeId.ToUpper())
                        {
                            listContentType = contentType;
                            break;
                        }
                    }

                    if (listContentType != null)
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                            string.Format("Found content type by matching ID start:[{0}]", srcContentTypeDef.ContentTypeId));
                    }
                }

                if (listContentType != null)
                {
#if !NET35

                    var existingCt = newContentTypeOrder.FirstOrDefault(ct => ct.ToString().ToUpper() == listContentType.Id.ToString().ToUpper());

                    if (existingCt != null && newContentTypeOrder.Contains(existingCt))
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, string.Format("Removing content type from the ordering"));
                        newContentTypeOrder.Remove(existingCt);
                    }

#endif

#if NET35
                    // .ToString() should return .StringValue of the content type ID
                    var existingCt = newContentTypeOrder.FirstOrDefault(ct => ct.ToString().ToUpper() == listContentType.ToString().ToUpper());

                    if (existingCt != null && newContentTypeOrder.Contains(existingCt))
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, string.Format("Removing content type from the ordering"));
                        newContentTypeOrder.Remove(existingCt);
                    }
#endif

                }
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
