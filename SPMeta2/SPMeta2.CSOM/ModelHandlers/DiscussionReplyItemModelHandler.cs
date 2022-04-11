using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

using SPMeta2.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class DiscussionReplyItemModelHandler : ListItemModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DiscussionReplyItemDefinition); }
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
