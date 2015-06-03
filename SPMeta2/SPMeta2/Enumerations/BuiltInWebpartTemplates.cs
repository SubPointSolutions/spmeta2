using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Enumerations
{
    public static class BuiltInWebPartTemplates
    {
        #region constructors

        static BuiltInWebPartTemplates()
        {
            var asm = typeof(BuiltInWebPartTemplates).Assembly;

            // TODO, build up reflection via attrs

            ContentEditorWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ContentEditorWebPart.webpart");
            XsltListViewWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.XsltListViewWebPart.webpart");
            ScriptEditorWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ScriptEditorWebPart.webpart");
            SiteFeedWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SiteFeedWebPart.webpart");
            ListViewWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ListViewWebPart.webpart");
            ContactFieldControl = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ContactFieldControl.webpart");
            ClientWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ClientWebPart.webpart");

            ContentByQueryWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ContentByQueryWebPart.webpart");
            SummaryLinkWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SummaryLinkWebPart.webpart");
            UserCodeWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.UserCodeWebPart.webpart");

            ResultScriptWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ResultScriptWebPart.webpart");

            ContentBySearchWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ContentBySearchWebPart.webpart");

            PageViewerWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.PageViewerWebPart.webpart");
            ProjectSummaryWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ProjectSummaryWebPart.webpart");

            SilverlightWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SilverlightWebPart.webpart");
            RefinementScriptWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.RefinementScriptWebPart.webpart");
        }

        #endregion

        #region properties


        public static string ProjectSummaryWebPart { get; set; }
        public static string ContentEditorWebPart { get; set; }
        public static string XsltListViewWebPart { get; set; }
        public static string ScriptEditorWebPart { get; set; }
        public static string SiteFeedWebPart { get; set; }
        public static string ListViewWebPart { get; set; }
        public static string ContactFieldControl { get; set; }
        public static string ClientWebPart { get; set; }
        public static string ContentByQueryWebPart { get; set; }
        public static string ContentBySearchWebPart { get; set; }
        public static string SummaryLinkWebPart { get; set; }
        public static string UserCodeWebPart { get; set; }
        public static string ResultScriptWebPart { get; set; }
        public static string PageViewerWebPart { get; set; }

        public static string SilverlightWebPart { get; set; }

        public static string RefinementScriptWebPart { get; set; }

        #endregion
    }
}

