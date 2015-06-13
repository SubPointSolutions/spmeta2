using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeFieldLinkModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeFieldLinkDefinition); }
        }

        //protected Field FindExistingListField(List list, ContentTypeFieldLinkDefinition listFieldLinkModel)
        //{
        //    return FindField(list.Fields, listFieldLinkModel);
        //}

        protected Field FindAvailableField(Web web, ContentTypeFieldLinkDefinition listFieldLinkModel)
        {
            return FindField(web.AvailableFields, listFieldLinkModel);
        }

        private Field FindField(FieldCollection fields, ContentTypeFieldLinkDefinition listFieldLinkModel)
        {
            var context = fields.Context;

            var scope = new ExceptionHandlingScope(context);

            Field field = null;

            if (listFieldLinkModel.FieldId.HasGuidValue())
            {
                var id = listFieldLinkModel.FieldId.Value;

                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        fields.GetById(id);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }
            else if (!string.IsNullOrEmpty(listFieldLinkModel.FieldInternalName))
            {
                var fieldInternalName = listFieldLinkModel.FieldInternalName;

                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        fields.GetByInternalNameOrTitle(fieldInternalName);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }

            context.ExecuteQueryWithTrace();

            if (!scope.HasException)
            {
                if (listFieldLinkModel.FieldId.HasGuidValue())
                {
                    field = fields.GetById(listFieldLinkModel.FieldId.Value);
                }
                else if (!string.IsNullOrEmpty(listFieldLinkModel.FieldInternalName))
                {
                    field = fields.GetByInternalNameOrTitle(listFieldLinkModel.FieldInternalName);
                }

                context.Load(field);
                context.Load(field, f => f.SchemaXml);

                context.ExecuteQueryWithTrace();
            }

            return field;
        }

        protected FieldLink FindExistingFieldLink(ContentType contentType, ContentTypeFieldLinkDefinition contentTypeFieldLinkModel)
        {
            var context = contentType.Context;

            context.Load(contentType);
            context.Load(contentType, c => c.FieldLinks);

            context.ExecuteQuery();

            if (contentTypeFieldLinkModel.FieldId.HasGuidValue())
            {
                return contentType.FieldLinks
                                  .ToList()
                                  .FirstOrDefault(l => l.Id == contentTypeFieldLinkModel.FieldId.Value);
            }
            else if (!string.IsNullOrEmpty(contentTypeFieldLinkModel.FieldInternalName))
            {
                return contentType.FieldLinks
                                  .ToList()
                                  .FirstOrDefault(l => l.Name.ToUpper() == contentTypeFieldLinkModel.FieldInternalName.ToUpper());
            }

            throw new ArgumentException("FieldId or FieldInternalName must be defined");
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var modelHostWrapper = modelHost.WithAssertAndCast<ModelHostContext>("modelHost", value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            //var site = modelHostWrapper.Site;
            var web = modelHostWrapper.Web;
            var contentType = modelHostWrapper.ContentType;

            var context = contentType.Context;

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Getting content type field link by ID: [{0}]", contentTypeFieldLinkModel.FieldId);

            FieldLink fieldLink = FindExistingFieldLink(contentType, contentTypeFieldLinkModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = fieldLink,
                ObjectType = typeof(FieldLink),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (fieldLink == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new content type field link");

                var targetField = FindAvailableField(web, contentTypeFieldLinkModel);

                fieldLink = contentType.FieldLinks.Add(new FieldLinkCreationInformation
                {
                    Field = targetField
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing content type field link");
            }

            if (contentTypeFieldLinkModel.Required.HasValue)
                fieldLink.Required = contentTypeFieldLinkModel.Required.Value;

            if (contentTypeFieldLinkModel.Hidden.HasValue)
                fieldLink.Hidden = contentTypeFieldLinkModel.Hidden.Value;

            if (!string.IsNullOrEmpty(contentTypeFieldLinkModel.DisplayName))
            {
                // CSOM limitation - DisplayName is not available yet.
                // https://officespdev.uservoice.com/forums/224641-general/suggestions/7024931-enhance-fieldlink-class-with-additional-properties

                //   fieldLink.DisplayName = contentTypeFieldLinkModel.DisplayName;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = fieldLink,
                ObjectType = typeof(FieldLink),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            contentType.Update(true);
            context.ExecuteQueryWithTrace();
        }


    }
}
