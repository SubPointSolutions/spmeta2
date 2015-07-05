using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Services
{
    public abstract class ClientRuntimeContextServiceBase
    {
        public abstract void ExecuteQuery(ClientRuntimeContext context);
    }
}
