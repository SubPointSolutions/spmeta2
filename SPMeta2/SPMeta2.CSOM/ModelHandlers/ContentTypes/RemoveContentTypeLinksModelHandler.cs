﻿using System;
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

        private void DeployHideContentTypeLinks(object modelHost, List list, RemoveContentTypeLinksDefinition contentTypeOrderDefinition)
        {
            var context = list.Context;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching list content types");

            context.Load(list, l => l.ContentTypes.Include(
               ct => ct.Id,
               ct => ct.Name,
               ct => ct.ReadOnly,

               ct => ct.Parent.Id
               ));
            context.ExecuteQueryWithTrace();

            var listContentTypes = list.ContentTypes.ToList();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = list,
                ObjectType = typeof(List),
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
                    try
                    {
                        TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, string.Format("Deleting list content type"));
                        listContentType.DeleteObject();
                    }
                    catch (Exception e)
                    {
                        TraceService.Error((int)LogEventId.ModelProvisionCoreCall, e);
                    }
                }
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = list,
                ObjectType = typeof(List),
                ObjectDefinition = contentTypeOrderDefinition,
                ModelHost = modelHost
            });

            context.ExecuteQueryWithTrace();
        }

        #endregion
    }
}
