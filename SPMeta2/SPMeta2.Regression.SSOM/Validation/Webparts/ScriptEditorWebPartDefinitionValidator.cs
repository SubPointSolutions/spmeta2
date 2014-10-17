using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;

namespace SPMeta2.Regression.SSOM.Validation.Webparts
{
    public class ScriptEditorWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ScriptEditorWebPartDefinition); }
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
