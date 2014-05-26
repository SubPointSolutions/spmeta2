using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeFieldLinkModelHandler : ModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeFieldLinkDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var contentType = modelHost.WithAssertAndCast<SPContentType>("modelHost", value => value.RequireNotNull());
            var contentTypeFieldLinkModel = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var rootWeb = contentType.ParentWeb;

            // TODO, some validation are required
            contentType.FieldLinks.Add(new SPFieldLink(rootWeb.AvailableFields[contentTypeFieldLinkModel.FieldId]));
        }

        #endregion
    }
}
