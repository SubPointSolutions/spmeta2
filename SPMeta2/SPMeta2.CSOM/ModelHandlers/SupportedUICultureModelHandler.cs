using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{

    public class SupportedUICultureModelHandler : CSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SupportedUICultureDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModel = model.WithAssertAndCast<SupportedUICultureDefinition>("model", value => value.RequireNotNull());
            var parentHost = modelHost;


        }

        #endregion
    }
}
