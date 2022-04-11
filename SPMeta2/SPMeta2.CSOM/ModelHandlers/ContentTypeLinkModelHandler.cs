using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.ModelHosts;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost as CSOMModelHostBase;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            var context = list.Context;

            //context.Load(list, l => l.ContentTypes);
            context.Load(list, l => l.ContentTypes.Include(
                ct => ct.Id,
                ct => ct.Name,
                ct => ct.ReadOnly,

                ct => ct.Parent.Id
                ));

            context.ExecuteQueryWithTrace();

            var contentType = FindListContentType(list, contentTypeLinkModel);

            var contentTypeLinkHost = ModelHostBase.Inherit<ContentTypeLinkModelHost>(modelHost, host =>
            {
                host.HostContentType = contentType;
                host.HostList = list;
                host.ShouldUpdateHost = true;
            });

            action(contentTypeLinkHost);

            if (contentTypeLinkHost.ShouldUpdateHost)
            {
                if (!contentType.ReadOnly)
                {
                    contentType.Update(false);
                }
            }

            context.ExecuteQueryWithTrace();
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            var context = list.Context;

            context.Load(list, l => l.ContentTypesEnabled);
            context.ExecuteQueryWithTrace();

            if (list.ContentTypesEnabled)
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "ContentTypesEnabled is TRUE. Processing content type link");

                var web = list.ParentWeb;

                // context.Load(web, w => w.AvailableContentTypes);
                //context.Load(list, l => l.ContentTypes);
                context.Load(list, l => l.ContentTypes.Include(
                    ct => ct.Id,
                    ct => ct.Name,
                    ct => ct.ReadOnly,

                    ct => ct.Parent.Id));

                context.ExecuteQueryWithTrace();

                // load by id, then fallback on name
                ContentType targetContentType = null;

                if (!string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeId))
                {
                    targetContentType = web.AvailableContentTypes.GetById(contentTypeLinkModel.ContentTypeId);
                    context.Load(targetContentType);
                    context.ExecuteQueryWithTrace();
                }

                if (targetContentType == null && !string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeName))
                {
                    var name = contentTypeLinkModel.ContentTypeName;

                    context.Load(web.AvailableContentTypes, c => c.Where(w => w.Name == name));
                    context.ExecuteQueryWithTrace();

                    targetContentType = web.AvailableContentTypes[0];
                }

                if (targetContentType == null || targetContentType.ServerObjectIsNull == true)
                {
                    TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall,
                        "Cannot find site content type by ID: [{0}] or Name:[{1}].",
                        new object[] { contentTypeLinkModel.ContentTypeId, contentTypeLinkModel.ContentTypeName });

                    throw new SPMeta2Exception(string.Format("Cannot find site content type by ID: [{0}] or Name:[{1}].",
                        new object[] { contentTypeLinkModel.ContentTypeId, contentTypeLinkModel.ContentTypeName }));
                }

                var listContentType = FindListContentType(list, contentTypeLinkModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = listContentType,
                    ObjectType = typeof(ContentType),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                if (targetContentType != null && listContentType == null)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list content type link");

                    var ct = list.ContentTypes.Add(new ContentTypeCreationInformation
                    {
                        Description = targetContentType.Description,
                        Group = targetContentType.Group,
                        Name = targetContentType.Name,
                        ParentContentType = targetContentType,
                    });

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = ct,
                        ObjectType = typeof(ContentType),
                        ObjectDefinition = model,
                        ModelHost = modelHost
                    });

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "list.Update()");
                    list.Update();

                    context.ExecuteQueryWithTrace();
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list content type link");

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = listContentType,
                        ObjectType = typeof(ContentType),
                        ObjectDefinition = model,
                        ModelHost = modelHost
                    });

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "listContentType.Update(false)");

                    // no update is required for content type link
                    // besides, CTH wouldn't work

                    // .AddContentTypeLink() should work well with the read-only content types (Content Type Hub)
                    // https://github.com/SubPointSolutions/spmeta2/issues/745

                    // listContentType.Update(false);
                    // context.ExecuteQueryWithTrace();
                }
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "ContentTypesEnabled is FALSE. Provision might break.");
            }
        }

        protected ContentType FindListContentType(List list, ContentTypeLinkDefinition contentTypeLinkModel)
        {
            ContentType result = null;

            // TODO
            // https://github.com/SubPointSolutions/spmeta2/issues/68

            // if content type name was not provided, this fails
            // should be re-done by ID and Name
            // OOTB content types could be binded by ID, and custom content types might be binded by name


            // trying to find by name
            if (!string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeName))
            {
                TraceService.InformationFormat((int)LogEventId.ModelProvisionCoreCall,
                    "ContentTypeName is not NULL. Trying to find list content type by ContentTypeName: [{0}]", contentTypeLinkModel.ContentTypeName);

                result = list.ContentTypes.FindByName(contentTypeLinkModel.ContentTypeName);
            }



            // trying to find by content type id
            // will never be resolved, actually
            // list content types have different ID

            //if (result == null && !string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeId))
            //    result = list.ContentTypes.GetById(contentTypeLinkModel.ContentTypeId);

            // trying to find by beat match
            if (result == null && !string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeId))
            {
                TraceService.InformationFormat((int)LogEventId.ModelProvisionCoreCall,
                    "Trying to find list content type by ContentTypeId: [{0}]", contentTypeLinkModel.ContentTypeId);

                // No SPContentTypeCollection.BestMatch() method avialable.
                // http://officespdev.uservoice.com/forums/224641-general/suggestions/6356289-expose-spcontenttypecollection-bestmatch-for-csom

                // TODO, correct best match impl

                // "Item" ContentTypeLink #1016
                // replacing best match, it does not work on list scoped content types

                // Content type operations within a list
                // http://docs.subpointsolutions.com/spmeta2/kb/kb-m2-000003.html

                //foreach (var contentType in list.ContentTypes)
                //{
                //    if (contentType.Id.ToString().ToUpper().StartsWith(contentTypeLinkModel.ContentTypeId.ToUpper()))
                //        result = contentType;
                //}

                foreach (var contentType in list.ContentTypes)
                {
                    if (contentType.Parent.Id.ToString().ToUpper() == contentTypeLinkModel.ContentTypeId.ToUpper())
                    {
                        result = contentType;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
