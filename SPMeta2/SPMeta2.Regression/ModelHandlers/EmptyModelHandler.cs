using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;

namespace SPMeta2.Regression.ModelHandlers
{
    public class EmptyModelhandler : ModelHandlerBase
    {

        public override Type TargetType
        {
            get { return typeof(DefinitionBase); }

        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {

        }
    }
}
