using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeFieldLinkModelHandler : ModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeFieldLinkDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var modelHostWrapper = modelHost.WithAssertAndCast<ModelHostContext>("modelHost", value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var site = modelHostWrapper.Site;
            var rootWeb = site.RootWeb;
            var contentType = modelHostWrapper.ContentType;

            var fields = rootWeb.AvailableFields;

            var context = site.Context;

            // TODO, ad better validation

            var fieldLinks = contentType.FieldLinks;

            context.Load(fields);
            context.Load(fieldLinks);

            context.ExecuteQuery();

            var fieldLink = FindContentTypeFieldLink(fieldLinks, contentTypeFieldLinkModel.FieldId);

            if (fieldLink == null)
            {
                var targetField = FindField(fields, contentTypeFieldLinkModel.FieldId);

                fieldLink = fieldLinks.Add(new FieldLinkCreationInformation
                {
                    Field = targetField
                });
            }

            contentType.Update(true);
            context.ExecuteQuery();
        }

        private Field FindField(FieldCollection fields, Guid guid)
        {
            foreach (var field in fields)
            {
                if (field.Id == guid)
                    return field;
            }

            return null;
        }

        private FieldLink FindContentTypeFieldLink(FieldLinkCollection fieldLinks, Guid guid)
        {
            foreach (var fieldlink in fieldLinks)
            {
                if (fieldlink.Id == guid)
                    return fieldlink;
            }

            return null;
        }
    }
}
