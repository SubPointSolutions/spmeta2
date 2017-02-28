using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.Services
{
    public abstract class ClientRuntimeQueryServiceBase
    {
        #region methods
        public abstract void SetProperty(ClientObject clientObject, string propertyName, object propertyValue);


        public void InvokeMethod(ClientObject clientObject, string methodName)
        {
            InvokeMethod(clientObject, methodName, (object[])null);
        }

        public void InvokeMethod(ClientObject clientObject, string methodName, object methodParameter)
        {
            InvokeMethod(clientObject, methodName, new object[] { methodParameter });
        }

        public void InvokeMethod(ClientObject clientObject, string methodName, object p1, object p2)
        {
            InvokeMethod(clientObject, methodName, new object[] { p1, p2 });
        }
        public abstract void InvokeMethod(ClientObject clientObject, string methodName, object[] methodParameters);

        #endregion
    }
}
