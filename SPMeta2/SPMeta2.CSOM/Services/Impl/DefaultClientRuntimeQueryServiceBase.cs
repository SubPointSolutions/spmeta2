using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.Services.Impl
{
    public class DefaultClientRuntimeQueryService : ClientRuntimeQueryServiceBase
    {
        #region methods
        public override void SetProperty(ClientObject clientObject, string propertyName, object propertyValue)
        {
            var context = clientObject.Context;
            var query = new ClientActionSetProperty(clientObject, propertyName, propertyValue);

            context.AddQuery(query);
        }

        public override void InvokeMethod(ClientObject clientObject, string methodName, object[] methodParameters)
        {
            var context = clientObject.Context;
            var query = new ClientActionInvokeMethod(clientObject, methodName, methodParameters);

            context.AddQuery(query);
        }

        #endregion
    }
}
