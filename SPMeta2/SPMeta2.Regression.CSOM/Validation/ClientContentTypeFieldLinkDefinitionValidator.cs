using System;
using System.Diagnostics;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeFieldLinkDefinitionValidator : ContentTypeFieldLinkModelHandler
    {
        protected FieldLink FindFieldLinkById(FieldLinkCollection fiedLinks, Guid fieldId)
        {
            foreach (var fieldLink in fiedLinks)
            {
                if (fieldLink.Id == fieldId)
                    return fieldLink;
            }

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var modelHostContext = modelHost.WithAssertAndCast<ModelHostContext>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ContentTypeFieldLinkDefinition>("model", value => value.RequireNotNull());

            var site = modelHostContext.Site;
            var contentType = modelHostContext.ContentType;

            var context = site.Context;

            context.Load(contentType, ct => ct.FieldLinks);
            context.ExecuteQuery();

            var spObject = FindFieldLinkById(contentType.FieldLinks, definition.FieldId);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FieldId, o => o.Id);
        }
    }
}
