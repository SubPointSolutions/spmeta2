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

        /// <summary>
        /// Detects if the context is SharePoint Online one.
        /// Tries to detect the version of the assembly looking for 16.1, 
        /// then fallaback to context.Credentials is SharePointOnlineCredentials
        /// 
        /// Returns false for the rest of the cases.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool IsSharePointOnlineContext(ClientRuntimeContext context)
        {
            // this must be overwritten in the inherited class
            throw new NotImplementedException();
        }
    }
}
