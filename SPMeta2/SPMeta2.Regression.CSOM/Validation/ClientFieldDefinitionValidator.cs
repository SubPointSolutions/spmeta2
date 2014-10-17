using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;

using SPMeta2.Regression.Utils;
using SPMeta2.Utils;


namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientFieldDefinitionValidator : FieldModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            Field spObject = null;

            if (modelHost is SiteModelHost)
                spObject = FindSiteField(modelHost as SiteModelHost, definition);
            else if (modelHost is ListModelHost)
                spObject = FindListField((modelHost as ListModelHost).HostList, definition);
            else
            {
                throw new SPMeta2NotSupportedException(
                    string.Format("Validation for artifact of type [{0}] under model host [{1}] is not supported.",
                    definition.GetType(),
                    modelHost.GetType()));
            }

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                    .ShouldBeEqual(m => m.InternalName, o => o.InternalName)
                    .ShouldBeEqual(m => m.Id, o => o.Id)
                    .ShouldBeEqual(m => m.Required, o => o.Required)
                    .ShouldBeEqual(m => m.Description, o => o.Description)
                    .ShouldBeEqual(m => m.FieldType, o => o.TypeAsString)
                    .ShouldBeEqual(m => m.Group, o => o.Group);
        }
    }
}
