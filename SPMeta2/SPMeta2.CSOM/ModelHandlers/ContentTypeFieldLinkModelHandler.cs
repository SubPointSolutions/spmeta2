using System;
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

        private Field FindExistingSiteField(Web rootWeb, Guid id)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Finding site field by ID: [{0}]", id);

            var context = rootWeb.Context;
            var scope = new ExceptionHandlingScope(context);

            Field field = null;

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    rootWeb.AvailableFields.GetById(id);
                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQueryWithTrace();

            if (!scope.HasException)
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Found site field by ID: [{0}]", id);

                field = rootWeb.AvailableFields.GetById(id);
                context.Load(field);

                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.ErrorFormat((int)LogEventId.ModelProvisionCoreCall, "Cannot find site field by ID: [{0}]. Provision might fatally fail.", id);
            }

            return field;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var modelHostWrapper = modelHost.WithAssertAndCast<ModelHostContext>("modelHost", value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var site = modelHostWrapper.Site;
            var rootWeb = site.RootWeb;
            var contentType = modelHostWrapper.ContentType;

            var context = contentType.Context;

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Getting content type field link by ID: [{0}]", contentTypeFieldLinkModel.FieldId);

            var tmp = contentType.FieldLinks.GetById(contentTypeFieldLinkModel.FieldId);
            context.ExecuteQueryWithTrace();

            FieldLink fieldLink = null;

            if (tmp != null && tmp.ServerObjectIsNull.HasValue && !tmp.ServerObjectIsNull.Value)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Found existing field link");

                fieldLink = tmp;
            }

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

                var targetField = FindExistingSiteField(rootWeb, contentTypeFieldLinkModel.FieldId);

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
