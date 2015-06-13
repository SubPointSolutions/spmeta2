using System;
using System.Linq;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeFieldLinkModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeFieldLinkDefinition); }
        }

        private SPField FindField(SPFieldCollection fields, ContentTypeFieldLinkDefinition listFieldLinkModel)
        {
            if (listFieldLinkModel.FieldId.HasGuidValue())
            {

                return fields
                    .OfType<SPField>()
                    .FirstOrDefault(f => f.Id == listFieldLinkModel.FieldId.Value);
            }

            if (!string.IsNullOrEmpty(listFieldLinkModel.FieldInternalName))
            {
                return fields
                   .OfType<SPField>()
                   .FirstOrDefault(f => f.InternalName.ToUpper() == listFieldLinkModel.FieldInternalName.ToUpper());
            }

            throw new ArgumentException("FieldId or FieldInternalName should be defined");
        }

        protected SPFieldLink FindExistingFieldLink(SPContentType contentType, ContentTypeFieldLinkDefinition fieldLinkModel)
        {
            if (fieldLinkModel.FieldId.HasValue)
            {
                return contentType.FieldLinks
                      .OfType<SPFieldLink>()
                      .FirstOrDefault(l => l.Id == fieldLinkModel.FieldId);

            }
            else if (!string.IsNullOrEmpty(fieldLinkModel.FieldInternalName))
            {
                return contentType.FieldLinks
                      .OfType<SPFieldLink>()
                      .FirstOrDefault(l => l.Name.ToUpper() == fieldLinkModel.FieldInternalName.ToUpper());
            }

            throw new ArgumentException("FieldId or FieldInternalName must be defined");
        }

        protected SPField FindAvailableField(SPWeb web, ContentTypeFieldLinkDefinition listFieldLinkModel)
        {
            return FindField(web.AvailableFields, listFieldLinkModel);
        }


        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var contentType = modelHost.WithAssertAndCast<SPContentType>("modelHost", value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var web = contentType.ParentWeb;

            var currentFieldLink = FindExistingFieldLink(contentType, contentTypeFieldLinkModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentFieldLink,
                ObjectType = typeof(SPFieldLink),
                ObjectDefinition = contentTypeFieldLinkModel,
                ModelHost = modelHost
            });

            if (currentFieldLink == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new content type field link");

                contentType.FieldLinks.Add(new SPFieldLink(FindAvailableField(web, contentTypeFieldLinkModel)));

                currentFieldLink = FindExistingFieldLink(contentType, contentTypeFieldLinkModel);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing content type field link");
            }

            if (contentTypeFieldLinkModel.Required.HasValue)
                currentFieldLink.Required = contentTypeFieldLinkModel.Required.Value;

            if (contentTypeFieldLinkModel.Hidden.HasValue)
                currentFieldLink.Hidden = contentTypeFieldLinkModel.Hidden.Value;

            if (!string.IsNullOrEmpty(contentTypeFieldLinkModel.DisplayName))
                currentFieldLink.DisplayName = contentTypeFieldLinkModel.DisplayName;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentFieldLink,
                ObjectType = typeof(SPFieldLink),
                ObjectDefinition = contentTypeFieldLinkModel,
                ModelHost = modelHost
            });

        }

        #endregion
    }
}
