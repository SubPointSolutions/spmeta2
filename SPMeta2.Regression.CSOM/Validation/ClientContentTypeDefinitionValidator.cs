using System;
using System.Diagnostics;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Regression.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientContentTypeDefinitionValidator : ContentTypeModelHandler
    {
        protected ContentType FindByName(ContentTypeCollection contentTypes, string name)
        {
            foreach (var ct in contentTypes)
            {
                if (String.Compare(ct.Name, name, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return ct;
            }

            return null;
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var site = modelHost.WithAssertAndCast<Site>("modelHost", value => value.RequireNotNull());
            var definitionModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var rootWeb = site.RootWeb;
            var contentTypes = rootWeb.AvailableContentTypes;

            var context = site.Context;

            context.Load(contentTypes);
            context.ExecuteQuery();

            var spModel = FindByName(contentTypes, definitionModel.Name);

            TraceUtils.WithScope(traceScope =>
            {
                Trace.WriteLine(string.Format("Validate model: {0} ContentType:{1}", definitionModel, spModel));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate Name: model:[{0}] ct:[{1}]", definitionModel.Name, spModel.Name));
                    Assert.AreEqual(definitionModel.Name, spModel.Name);

                    trace.WriteLine(string.Format("Validate Description: model:[{0}] ct:[{1}]", definitionModel.Description, spModel.Description));
                    Assert.AreEqual(definitionModel.Description, spModel.Description);

                    // skipping content type ID validation as it 
                    trace.WriteLine("Skipping content type ID validation as ID cannot be set via ClientOM...");

                    //trace.WriteLine(string.Format("Validate Id: model:[{0}] ct:[{1}]", definitionModel.GetContentTypeId(), spModel.Id));
                    //Assert.AreEqual(new SPContentTypeId(definitionModel.GetContentTypeId()), spModel.Id);

                    trace.WriteLine(string.Format("Validate Group: model:[{0}] ct:[{1}]", definitionModel.Group, spModel.Group));
                    Assert.AreEqual(definitionModel.Group, spModel.Group);
                });
            });
        }
    }
}
