using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Services.Impl
{
    public class DefaultClientRuntimeContextService : ClientRuntimeContextServiceBase
    {
        public override void ExecuteQuery(ClientRuntimeContext context)
        {
            context.ExecuteQuery();
        }
    }
}
