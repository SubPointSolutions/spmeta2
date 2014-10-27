using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;

            var context = list.Context;

            context.Load(list, l => l.ContentTypesEnabled);
            context.ExecuteQuery();

            if (list.ContentTypesEnabled)
            {
                var web = list.ParentWeb;

                // context.Load(web, w => w.AvailableContentTypes);
                context.Load(list, l => l.ContentTypes);

                context.ExecuteQuery();

                var targetContentType = web.AvailableContentTypes.GetById(contentTypeLinkModel.ContentTypeId);
                var listContentType = FindListContentType(list, contentTypeLinkModel);

                context.Load(targetContentType);
                context.ExecuteQuery();

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
                    var ct = list.ContentTypes.Add(new ContentTypeCreationInformation
                    {
                        Description = targetContentType.Description,
                        Group = targetContentType.Group,
                        Name = targetContentType.Name,
                        ParentContentType = targetContentType
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

                    list.Update();
                    context.ExecuteQuery();
                }
                else
                {
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

                    listContentType.Update(false);
                    context.ExecuteQuery();
                }
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
                result = list.ContentTypes.FindByName(contentTypeLinkModel.ContentTypeName);

            // trying to find by content type id
            // will never be resilved, actually
            // list content types have different ID

            //if (result == null && !string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeId))
            //    result = list.ContentTypes.GetById(contentTypeLinkModel.ContentTypeId);

            // trying to find by beat match
            if (result == null)
            {
                // No SPContentTypeCollection.BestMatch() method avialable.
                // http://officespdev.uservoice.com/forums/224641-general/suggestions/6356289-expose-spcontenttypecollection-bestmatch-for-csom

                // TODO, correct best match impl
                foreach (var contentType in list.ContentTypes)
                {
                    if (contentType.Id.ToString().ToUpper().StartsWith(contentTypeLinkModel.ContentTypeId.ToUpper()))
                        result = contentType;
                }
            }

            return result;

            //throw new Exception(
            //    string.Format("Either ContentTypeName or ContentTypeId must be provides. Can't lookup current list content type by Name:[{0}] and ContentTypeId:[{1}] provided.",
            //    contentTypeLinkModel.ContentTypeName, contentTypeLinkModel.ContentTypeId));
        }

        //protected ContentType FindSiteContentType(Web web, ContentTypeLinkDefinition contentTypeLinkModel)
        //{
        //    ContentType targetContentType = null;

        //    if (!string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeName))
        //        targetContentType = web.AvailableContentTypes.FindByName(contentTypeLinkModel.ContentTypeName);

        //    if (targetContentType == null && !string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeId))
        //        targetContentType = web.AvailableContentTypes.FindById(contentTypeLinkModel.ContentTypeId);

        //    if (targetContentType == null)
        //        throw new Exception(string.Format("Cannot find content type specified by model: id:[{0}] name:[{1}]",
        //                                    contentTypeLinkModel.ContentTypeId, contentTypeLinkModel.ContentTypeName));

        //    return targetContentType;
        //}
    }
}
