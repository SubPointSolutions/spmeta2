using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class FieldDefinitionValidator : FieldModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;

            TraceUtils.WithScope(traceScope =>
            {
                var spField = GetField(modelHost, fieldModel);
                var pair = new ComparePair<FieldDefinition, SPField>(fieldModel, spField);

                traceScope.WriteLine(string.Format("Validating model:[{0}] field:[{1}]", fieldModel, spField));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, w => w.Title)
                    .ShouldBeEqual(trace, m => m.Description, w => w.Description)
                    .ShouldBeEqual(trace, m => m.Group, w => w.Group)
                    .ShouldBeEqual(trace, m => m.InternalName, w => w.InternalName)
                    .ShouldBeEqual(trace, m => m.Id, w => w.Id));
            });
        }
    }
}
