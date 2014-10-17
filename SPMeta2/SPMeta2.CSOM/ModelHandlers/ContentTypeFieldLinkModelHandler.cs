using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
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

            context.ExecuteQuery();

            if (!scope.HasException)
            {
                field = rootWeb.AvailableFields.GetById(id);
                context.Load(field);
                context.ExecuteQuery();
            }

            return field;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var modelHostWrapper = modelHost.WithAssertAndCast<ModelHostContext>("modelHost",
                value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model",
                value => value.RequireNotNull());

            var site = modelHostWrapper.Site;
            var rootWeb = site.RootWeb;
            var contentType = modelHostWrapper.ContentType;

            var context = contentType.Context;

            var tmp = contentType.FieldLinks.GetById(contentTypeFieldLinkModel.FieldId);
            context.ExecuteQuery();

            FieldLink fieldLink = null;

            if (tmp != null && tmp.ServerObjectIsNull.HasValue && !tmp.ServerObjectIsNull.Value)
                fieldLink = tmp;

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
                var targetField = FindExistingSiteField(rootWeb, contentTypeFieldLinkModel.FieldId);

                fieldLink = contentType.FieldLinks.Add(new FieldLinkCreationInformation
                {
                    Field = targetField
                });
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
            context.ExecuteQuery();
        }
    }
}
