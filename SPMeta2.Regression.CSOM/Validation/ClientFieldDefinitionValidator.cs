using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Regression.SSOM.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFieldDefinitionValidator : FieldModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            if (modelHost is SiteModelHost)
                ValidateSiteField(modelHost as SiteModelHost, fieldModel);
            else if (modelHost is ListModelHost)
                ValidateListField(modelHost as ListModelHost, fieldModel);
            else
            {
                throw new SPMeta2NotSupportedException(
                    string.Format("Validation for artifact of type [{0}] under model host [{1}] is not supported.",
                    fieldModel.GetType(),
                    modelHost.GetType()));
            }
        }

        private void ValidateListField(ListModelHost listModelHost, FieldDefinition fieldModel)
        {
            throw new SPMeta2NotImplementedException();
        }

        private void ValidateSiteField(SiteModelHost siteModelHost, FieldDefinition model)
        {
            var spObject = FindSiteField(siteModelHost, model);

            TraceUtils.WithScope(traceScope =>
            {
                var pair = new ComparePair<FieldDefinition, Field>(model, spObject);

                traceScope.WriteLine(string.Format("Validating model:[{0}] field:[{1}]", model, spObject));

                traceScope.WithTraceIndent(trace => pair
                    .ShouldBeEqual(trace, m => m.Title, o => o.Title)
                    .ShouldBeEqual(trace, m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(trace, m => m.Id, o => o.Id)
                    .ShouldBeEqual(trace, m => m.Description, o => o.Description)
                    .ShouldBeEqual(trace, m => m.Group, o => o.Group));
            });
        }

        public override System.Type TargetType
        {
            get { return typeof(FieldDefinition); }
        }
    }
}
