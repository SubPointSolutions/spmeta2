using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FieldDefinitionValidator : FieldModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var site = modelHost.WithAssertAndCast<SPSite>("modelHost", value => value.RequireNotNull());
            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            TraceUtils.WithScope(traceScope =>
            {
                var rootWeb = site.RootWeb;
                var fields = rootWeb.AvailableFields;

                var spField = fields[fieldModel.Id];

                traceScope.WriteLine(string.Format("Validate model:[{0}] field:[{1}]", fieldModel, spField));

                // assert base properties
                traceScope.WithTraceIndent(trace =>
                {
                    trace.WriteLine(string.Format("Validate InternalName: model:[{0}] field:[{1}]", fieldModel.InternalName, spField.InternalName));
                    Assert.AreEqual(fieldModel.InternalName, spField.InternalName);

                    trace.WriteLine(string.Format("Validate Id: model:[{0}] field:[{1}]", fieldModel.Id, spField.Id));
                    Assert.AreEqual(fieldModel.Id, spField.Id);

                    trace.WriteLine(string.Format("Validate Title: model:[{0}] field:[{1}]", fieldModel.Title, spField.Title));
                    Assert.AreEqual(fieldModel.Title, spField.Title);

                    trace.WriteLine(string.Format("Validate Description: model:[{0}] field:[{1}]", fieldModel.Description, spField.Description));
                    Assert.AreEqual(fieldModel.Description, spField.Description);

                    trace.WriteLine(string.Format("Validate Group: model:[{0}] field:[{1}]", fieldModel.Description, spField.Description));
                    Assert.AreEqual(fieldModel.Group, spField.Group);
                });
            });
        }
    }
}
