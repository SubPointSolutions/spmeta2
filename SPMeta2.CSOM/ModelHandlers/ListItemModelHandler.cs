using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListItemModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListItemDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, Definitions.DefinitionBase model)
        {

        }

        #endregion
    }
}
