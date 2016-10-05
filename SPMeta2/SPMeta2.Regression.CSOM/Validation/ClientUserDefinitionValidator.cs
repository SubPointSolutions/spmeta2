using System;
using System.Linq;

using Microsoft.SharePoint.Client;

using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.CSOM.Extensions;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Text;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientUserDefinitionValidator : UserModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // TODO
        }
    }
}
