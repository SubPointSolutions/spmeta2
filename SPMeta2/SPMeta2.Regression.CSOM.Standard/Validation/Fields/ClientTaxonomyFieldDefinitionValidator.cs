using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Fields
{
    public class ClientTaxonomyFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        #region constructors

        public ClientTaxonomyFieldDefinitionValidator()
        {
            SkipRequredPropValidation = true;
            SkipDescriptionPropValidation = true;
        }

        #endregion

        #region properties

        public override Type TargetType
        {
            get
            {
                return typeof(TaxonomyFieldDefinition);
            }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            var spObject = GetField(modelHost, definition);
            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);
        }

        #endregion
    }
}
