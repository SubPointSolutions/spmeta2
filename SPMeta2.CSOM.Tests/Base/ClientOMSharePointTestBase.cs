using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Tests.Services;
using SPMeta2.Regression.Base;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Tests.Base
{
    public class ClientOMSharePointTestBase : SPWebApplicationSandboxTest
    {
        #region contructors

        public ClientOMSharePointTestBase()
        {

        }

        #endregion

        #region properties

        public virtual string SteCollectionTitle
        {
            get
            {
                return "csom-application";
            }
        }

        #endregion

        #region methods

        public virtual void WithStaticSharePointClientContext(Action<ClientContext> action)
        {
            WithStaticSharePointClientContext(SteCollectionTitle, action);
        }

        public virtual void WithStaticSharePointClientContext(string siteCollectionTitle, Action<ClientContext> action)
        {
            base.WithStaticSPSiteSandbox(siteCollectionTitle, (spSite, spWeb) =>
            {
                using (var context = new ClientContext(spWeb.Url))
                    action(context);
            });
        }

        private readonly ModelServiceBase _serviceFactory = new TracebleClientOModelService();

        public virtual ModelServiceBase ServiceFactory
        {
            get
            {
                return _serviceFactory;
            }
        }

        #endregion
    }
}
