using System;
using System.Diagnostics;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Regression.Utils;
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

            var spObject = contentType.FieldLinks.GetById(definition.FieldId);
            context.Load(spObject);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FieldId, o => o.Id);
        }
    }
}
