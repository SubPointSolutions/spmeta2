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
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            if (modelHost is SiteModelHost)
                ValidateSiteField(modelHost as SiteModelHost, model, fieldModel);
            else if (modelHost is ListModelHost)
                ValidateListField(modelHost as ListModelHost, model, fieldModel);
            else
            {
                throw new SPMeta2NotSupportedException(
                    string.Format("Validation for artifact of type [{0}] under model host [{1}] is not supported.",
                    fieldModel.GetType(),
                    modelHost.GetType()));
            }
        }

        private void ValidateListField(ListModelHost listModelHost, DefinitionBase model, FieldDefinition definition)
        {
            var spObject = FindListField(listModelHost.HostList, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                    .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id)
                    .ShouldBeEqual(m => m.Required, o => o.Required)
                    .ShouldBeEqual(m => m.Description, o => o.Description)
                    .ShouldBeEqual(m => m.Group, o => o.Group);
        }

        private void ValidateSiteField(SiteModelHost siteModelHost, DefinitionBase model, FieldDefinition definition)
        {
            var spObject = FindSiteField(siteModelHost, definition);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                    .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id)
                    .ShouldBeEqual(m => m.Required, o => o.Required)
                    .ShouldBeEqual(m => m.Description, o => o.Description)
                    .ShouldBeEqual(m => m.Group, o => o.Group);
        }
    }
}
