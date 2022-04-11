using SPMeta2.Definitions.Webparts;
using SPMeta2.Utils;

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

            BlogAdminWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.BlogAdminWebPart.webpart");
            ImageWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.ImageWebPart.webpart");

            DocumentSetContentsWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.DocumentSetContentsWebPart.webpart");
            DocumentSetPropertiesWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.DocumentSetPropertiesWebPart.webpart");

            XmlWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.XmlWebPart.webpart");
            UserTasksWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.UserTasksWebPart.webpart");
            GettingStartedWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.GettingStartedWebPart.webpart");

            RSSAggregatorWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.RSSAggregatorWebPart.webpart");
            TableOfContentsWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.TableOfContentsWebPart.webpart");

            SPTimelineWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SPTimelineWebPart.webpart");
            SearchBoxScriptWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SearchBoxScriptWebPart.webpart");

            SimpleFormWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SimpleFormWebPart.webpart");
            SearchNavigationWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SearchNavigationWebPart.webpart");

            BlogLinksWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.BlogLinksWebPart.webpart");

            DataFormWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.DataFormWebPart.webpart");
            SiteDocuments = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SiteDocuments.webpart");

            CategoryWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.CategoryWebPart.webpart");
            CommunityAdminWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.CommunityAdminWebPart.webpart");
            CommunityJoinWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.CommunityJoinWebPart.webpart");
            MembersWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.MembersWebPart.webpart");
            MyMembershipWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.MyMembershipWebPart.webpart");
            PictureLibrarySlideshowWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.PictureLibrarySlideshowWebPart.webpart");
            SocialCommentWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.SocialCommentWebPart.webpart");
            TagCloudWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.TagCloudWebPart.webpart");
            UserDocsWebPart = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.UserDocsWebPart.webpart");

            BlogMonthQuickLaunch = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.BlogMonthQuickLaunch.webpart");

            AdvancedSearchBox = ResourceUtils.ReadFromResourceName(asm, "SPMeta2.Templates.Webparts.AdvancedSearchBox.webpart");

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

        public static string BlogAdminWebPart { get; set; }

        public static string BlogLinksWebPart { get; set; }

        public static string DataFormWebPart { get; set; }

        public static string GettingStartedWebPart { get; set; }

        public static string ImageWebPart { get; set; }

        public static string SimpleFormWebPart { get; set; }

        public static string SPTimelineWebPart { get; set; }

        public static string UserTasksWebPart { get; set; }

        public static string XmlWebPart { get; set; }

        public static string DocumentSetContentsWebPart { get; set; }

        public static string DocumentSetPropertiesWebPart { get; set; }

        public static string RSSAggregatorWebPart { get; set; }

        public static string SearchBoxScriptWebPart { get; set; }

        public static string SearchNavigationWebPart { get; set; }

        public static string TableOfContentsWebPart { get; set; }

        public static string SiteDocuments { get; set; }

        #endregion



        public static string UserDocsWebPart { get; set; }

        public static string PictureLibrarySlideshowWebPart { get; set; }

        public static string MembersWebPart { get; set; }

        public static string TagCloudWebPart { get; set; }

        public static string SocialCommentWebPart { get; set; }

        public static string MyMembershipWebPart { get; set; }

        public static string CommunityJoinWebPart { get; set; }

        public static string CommunityAdminWebPart { get; set; }

        public static string CategoryWebPart { get; set; }

        public static string BlogMonthQuickLaunch { get; set; }

        public static string AdvancedSearchBox { get; set; }
    }
}

