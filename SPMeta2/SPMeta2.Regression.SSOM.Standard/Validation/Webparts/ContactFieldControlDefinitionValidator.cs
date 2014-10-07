using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.SSOM.Validation;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Webparts
{
    public class ContactFieldControlDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContactFieldControlDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);


            // content editor specific validation
        }

        #endregion
    }
}
