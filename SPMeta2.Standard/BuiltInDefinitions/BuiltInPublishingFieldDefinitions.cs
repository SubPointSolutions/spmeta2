using System;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Standard.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint definitions.
    /// </summary>
    public static class BuiltInPublishingFieldDefinitions
    {
        #region properties
        /// <summary>
        /// Corresponds to built-in field with Title [Number of Ratings], ID [b1996002-9167-45e5-a4df-b2c41c6723c7] and Group: [Content Feedback]'
        /// </summary>
        public static FieldDefinition RatingCount = new FieldDefinition
        {
            Title = "Number of Ratings",
            InternalName = "RatingCount",
            Description = "Number of ratings submitted",
            Group = "Content Feedback",
            FieldType = BuiltInFieldTypes.RatingCount,
            Id = new Guid("b1996002-9167-45e5-a4df-b2c41c6723c7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Page Image], ID [3de94b06-4120-41a5-b907-88773e493458] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition PublishingPageImage = new FieldDefinition
        {
            Title = "Page Image",
            InternalName = "PublishingPageImage",
            Description = "Page Image is a site column created by the Publishing feature. It is used on the Article Page Content Type as the primary image of the page.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.Image,
            Id = new Guid("3de94b06-4120-41a5-b907-88773e493458"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hidden Page], ID [7581e709-5d87-42e7-9fe6-698ef5e86dd3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingHidden = new FieldDefinition
        {
            Title = "Hidden Page",
            InternalName = "PublishingHidden",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("7581e709-5d87-42e7-9fe6-698ef5e86dd3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Display Name], ID [983f490b-fc53-4820-9354-e8de646b4b82] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheDisplayName = new FieldDefinition
        {
            Title = "Display Name",
            InternalName = "PublishingCacheDisplayName",
            Description = "Display name is used to populate the list of available cache profiles for site owners and page layout owners.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("983f490b-fc53-4820-9354-e8de646b4b82"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contact Name], ID [7546ad0d-6c33-4501-b470-fb3003ca14ba] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingContactName = new FieldDefinition
        {
            Title = "Contact Name",
            InternalName = "PublishingContactName",
            Description = "Contact Name is a site column created by the Publishing feature. It is used on the Page Content Type as the name of the person or group who is the contact person for the page.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("7546ad0d-6c33-4501-b470-fb3003ca14ba"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Title], ID [fa564e0f-0c70-4ab9-b863-0177e6ddd247] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Title = new FieldDefinition
        {
            Title = "Title",
            InternalName = "Title",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fa564e0f-0c70-4ab9-b863-0177e6ddd247"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Page Layout], ID [0f800910-b30d-4c8f-b011-8189b2297094] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingPageLayout = new FieldDefinition
        {
            Title = "Page Layout",
            InternalName = "PublishingPageLayout",
            Description = "",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("0f800910-b30d-4c8f-b011-8189b2297094"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Summary Links 2], ID [27761311-936a-40ba-80cd-ca5e7a540a36] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition SummaryLinks2 = new FieldDefinition
        {
            Title = "Summary Links 2",
            InternalName = "SummaryLinks2",
            Description = "Summary Links 2 is a site column created by the Publishing feature. It is used on the Welcome Page Content Type to display a second set of links.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.SummaryLinks,
            Id = new Guid("27761311-936a-40ba-80cd-ca5e7a540a36"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Vary by Custom Parameter], ID [4689a812-320e-4623-aab9-10ad68941126] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingVaryByCustom = new FieldDefinition
        {
            Title = "Vary by Custom Parameter",
            InternalName = "PublishingVaryByCustom",
            Description = "As specified by HttpCachePolicy.SetVaryByCustom in ASP.Net 2.0.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4689a812-320e-4623-aab9-10ad68941126"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Scheduling Start Date], ID [51d39414-03dc-4bd0-b777-d3e20cb350f7] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingStartDate = new FieldDefinition
        {
            Title = "Scheduling Start Date",
            InternalName = "PublishingStartDate",
            Description = "Scheduling Start Date is a site column created by the Publishing feature. It is used to specify the date and time on which this page will first appear to site visitors.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.PublishingScheduleStartDateFieldType,
            Id = new Guid("51d39414-03dc-4bd0-b777-d3e20cb350f7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Exempt from Policy], ID [b0227f1a-b179-4d45-855b-a18f03706bcb] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition _dlc_Exempt = new FieldDefinition
        {
            Title = "Exempt from Policy",
            InternalName = "_dlc_Exempt",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.ExemptField,
            Id = new Guid("b0227f1a-b179-4d45-855b-a18f03706bcb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Vary by User Rights], ID [d4a6af1d-c6d7-4045-8def-cefa25b9ec30] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingVaryByRights = new FieldDefinition
        {
            Title = "Vary by User Rights",
            InternalName = "PublishingVaryByRights",
            Description = "Selecting this check box ensures that users must have identical effective permissions on all securable objects to see the same cached page as any other user.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("d4a6af1d-c6d7-4045-8def-cefa25b9ec30"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Declared Record], ID [f9a44731-84eb-43a4-9973-cd2953ad8646] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition _vti_ItemDeclaredRecord = new FieldDefinition
        {
            Title = "Declared Record",
            InternalName = "_vti_ItemDeclaredRecord",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("f9a44731-84eb-43a4-9973-cd2953ad8646"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Style Definitions], ID [a932ec3f-94c1-48b1-b6dc-41aaa6eb7e54] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition HeaderStyleDefinitions = new FieldDefinition
        {
            Title = "Style Definitions",
            InternalName = "HeaderStyleDefinitions",
            Description = "",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.HTML,
            Id = new Guid("a932ec3f-94c1-48b1-b6dc-41aaa6eb7e54"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Page Icon], ID [3894ec3f-4674-4924-a440-8872bec40cf9] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition PublishingPageIcon = new FieldDefinition
        {
            Title = "Page Icon",
            InternalName = "PublishingPageIcon",
            Description = "Page Icon is a site column created by the Publishing feature. It is used on the Page Content Type as the icon image for the page.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.Image,
            Id = new Guid("3894ec3f-4674-4924-a440-8872bec40cf9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reusable Text], ID [890e9d41-5a0e-4988-87bf-0fb9d80f60df] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReusableText = new FieldDefinition
        {
            Title = "Reusable Text",
            InternalName = "ReusableText",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("890e9d41-5a0e-4988-87bf-0fb9d80f60df"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Scheduling End Date], ID [a990e64f-faa3-49c1-aafa-885fda79de62] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingExpirationDate = new FieldDefinition
        {
            Title = "Scheduling End Date",
            InternalName = "PublishingExpirationDate",
            Description = "Scheduling End Date is a site column created by the Publishing feature. It is used to specify the date and time on which this page will no longer appear to site visitors.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.PublishingScheduleEndDateFieldType,
            Id = new Guid("a990e64f-faa3-49c1-aafa-885fda79de62"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Variations], ID [d211d750-4fe6-4d92-90e8-eb16dff196c8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingAssociatedVariations = new FieldDefinition
        {
            Title = "Variations",
            InternalName = "PublishingAssociatedVariations",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.LayoutVariationsField,
            Id = new Guid("d211d750-4fe6-4d92-90e8-eb16dff196c8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Allow writers to view cached content], ID [773ed051-58db-4ff2-879b-08b21ab001e0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheAllowWriters = new FieldDefinition
        {
            Title = "Allow writers to view cached content",
            InternalName = "PublishingCacheAllowWriters",
            Description = "Selecting this check box bypasses the normal behavior of not allowing people with edit permissions to have their pages cached. This check box should be selected only in scenarios in which you know that the page will be published, but will not have any content that might be checked out or in draft.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("773ed051-58db-4ff2-879b-08b21ab001e0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Created By], ID [1df5e554-ec7e-46a6-901d-d85a3881cb18] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Author = new FieldDefinition
        {
            Title = "Created By",
            InternalName = "Author",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("1df5e554-ec7e-46a6-901d-d85a3881cb18"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Content Type], ID [c042a256-787d-4a6f-8a8a-cf6ab767f12d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ContentType = new FieldDefinition
        {
            Title = "Content Type",
            InternalName = "ContentType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("c042a256-787d-4a6f-8a8a-cf6ab767f12d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Display Name], ID [c80f535b-a430-4273-8f4f-f3e95507b62a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishedLinksDisplayName = new FieldDefinition
        {
            Title = "Display Name",
            InternalName = "PublishedLinksDisplayName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c80f535b-a430-4273-8f4f-f3e95507b62a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contact Picture], ID [dc47d55f-9bf9-494a-8d5b-e619214dd19a] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingContactPicture = new FieldDefinition
        {
            Title = "Contact Picture",
            InternalName = "PublishingContactPicture",
            Description = "Contact Picture is a site column created by the Publishing feature. It is used on the Page Content Type as the picture of the user or group who is the contact person for the page.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("dc47d55f-9bf9-494a-8d5b-e619214dd19a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Vary by Query String Parameters], ID [b8abfc64-c2bd-4c88-8cef-b040c1b9d8c0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingVaryByParam = new FieldDefinition
        {
            Title = "Vary by Query String Parameters",
            InternalName = "PublishingVaryByParam",
            Description = "As specified by HttpCachePolicy.VaryByParams in ASP.Net 2.0.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("b8abfc64-c2bd-4c88-8cef-b040c1b9d8c0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Url], ID [70b38565-a310-4546-84a7-709cfdc140cf] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishedLinksURL = new FieldDefinition
        {
            Title = "Url",
            InternalName = "PublishedLinksURL",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("70b38565-a310-4546-84a7-709cfdc140cf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Audiences], ID [61cbb965-1e04-4273-b658-eedaa662f48d] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition Audience = new FieldDefinition
        {
            Title = "Target Audiences",
            InternalName = "Audience",
            Description = "Target Audiences is a site column created by the Publishing feature. It is used to specify audiences to which this page will be targeted.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.TargetTo,
            Id = new Guid("61cbb965-1e04-4273-b658-eedaa662f48d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Enabled], ID [d8f18167-7cff-4c4e-bdbe-e7b0f01678f3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheEnabled = new FieldDefinition
        {
            Title = "Enabled",
            InternalName = "PublishingCacheEnabled",
            Description = "Selecting this check box turns caching on. If you clear this check box, caching will not take place anywhere this profile is selected. Clearing this check box can be useful for troubleshooting the rendering of all sites and page layouts associated with this cache profile. Remember to select this check box when troubleshooting is complete. ",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("d8f18167-7cff-4c4e-bdbe-e7b0f01678f3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Preview Image], ID [188ce56c-61e0-4d2a-9d3e-7561390668f7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingPreviewImage = new FieldDefinition
        {
            Title = "Preview Image",
            InternalName = "PublishingPreviewImage",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("188ce56c-61e0-4d2a-9d3e-7561390668f7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Redirect URL], ID [ac57186e-e90b-4711-a038-b6c6a62a57dc] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition RedirectURL = new FieldDefinition
        {
            Title = "Redirect URL",
            InternalName = "RedirectURL",
            Description = "Redirect URL is a site column created by the Publishing feature. It is used on the Redirect Page Content Type as the web address that the page will redirect to.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("ac57186e-e90b-4711-a038-b6c6a62a57dc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Display Description], ID [9550e77a-4d10-464f-bc0c-102d5b1aec42] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheDisplayDescription = new FieldDefinition
        {
            Title = "Display Description",
            InternalName = "PublishingCacheDisplayDescription",
            Description = "Display description is used to populate the list of available cache profiles for site owners and page layout owners.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9550e77a-4d10-464f-bc0c-102d5b1aec42"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Check for Changes], ID [5b4d927c-d383-496b-bc79-1e61bd383019] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheCheckForChanges = new FieldDefinition
        {
            Title = "Check for Changes",
            InternalName = "PublishingCacheCheckForChanges",
            Description = "Selecting this check box validates on each page request that the site has not changed and flushes the cache on changes to the site. Clearing this check box can improve performance but will not check for updates to the site for the number of seconds specified in duration. ",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("5b4d927c-d383-496b-bc79-1e61bd383019"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [92bba27e-eef6-41aa-b728-6dd9caf2bde2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishedLinksDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "PublishedLinksDescription",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("92bba27e-eef6-41aa-b728-6dd9caf2bde2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Variation Group ID], ID [914fdb80-7d4f-4500-bf4c-ce46ad7484a4] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingVariationGroupID = new FieldDefinition
        {
            Title = "Variation Group ID",
            InternalName = "PublishingVariationGroupID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("914fdb80-7d4f-4500-bf4c-ce46ad7484a4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Content Type ID], ID [03e45e84-1992-4d42-9116-26f756012634] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ContentTypeId = new FieldDefinition
        {
            Title = "Content Type ID",
            InternalName = "ContentTypeId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.ContentTypeId,
            Id = new Guid("03e45e84-1992-4d42-9116-26f756012634"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Page Content], ID [f55c4d88-1f2e-4ad9-aaa8-819af4ee7ee8] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition PublishingPageContent = new FieldDefinition
        {
            Title = "Page Content",
            InternalName = "PublishingPageContent",
            Description = "Page Content is a site column created by the Publishing feature. It is used on the Article Page Content Type as the content of the page.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.HTML,
            Id = new Guid("f55c4d88-1f2e-4ad9-aaa8-819af4ee7ee8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comments], ID [9da97a8a-1da5-4a77-98d3-4bc10456e700] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Comments = new FieldDefinition
        {
            Title = "Comments",
            InternalName = "Comments",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("9da97a8a-1da5-4a77-98d3-4bc10456e700"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Original Expiration Date], ID [74e6ae8a-0e3e-4dcb-bbff-b5a016d74d64] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition _dlc_ExpireDateSaved = new FieldDefinition
        {
            Title = "Original Expiration Date",
            InternalName = "_dlc_ExpireDateSaved",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("74e6ae8a-0e3e-4dcb-bbff-b5a016d74d64"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contact E-Mail Address], ID [c79dba91-e60b-400e-973d-c6d06f192720] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingContactEmail = new FieldDefinition
        {
            Title = "Contact E-Mail Address",
            InternalName = "PublishingContactEmail",
            Description = "Contact E-mail Address is a site column created by the Publishing feature. It is used on the Page Content Type as the e-mail address of the person or group who is the contact person for the page.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c79dba91-e60b-400e-973d-c6d06f192720"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Variation Relationship Link], ID [766da693-38e5-4b1b-997f-e830b6dfcc7b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingVariationRelationshipLinkFieldID = new FieldDefinition
        {
            Title = "Variation Relationship Link",
            InternalName = "PublishingVariationRelationshipLinkFieldID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("766da693-38e5-4b1b-997f-e830b6dfcc7b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Automatic Update], ID [e977ed93-da24-4fcc-b77d-ac34eea7288f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AutomaticUpdate = new FieldDefinition
        {
            Title = "Automatic Update",
            InternalName = "AutomaticUpdate",
            Description = "If this option is selected, the content of this list item will be inserted into web pages as a read-only reference. New versions of this item will automatically appear in the web pages. If the option is not selected, the content of this list item will be inserted into web pages as a copy that page authors can then modify. New versions of this item will not appear in the web pages. Any change to this setting will not affect existing web pages that are using this item.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("e977ed93-da24-4fcc-b77d-ac34eea7288f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Migrated GUID], ID [75bed596-0661-4edd-9724-1d607ab8d3b5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _PublishingMigratedGuid = new FieldDefinition
        {
            Title = "Migrated GUID",
            InternalName = "_PublishingMigratedGuid",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("75bed596-0661-4edd-9724-1d607ab8d3b5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Content Category], ID [3a4b7f98-8d14-4800-8bf5-9ad1dd6a82ee] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ContentCategory = new FieldDefinition
        {
            Title = "Content Category",
            InternalName = "ContentCategory",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("3a4b7f98-8d14-4800-8bf5-9ad1dd6a82ee"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Show in drop-down menu], ID [32e03f99-6949-466a-a4a6-057c21d4b516] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShowInRibbon = new FieldDefinition
        {
            Title = "Show in drop-down menu",
            InternalName = "ShowInRibbon",
            Description = "Select this option if you want this reusable content item to appear in a drop-down menu available during page editing. This will offer authors a quick way to add this item to a page.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("32e03f99-6949-466a-a4a6-057c21d4b516"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Perform ACL Check], ID [db03cb99-cf1e-40b8-adc7-913f7181dac3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCachePerformACLCheck = new FieldDefinition
        {
            Title = "Perform ACL Check",
            InternalName = "PublishingCachePerformACLCheck",
            Description = "Selecting this check box ensures that all items in the cache are appropriately security trimmed. Clearing this check box allows for better performance but should only be applied to sites or page layouts that do not have information that needs security trimming.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("db03cb99-cf1e-40b8-adc7-913f7181dac3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Rating (0-5)], ID [5a14d1ab-1513-48c7-97b3-657a5ba6c742] and Group: [Content Feedback]'
        /// </summary>
        public static FieldDefinition AverageRating = new FieldDefinition
        {
            Title = "Rating (0-5)",
            InternalName = "AverageRating",
            Description = "Average value of all the ratings that have been submitted",
            Group = "Content Feedback",
            FieldType = BuiltInFieldTypes.AverageRating,
            Id = new Guid("5a14d1ab-1513-48c7-97b3-657a5ba6c742"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Cacheability], ID [18f165be-6285-4a57-b3ab-4e9f913d299f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheability = new FieldDefinition
        {
            Title = "Cacheability",
            InternalName = "PublishingCacheability",
            Description = "As specified by HttpCacheability in ASP.Net 2.0.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("18f165be-6285-4a57-b3ab-4e9f913d299f"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reusable HTML], ID [82dd22bf-433e-4260-b26e-5b8360dd9105] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReusableHtml = new FieldDefinition
        {
            Title = "Reusable HTML",
            InternalName = "ReusableHtml",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.HTML,
            Id = new Guid("82dd22bf-433e-4260-b26e-5b8360dd9105"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Associated Content Type], ID [b510aac1-bba3-4652-ab70-2d756c29540f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingAssociatedContentType = new FieldDefinition
        {
            Title = "Associated Content Type",
            InternalName = "PublishingAssociatedContentType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.ContentTypeIdFieldType,
            Id = new Guid("b510aac1-bba3-4652-ab70-2d756c29540f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Duration], ID [bdd1b3c3-18db-4acf-a963-e70ef4227fbc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingCacheDuration = new FieldDefinition
        {
            Title = "Duration",
            InternalName = "PublishingCacheDuration",
            Description = "Duration in seconds to keep the cached version available.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("bdd1b3c3-18db-4acf-a963-e70ef4227fbc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Modified], ID [28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Modified = new FieldDefinition
        {
            Title = "Modified",
            InternalName = "Modified",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hold and Record Status], ID [3afcc5c7-c6ef-44f8-9479-3561d72f9e8e] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition _vti_ItemHoldRecordStatus = new FieldDefinition
        {
            Title = "Hold and Record Status",
            InternalName = "_vti_ItemHoldRecordStatus",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("3afcc5c7-c6ef-44f8-9479-3561d72f9e8e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Byline], ID [d3429cc9-adc4-439b-84a8-5679070f84cb] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition ArticleByLine = new FieldDefinition
        {
            Title = "Byline",
            InternalName = "ArticleByLine",
            Description = "Byline is a site column created by the Publishing feature. It is used on the Article Page Content Type as the byline of the page.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d3429cc9-adc4-439b-84a8-5679070f84cb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Created], ID [8c06beca-0777-48f7-91c7-6da68bc07b69] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Created = new FieldDefinition
        {
            Title = "Created",
            InternalName = "Created",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("8c06beca-0777-48f7-91c7-6da68bc07b69"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Rollup Image], ID [543bc2cf-1f30-488e-8f25-6fe3b689d9ac] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition PublishingRollupImage = new FieldDefinition
        {
            Title = "Rollup Image",
            InternalName = "PublishingRollupImage",
            Description = "Rollup Image is a site column created by the Publishing feature. It is used on the Page Content Type as the image for the page shown in content roll-ups such as the Content By Search web part.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.Image,
            Id = new Guid("543bc2cf-1f30-488e-8f25-6fe3b689d9ac"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [IconOverlay], ID [b77cdbcf-5dce-4937-85a7-9fc202705c91] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IconOverlay = new FieldDefinition
        {
            Title = "IconOverlay",
            InternalName = "IconOverlay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("b77cdbcf-5dce-4937-85a7-9fc202705c91"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Modified By], ID [d31655d1-1d5b-4511-95a1-7a09e9b75bf2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Editor = new FieldDefinition
        {
            Title = "Modified By",
            InternalName = "Editor",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("d31655d1-1d5b-4511-95a1-7a09e9b75bf2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contact], ID [aea1a4dd-0f19-417d-8721-95a1d28762ab] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition PublishingContact = new FieldDefinition
        {
            Title = "Contact",
            InternalName = "PublishingContact",
            Description = "Contact is a site column created by the Publishing feature. It is used on the Page Content Type as the person or group who is the contact person for the page.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("aea1a4dd-0f19-417d-8721-95a1d28762ab"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Expiration Date], ID [acd16fdf-052f-40f7-bb7e-564c269c9fbc] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition _dlc_ExpireDate = new FieldDefinition
        {
            Title = "Expiration Date",
            InternalName = "_dlc_ExpireDate",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("acd16fdf-052f-40f7-bb7e-564c269c9fbc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Locked For Edit], ID [740931e6-d79e-44a6-a752-a06eb23c11b0] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition _vti_ItemIsLocked = new FieldDefinition
        {
            Title = "Is Locked For Edit",
            InternalName = "_vti_ItemIsLocked",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("740931e6-d79e-44a6-a752-a06eb23c11b0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Safe for Authenticated Use], ID [0a90b5e8-185a-4dec-bf3c-e60aae08373f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingAuthenticatedUse = new FieldDefinition
        {
            Title = "Safe for Authenticated Use",
            InternalName = "PublishingAuthenticatedUse",
            Description = "This check box should be selected for only those policies that you want to allow to be applied to authenticated scenarios by administrators and page layout designers.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("0a90b5e8-185a-4dec-bf3c-e60aae08373f"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Image Caption], ID [66f500e9-7955-49ab-abb1-663621727d10] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition PublishingImageCaption = new FieldDefinition
        {
            Title = "Image Caption",
            InternalName = "PublishingImageCaption",
            Description = "Image Caption is a site column created by the Publishing feature. It is used on the Article Page Content Type as the caption for the primary image displayed on the page.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.HTML,
            Id = new Guid("66f500e9-7955-49ab-abb1-663621727d10"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Article Date], ID [71316cea-40a0-49f3-8659-f0cefdbdbd4f] and Group: [Publishing Columns]'
        /// </summary>
        public static FieldDefinition ArticleStartDate = new FieldDefinition
        {
            Title = "Article Date",
            InternalName = "ArticleStartDate",
            Description = "Article Date is a site column created by the Publishing feature. It is used on the Article Page Content Type as the date of the page.",
            Group = "Publishing Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("71316cea-40a0-49f3-8659-f0cefdbdbd4f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Vary by HTTP Header], ID [89587dfd-b9ca-4fae-8eb9-ba779e917d48] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishingVaryByHeader = new FieldDefinition
        {
            Title = "Vary by HTTP Header",
            InternalName = "PublishingVaryByHeader",
            Description = "As specified by HttpCachePolicy.VaryByHeaders in ASP.Net 2.0.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("89587dfd-b9ca-4fae-8eb9-ba779e917d48"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Summary Links], ID [b3525efe-59b5-4f0f-b1e4-6e26cb6ef6aa] and Group: [Page Layout Columns]'
        /// </summary>
        public static FieldDefinition SummaryLinks = new FieldDefinition
        {
            Title = "Summary Links",
            InternalName = "SummaryLinks",
            Description = "Summary Links is a site column created by the Publishing feature. It is used on the Welcome Page Content Type to display a set of links.",
            Group = "Page Layout Columns",
            FieldType = BuiltInFieldTypes.SummaryLinks,
            Id = new Guid("b3525efe-59b5-4f0f-b1e4-6e26cb6ef6aa"),
            Required = false
        };




        #endregion
    }



}
