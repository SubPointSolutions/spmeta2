using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

using AspWebPart = System.Web.UI.WebControls.WebParts;

namespace SPMeta2.SSOM.Behaviours
{
    public static class WebPartBehaviours
    {
        #region common extensions

        public static AspWebPart.WebPart MakeTitle(this AspWebPart.WebPart webpart, string title)
        {
            webpart.Title = title;

            return webpart;
        }

        public static AspWebPart.WebPart MakeTitleUrl(this AspWebPart.WebPart webpart, string titleUrl)
        {
            webpart.TitleUrl = titleUrl;

            return webpart;
        }

        public static AspWebPart.WebPart MakeWidth(this AspWebPart.WebPart webpart, int widthInPixels)
        {
            webpart.Width = new System.Web.UI.WebControls.Unit(widthInPixels, System.Web.UI.WebControls.UnitType.Pixel);

            return webpart;
        }

        public static AspWebPart.WebPart MakeHeight(this AspWebPart.WebPart webpart, int heightInPixels)
        {
            webpart.Height = new System.Web.UI.WebControls.Unit(heightInPixels, System.Web.UI.WebControls.UnitType.Pixel);

            return webpart;
        }

        public static AspWebPart.WebPart MakeSize(this AspWebPart.WebPart webpart, int widthInPixels, int heightInPixels)
        {
            webpart.Width = new System.Web.UI.WebControls.Unit(widthInPixels, System.Web.UI.WebControls.UnitType.Pixel);
            webpart.Height = new System.Web.UI.WebControls.Unit(heightInPixels, System.Web.UI.WebControls.UnitType.Pixel);

            return webpart;
        }

        public static AspWebPart.WebPart MakeExportMode(this AspWebPart.WebPart webpart, AspWebPart.WebPartExportMode exportMode)
        {
            webpart.ExportMode = exportMode;

            return webpart;
        }

        public static AspWebPart.WebPart MakeHideChrome(this AspWebPart.WebPart webpart)
        {
            return MakeChromeType(webpart, AspWebPart.PartChromeType.None);
        }

        public static AspWebPart.WebPart MakeChromeType(this AspWebPart.WebPart webpart, AspWebPart.PartChromeType chrome)
        {
            webpart.ChromeType = chrome;

            return webpart;
        }

        #endregion

        #region content editor

        public static AspWebPart.WebPart MakeContentEditorText(this AspWebPart.WebPart webpart, string content)
        {
            var ceWebPart = webpart as ContentEditorWebPart;

            if (ceWebPart != null)
            {
                var xmlDoc = new XmlDocument();
                var xmlElement = xmlDoc.CreateElement("Root");

                xmlElement.InnerText = content;

                ceWebPart.Content = xmlElement;
                ceWebPart.Content.InnerText = xmlElement.InnerText;
            }

            return webpart;
        }

        #endregion

        #region XsltListViewWebPart

        public static AspWebPart.WebPart MakeXsltListViewXslLink(this AspWebPart.WebPart webpart,
                                                                 string siteRelativeLink)
        {
            return MakeXsltListViewXslLink(webpart, siteRelativeLink, 86400, true);
        }

        public static AspWebPart.WebPart MakeXsltListViewXslLink(this AspWebPart.WebPart webpart, string xsltLink,
            int cacheXslTimeOut, bool cacheXslStorage)
        {
            var xsltListViewWebPart = webpart as XsltListViewWebPart;

            if (xsltListViewWebPart != null)
            {
                xsltListViewWebPart.XslLink = xsltLink;

                xsltListViewWebPart.CacheXslStorage = cacheXslStorage;
                xsltListViewWebPart.CacheXslTimeOut = cacheXslTimeOut;
            }

            return webpart;
        }

        public static AspWebPart.WebPart MakeXsltListViewBindingToView(this AspWebPart.WebPart webpart, SPView view)
        {
            var xsltListViewWebPart = webpart as XsltListViewWebPart;

            if (xsltListViewWebPart != null)
            {
                // TODO
                // it is impossible to change view with public methods while it has been already set
                // reflection has to be utilized for the future

                xsltListViewWebPart.WebId = view.ParentList.ParentWeb.ID;
                xsltListViewWebPart.ListName = view.ParentList.ID.ToString("B");
                xsltListViewWebPart.ViewGuid = view.ID.ToString("B");
            }

            return webpart;
        }

        #endregion
    }
}
