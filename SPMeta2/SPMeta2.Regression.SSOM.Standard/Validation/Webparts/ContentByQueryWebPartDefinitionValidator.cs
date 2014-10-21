using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.Standard.Definitions.Webparts;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Webparts
{
    public class ContentByQueryWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentByQueryWebPartDefinition); }
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
