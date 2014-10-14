using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Standard.Enumerations
{

    /// <summary>
    /// Out of the box SharePoint publishing field internal names.
    /// </summary>
    public static class BuiltInInternalPublishingFieldNames
    {
        #region properties

        /// <summary>
        /// Corresponds to built-in field with Title [Number of Ratings], ID [b1996002-9167-45e5-a4df-b2c41c6723c7] and Group: [Content Feedback]'
        /// </summary>
        public static string RatingCount = "RatingCount";
        /// <summary>
        /// Corresponds to built-in field with Title [Page Image], ID [3de94b06-4120-41a5-b907-88773e493458] and Group: [Page Layout Columns]'
        /// </summary>
        public static string PublishingPageImage = "PublishingPageImage";
        /// <summary>
        /// Corresponds to built-in field with Title [Hidden Page], ID [7581e709-5d87-42e7-9fe6-698ef5e86dd3] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingHidden = "PublishingHidden";
        /// <summary>
        /// Corresponds to built-in field with Title [Display Name], ID [983f490b-fc53-4820-9354-e8de646b4b82] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheDisplayName = "PublishingCacheDisplayName";
        /// <summary>
        /// Corresponds to built-in field with Title [Contact Name], ID [7546ad0d-6c33-4501-b470-fb3003ca14ba] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingContactName = "PublishingContactName";
        /// <summary>
        /// Corresponds to built-in field with Title [Title], ID [fa564e0f-0c70-4ab9-b863-0177e6ddd247] and Group: [_Hidden]'
        /// </summary>
        public static string Title = "Title";
        /// <summary>
        /// Corresponds to built-in field with Title [Page Layout], ID [0f800910-b30d-4c8f-b011-8189b2297094] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingPageLayout = "PublishingPageLayout";
        /// <summary>
        /// Corresponds to built-in field with Title [Summary Links 2], ID [27761311-936a-40ba-80cd-ca5e7a540a36] and Group: [Page Layout Columns]'
        /// </summary>
        public static string SummaryLinks2 = "SummaryLinks2";
        /// <summary>
        /// Corresponds to built-in field with Title [Vary by Custom Parameter], ID [4689a812-320e-4623-aab9-10ad68941126] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingVaryByCustom = "PublishingVaryByCustom";
        /// <summary>
        /// Corresponds to built-in field with Title [Scheduling Start Date], ID [51d39414-03dc-4bd0-b777-d3e20cb350f7] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingStartDate = "PublishingStartDate";
        /// <summary>
        /// Corresponds to built-in field with Title [Exempt from Policy], ID [b0227f1a-b179-4d45-855b-a18f03706bcb] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static string _dlc_Exempt = "_dlc_Exempt";
        /// <summary>
        /// Corresponds to built-in field with Title [Vary by User Rights], ID [d4a6af1d-c6d7-4045-8def-cefa25b9ec30] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingVaryByRights = "PublishingVaryByRights";
        /// <summary>
        /// Corresponds to built-in field with Title [Declared Record], ID [f9a44731-84eb-43a4-9973-cd2953ad8646] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static string _vti_ItemDeclaredRecord = "_vti_ItemDeclaredRecord";
        /// <summary>
        /// Corresponds to built-in field with Title [Style Definitions], ID [a932ec3f-94c1-48b1-b6dc-41aaa6eb7e54] and Group: [Page Layout Columns]'
        /// </summary>
        public static string HeaderStyleDefinitions = "HeaderStyleDefinitions";
        /// <summary>
        /// Corresponds to built-in field with Title [Page Icon], ID [3894ec3f-4674-4924-a440-8872bec40cf9] and Group: [Page Layout Columns]'
        /// </summary>
        public static string PublishingPageIcon = "PublishingPageIcon";
        /// <summary>
        /// Corresponds to built-in field with Title [Reusable Text], ID [890e9d41-5a0e-4988-87bf-0fb9d80f60df] and Group: [_Hidden]'
        /// </summary>
        public static string ReusableText = "ReusableText";
        /// <summary>
        /// Corresponds to built-in field with Title [Scheduling End Date], ID [a990e64f-faa3-49c1-aafa-885fda79de62] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingExpirationDate = "PublishingExpirationDate";
        /// <summary>
        /// Corresponds to built-in field with Title [Variations], ID [d211d750-4fe6-4d92-90e8-eb16dff196c8] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingAssociatedVariations = "PublishingAssociatedVariations";
        /// <summary>
        /// Corresponds to built-in field with Title [Allow writers to view cached content], ID [773ed051-58db-4ff2-879b-08b21ab001e0] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheAllowWriters = "PublishingCacheAllowWriters";
        /// <summary>
        /// Corresponds to built-in field with Title [Created By], ID [1df5e554-ec7e-46a6-901d-d85a3881cb18] and Group: [_Hidden]'
        /// </summary>
        public static string Author = "Author";
        /// <summary>
        /// Corresponds to built-in field with Title [Content Type], ID [c042a256-787d-4a6f-8a8a-cf6ab767f12d] and Group: [_Hidden]'
        /// </summary>
        public static string ContentType = "ContentType";
        /// <summary>
        /// Corresponds to built-in field with Title [Display Name], ID [c80f535b-a430-4273-8f4f-f3e95507b62a] and Group: [_Hidden]'
        /// </summary>
        public static string PublishedLinksDisplayName = "PublishedLinksDisplayName";
        /// <summary>
        /// Corresponds to built-in field with Title [Contact Picture], ID [dc47d55f-9bf9-494a-8d5b-e619214dd19a] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingContactPicture = "PublishingContactPicture";
        /// <summary>
        /// Corresponds to built-in field with Title [Vary by Query String Parameters], ID [b8abfc64-c2bd-4c88-8cef-b040c1b9d8c0] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingVaryByParam = "PublishingVaryByParam";
        /// <summary>
        /// Corresponds to built-in field with Title [Url], ID [70b38565-a310-4546-84a7-709cfdc140cf] and Group: [_Hidden]'
        /// </summary>
        public static string PublishedLinksURL = "PublishedLinksURL";
        /// <summary>
        /// Corresponds to built-in field with Title [Target Audiences], ID [61cbb965-1e04-4273-b658-eedaa662f48d] and Group: [Publishing Columns]'
        /// </summary>
        public static string Audience = "Audience";
        /// <summary>
        /// Corresponds to built-in field with Title [Enabled], ID [d8f18167-7cff-4c4e-bdbe-e7b0f01678f3] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheEnabled = "PublishingCacheEnabled";
        /// <summary>
        /// Corresponds to built-in field with Title [Preview Image], ID [188ce56c-61e0-4d2a-9d3e-7561390668f7] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingPreviewImage = "PublishingPreviewImage";
        /// <summary>
        /// Corresponds to built-in field with Title [Redirect URL], ID [ac57186e-e90b-4711-a038-b6c6a62a57dc] and Group: [Page Layout Columns]'
        /// </summary>
        public static string RedirectURL = "RedirectURL";
        /// <summary>
        /// Corresponds to built-in field with Title [Display Description], ID [9550e77a-4d10-464f-bc0c-102d5b1aec42] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheDisplayDescription = "PublishingCacheDisplayDescription";
        /// <summary>
        /// Corresponds to built-in field with Title [Check for Changes], ID [5b4d927c-d383-496b-bc79-1e61bd383019] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheCheckForChanges = "PublishingCacheCheckForChanges";
        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [92bba27e-eef6-41aa-b728-6dd9caf2bde2] and Group: [_Hidden]'
        /// </summary>
        public static string PublishedLinksDescription = "PublishedLinksDescription";
        /// <summary>
        /// Corresponds to built-in field with Title [Variation Group ID], ID [914fdb80-7d4f-4500-bf4c-ce46ad7484a4] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingVariationGroupID = "PublishingVariationGroupID";
        /// <summary>
        /// Corresponds to built-in field with Title [Content Type ID], ID [03e45e84-1992-4d42-9116-26f756012634] and Group: [_Hidden]'
        /// </summary>
        public static string ContentTypeId = "ContentTypeId";
        /// <summary>
        /// Corresponds to built-in field with Title [Page Content], ID [f55c4d88-1f2e-4ad9-aaa8-819af4ee7ee8] and Group: [Page Layout Columns]'
        /// </summary>
        public static string PublishingPageContent = "PublishingPageContent";
        /// <summary>
        /// Corresponds to built-in field with Title [Comments], ID [9da97a8a-1da5-4a77-98d3-4bc10456e700] and Group: [_Hidden]'
        /// </summary>
        public static string Comments = "Comments";
        /// <summary>
        /// Corresponds to built-in field with Title [Original Expiration Date], ID [74e6ae8a-0e3e-4dcb-bbff-b5a016d74d64] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static string _dlc_ExpireDateSaved = "_dlc_ExpireDateSaved";
        /// <summary>
        /// Corresponds to built-in field with Title [Contact E-Mail Address], ID [c79dba91-e60b-400e-973d-c6d06f192720] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingContactEmail = "PublishingContactEmail";
        /// <summary>
        /// Corresponds to built-in field with Title [Variation Relationship Link], ID [766da693-38e5-4b1b-997f-e830b6dfcc7b] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingVariationRelationshipLinkFieldID = "PublishingVariationRelationshipLinkFieldID";
        /// <summary>
        /// Corresponds to built-in field with Title [Automatic Update], ID [e977ed93-da24-4fcc-b77d-ac34eea7288f] and Group: [_Hidden]'
        /// </summary>
        public static string AutomaticUpdate = "AutomaticUpdate";
        /// <summary>
        /// Corresponds to built-in field with Title [Migrated GUID], ID [75bed596-0661-4edd-9724-1d607ab8d3b5] and Group: [_Hidden]'
        /// </summary>
        public static string _PublishingMigratedGuid = "_PublishingMigratedGuid";
        /// <summary>
        /// Corresponds to built-in field with Title [Content Category], ID [3a4b7f98-8d14-4800-8bf5-9ad1dd6a82ee] and Group: [_Hidden]'
        /// </summary>
        public static string ContentCategory = "ContentCategory";
        /// <summary>
        /// Corresponds to built-in field with Title [Show in drop-down menu], ID [32e03f99-6949-466a-a4a6-057c21d4b516] and Group: [_Hidden]'
        /// </summary>
        public static string ShowInRibbon = "ShowInRibbon";
        /// <summary>
        /// Corresponds to built-in field with Title [Perform ACL Check], ID [db03cb99-cf1e-40b8-adc7-913f7181dac3] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCachePerformACLCheck = "PublishingCachePerformACLCheck";
        /// <summary>
        /// Corresponds to built-in field with Title [Rating (0-5)], ID [5a14d1ab-1513-48c7-97b3-657a5ba6c742] and Group: [Content Feedback]'
        /// </summary>
        public static string AverageRating = "AverageRating";
        /// <summary>
        /// Corresponds to built-in field with Title [Cacheability], ID [18f165be-6285-4a57-b3ab-4e9f913d299f] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheability = "PublishingCacheability";
        /// <summary>
        /// Corresponds to built-in field with Title [Reusable HTML], ID [82dd22bf-433e-4260-b26e-5b8360dd9105] and Group: [_Hidden]'
        /// </summary>
        public static string ReusableHtml = "ReusableHtml";
        /// <summary>
        /// Corresponds to built-in field with Title [Associated Content Type], ID [b510aac1-bba3-4652-ab70-2d756c29540f] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingAssociatedContentType = "PublishingAssociatedContentType";
        /// <summary>
        /// Corresponds to built-in field with Title [Duration], ID [bdd1b3c3-18db-4acf-a963-e70ef4227fbc] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingCacheDuration = "PublishingCacheDuration";
        /// <summary>
        /// Corresponds to built-in field with Title [Modified], ID [28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f] and Group: [_Hidden]'
        /// </summary>
        public static string Modified = "Modified";
        /// <summary>
        /// Corresponds to built-in field with Title [Hold and Record Status], ID [3afcc5c7-c6ef-44f8-9479-3561d72f9e8e] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static string _vti_ItemHoldRecordStatus = "_vti_ItemHoldRecordStatus";
        /// <summary>
        /// Corresponds to built-in field with Title [Byline], ID [d3429cc9-adc4-439b-84a8-5679070f84cb] and Group: [Page Layout Columns]'
        /// </summary>
        public static string ArticleByLine = "ArticleByLine";
        /// <summary>
        /// Corresponds to built-in field with Title [Created], ID [8c06beca-0777-48f7-91c7-6da68bc07b69] and Group: [_Hidden]'
        /// </summary>
        public static string Created = "Created";
        /// <summary>
        /// Corresponds to built-in field with Title [Rollup Image], ID [543bc2cf-1f30-488e-8f25-6fe3b689d9ac] and Group: [Page Layout Columns]'
        /// </summary>
        public static string PublishingRollupImage = "PublishingRollupImage";
        /// <summary>
        /// Corresponds to built-in field with Title [IconOverlay], ID [b77cdbcf-5dce-4937-85a7-9fc202705c91] and Group: [_Hidden]'
        /// </summary>
        public static string IconOverlay = "IconOverlay";
        /// <summary>
        /// Corresponds to built-in field with Title [Modified By], ID [d31655d1-1d5b-4511-95a1-7a09e9b75bf2] and Group: [_Hidden]'
        /// </summary>
        public static string Editor = "Editor";
        /// <summary>
        /// Corresponds to built-in field with Title [Contact], ID [aea1a4dd-0f19-417d-8721-95a1d28762ab] and Group: [Publishing Columns]'
        /// </summary>
        public static string PublishingContact = "PublishingContact";
        /// <summary>
        /// Corresponds to built-in field with Title [Expiration Date], ID [acd16fdf-052f-40f7-bb7e-564c269c9fbc] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static string _dlc_ExpireDate = "_dlc_ExpireDate";
        /// <summary>
        /// Corresponds to built-in field with Title [Is Locked For Edit], ID [740931e6-d79e-44a6-a752-a06eb23c11b0] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static string _vti_ItemIsLocked = "_vti_ItemIsLocked";
        /// <summary>
        /// Corresponds to built-in field with Title [Safe for Authenticated Use], ID [0a90b5e8-185a-4dec-bf3c-e60aae08373f] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingAuthenticatedUse = "PublishingAuthenticatedUse";
        /// <summary>
        /// Corresponds to built-in field with Title [Image Caption], ID [66f500e9-7955-49ab-abb1-663621727d10] and Group: [Page Layout Columns]'
        /// </summary>
        public static string PublishingImageCaption = "PublishingImageCaption";
        /// <summary>
        /// Corresponds to built-in field with Title [Article Date], ID [71316cea-40a0-49f3-8659-f0cefdbdbd4f] and Group: [Publishing Columns]'
        /// </summary>
        public static string ArticleStartDate = "ArticleStartDate";
        /// <summary>
        /// Corresponds to built-in field with Title [Vary by HTTP Header], ID [89587dfd-b9ca-4fae-8eb9-ba779e917d48] and Group: [_Hidden]'
        /// </summary>
        public static string PublishingVaryByHeader = "PublishingVaryByHeader";
        /// <summary>
        /// Corresponds to built-in field with Title [Summary Links], ID [b3525efe-59b5-4f0f-b1e4-6e26cb6ef6aa] and Group: [Page Layout Columns]'
        /// </summary>
        public static string SummaryLinks = "SummaryLinks";


        #endregion

    }
}
