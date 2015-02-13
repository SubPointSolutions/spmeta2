using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Behaviours
{
    public static class WebBehaviours
    {
        #region methods

        public static Web MakeWelcomePage(this Web web, string webRelativeUrl)
        {
            var webContex = web.Context;
            var url = webRelativeUrl.TrimStart(new char[] { '\\', '/' });

            webContex.Load(web, r => r.RootFolder);
            webContex.Load(web, r => r.ServerRelativeUrl);

            webContex.ExecuteQuery();

            web.RootFolder.WelcomePage = url;
            web.RootFolder.Update();

            return web;
        }

        #endregion
    }
}
