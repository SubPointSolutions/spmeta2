using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Client;

using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class UserModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserDefinition); }
        }

        #endregion

        #region methods
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // TODO
        }

        #endregion
    }
}
