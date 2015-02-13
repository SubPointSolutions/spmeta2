using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint definitions.
    /// </summary>
    public static class BuiltInFieldDefinitions
    {
        #region properties

        /// <summary>
        /// Corresponds to built-in field with Title [Lower values are better], ID [56747800-d36e-4625-abe3-b1bc74a7d5f8] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition LowerValuesAreBetter = new FieldDefinition
        {
            Title = "Lower values are better",
            InternalName = "LowerValuesAreBetter",
            Description = "Whether lower is better or higher is better",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("56747800-d36e-4625-abe3-b1bc74a7d5f8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comments Lookup], ID [672d9500-5649-49ae-8166-777f40527874] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AbuseReportsCommentsLookup = new FieldDefinition
        {
            Title = "Comments Lookup",
            InternalName = "AbuseReportsCommentsLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("672d9500-5649-49ae-8166-777f40527874"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Address Country/Region], ID [3c0e9e00-8fcc-479f-9d8d-3447cda34c5b] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherAddressCountry = new FieldDefinition
        {
            Title = "Other Address Country/Region",
            InternalName = "OtherAddressCountry",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3c0e9e00-8fcc-479f-9d8d-3447cda34c5b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Instance Id], ID [1f30d200-0d4e-4c8a-a7eb-2e49815bf2be] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WF4InstanceId = new FieldDefinition
        {
            Title = "Instance Id",
            InternalName = "WF4InstanceId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("1f30d200-0d4e-4c8a-a7eb-2e49815bf2be"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Categories], ID [9ebcd900-9d05-46c8-8f4d-e46e87328844] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Categories = new FieldDefinition
        {
            Title = "Categories",
            InternalName = "Categories",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9ebcd900-9d05-46c8-8f4d-e46e87328844"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Enterprise Keywords], ID [23f27201-bee3-471e-b2e7-b64fd8b7ca38] and Group: [Enterprise Keywords Group]'
        /// </summary>
        public static FieldDefinition TaxKeyword = new FieldDefinition
        {
            Title = "Enterprise Keywords",
            InternalName = "TaxKeyword",
            Description = "Enterprise Keywords are shared with other users and applications to allow for ease of search and filtering, as well as metadata consistency and reuse",
            Group = "Enterprise Keywords Group",
            FieldType = BuiltInFieldTypes.TaxonomyFieldTypeMulti,
            Id = new Guid("23f27201-bee3-471e-b2e7-b64fd8b7ca38"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Manager's Name], ID [ba934502-d68d-4960-a54b-51e15fef5fd3] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ManagersName = new FieldDefinition
        {
            Title = "Manager's Name",
            InternalName = "ManagersName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ba934502-d68d-4960-a54b-51e15fef5fd3"),
            Required = false
        };

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
        /// Corresponds to built-in field with Title [Children's Names], ID [6440b402-8ec5-4d7a-83f4-afccb556b5cc] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ChildrensNames = new FieldDefinition
        {
            Title = "Children's Names",
            InternalName = "ChildrensNames",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("6440b402-8ec5-4d7a-83f4-afccb556b5cc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date Modified], ID [810dbd02-bbf5-4c67-b1ce-5ad7c5a512b2] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _DCDateModified = new FieldDefinition
        {
            Title = "Date Modified",
            InternalName = "_DCDateModified",
            Description = "The date on which this resource was last modified",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("810dbd02-bbf5-4c67-b1ce-5ad7c5a512b2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Company], ID [038d1503-4629-40f6-adaf-b47d1ab2d4fe] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Company = new FieldDefinition
        {
            Title = "Company",
            InternalName = "Company",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("038d1503-4629-40f6-adaf-b47d1ab2d4fe"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Event Cancelled], ID [b8bbe503-bb22-4237-8d9e-0587756a2176] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EventCanceled = new FieldDefinition
        {
            Title = "Event Cancelled",
            InternalName = "EventCanceled",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("b8bbe503-bb22-4237-8d9e-0587756a2176"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [UID], ID [63055d04-01b5-48f3-9e1e-e564e7c6b23b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition UID = new FieldDefinition
        {
            Title = "UID",
            InternalName = "UID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("63055d04-01b5-48f3-9e1e-e564e7c6b23b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home Address State Or Province], ID [f5b36006-69b0-418c-bd4a-f25ca7e096bb] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomeAddressStateOrProvince = new FieldDefinition
        {
            Title = "Home Address State Or Province",
            InternalName = "HomeAddressStateOrProvince",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f5b36006-69b0-418c-bd4a-f25ca7e096bb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date/Time], ID [63fc6806-db53-4d0d-b18b-eaf90e96ddf5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition CallTime = new FieldDefinition
        {
            Title = "Date/Time",
            InternalName = "CallTime",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("63fc6806-db53-4d0d-b18b-eaf90e96ddf5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Link to Report], ID [851c7906-3c95-46bc-a81e-30588602d910] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportLink = new FieldDefinition
        {
            Title = "Link to Report",
            InternalName = "ReportLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("851c7906-3c95-46bc-a81e-30588602d910"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Break], ID [9b12fb06-254e-43b3-bfc8-8eea422ebc9f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Break = new FieldDefinition
        {
            Title = "Break",
            InternalName = "Break",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9b12fb06-254e-43b3-bfc8-8eea422ebc9f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author], ID [246d0907-637c-46b7-9aa0-0bb914daa832] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Author = new FieldDefinition
        {
            Title = "Author",
            InternalName = "_Author",
            Description = "The primary author",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("246d0907-637c-46b7-9aa0-0bb914daa832"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Late Night], ID [aaa68c08-6276-4337-9bce-b9cd852c7328] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NightWork = new FieldDefinition
        {
            Title = "Late Night",
            InternalName = "NightWork",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("aaa68c08-6276-4337-9bce-b9cd852c7328"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [End], ID [04b29608-b1e8-4ff9-90d5-5328096dd5ac] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition End = new FieldDefinition
        {
            Title = "End",
            InternalName = "End",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("04b29608-b1e8-4ff9-90d5-5328096dd5ac"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Out of Office(Private)], ID [63c1c608-df6f-4cfa-bcab-fdbf9c223e31] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Oof = new FieldDefinition
        {
            Title = "Out of Office(Private)",
            InternalName = "Oof",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("63c1c608-df6f-4cfa-bcab-fdbf9c223e31"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Previously Assigned To], ID [1982e408-0f94-4149-8349-16f301d89134] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PreviouslyAssignedTo = new FieldDefinition
        {
            Title = "Previously Assigned To",
            InternalName = "PreviouslyAssignedTo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("1982e408-0f94-4149-8349-16f301d89134"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [34a72e09-3ca6-4931-b2e3-f81c40bb87bd] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingRuleDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "RoutingRuleDescription",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("34a72e09-3ca6-4931-b2e3-f81c40bb87bd"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Display Name], ID [5263cd09-a770-4549-b012-d9f3df3d8df6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowDisplayName = new FieldDefinition
        {
            Title = "Workflow Display Name",
            InternalName = "WorkflowDisplayName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("5263cd09-a770-4549-b012-d9f3df3d8df6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Checked out User], ID [3881510a-4e4a-4ee8-b102-8ee8e2d0dd4b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition CheckoutUser = new FieldDefinition
        {
            Title = "Checked out User",
            InternalName = "CheckoutUser",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("3881510a-4e4a-4ee8-b102-8ee8e2d0dd4b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Route To External Location], ID [a2455e0a-f63c-46af-857c-dbd4199ff95f] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingRuleExternal = new FieldDefinition
        {
            Title = "Route To External Location",
            InternalName = "RoutingRuleExternal",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("a2455e0a-f63c-46af-857c-dbd4199ff95f"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Formatted indicator warning], ID [f53d350d-854e-4962-9318-89d56d30773a] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition FormattedWarning = new FieldDefinition
        {
            Title = "Formatted indicator warning",
            InternalName = "FormattedWarning",
            Description = "The warning value of the indicator, formatted by its datasource provider",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f53d350d-854e-4962-9318-89d56d30773a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Toggle Quoted Text], ID [e451420d-4e62-43e3-af83-010d36e353a2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ToggleQuotedText = new FieldDefinition
        {
            Title = "Toggle Quoted Text",
            InternalName = "ToggleQuotedText",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("e451420d-4e62-43e3-af83-010d36e353a2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Aggregated Rating Count], ID [5feb760d-e1c5-42d7-92ac-26ae20a1365a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DescendantRatingsCount = new FieldDefinition
        {
            Title = "Aggregated Rating Count",
            InternalName = "DescendantRatingsCount",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("5feb760d-e1c5-42d7-92ac-26ae20a1365a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Primary Phone], ID [d69bcc0e-57c3-4f3b-bbc5-b090edf21f0f] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition PrimaryNumber = new FieldDefinition
        {
            Title = "Primary Phone",
            InternalName = "PrimaryNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d69bcc0e-57c3-4f3b-bbc5-b090edf21f0f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Holiday], ID [b5a7350f-2716-46ca-9c42-66bb39d042ec] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HolidayWork = new FieldDefinition
        {
            Title = "Holiday",
            InternalName = "HolidayWork",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("b5a7350f-2716-46ca-9c42-66bb39d042ec"),
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
        /// Corresponds to built-in field with Title [Target Folder], ID [7383b80f-b38d-4dde-b9e0-5319f0777069] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingTargetFolder = new FieldDefinition
        {
            Title = "Target Folder",
            InternalName = "RoutingTargetFolder",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("7383b80f-b38d-4dde-b9e0-5319f0777069"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Full Name], ID [475c2610-c157-4b91-9e2d-6855031b3538] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition FullName = new FieldDefinition
        {
            Title = "Full Name",
            InternalName = "FullName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("475c2610-c157-4b91-9e2d-6855031b3538"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [3f155110-a6a2-4d70-926c-94648101f0e8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Description = new FieldDefinition
        {
            Title = "Description",
            InternalName = "Description",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("3f155110-a6a2-4d70-926c-94648101f0e8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [File Type], ID [39360f11-34cf-4356-9945-25c44e68dade] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition File_x0020_Type = new FieldDefinition
        {
            Title = "File Type",
            InternalName = "File_x0020_Type",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("39360f11-34cf-4356-9945-25c44e68dade"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Auto folder configuration data], ID [e1fa3211-0188-4a95-a737-8775782cbac0] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingAutoFolderSettings = new FieldDefinition
        {
            Title = "Auto folder configuration data",
            InternalName = "RoutingAutoFolderSettings",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("e1fa3211-0188-4a95-a737-8775782cbac0"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Icon URL], ID [3dfb3e11-9ccd-4404-b44a-a71f6399ea56] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XSLStyleIconUrl = new FieldDefinition
        {
            Title = "Icon URL",
            InternalName = "XSLStyleIconUrl",
            Description = "A 64x48 icon that represents this style",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("3dfb3e11-9ccd-4404-b44a-a71f6399ea56"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Description], ID [2a16b911-b094-46e6-a7cd-227eea3effdb] and Group: [Reports]'
        /// </summary>
        public static FieldDefinition ReportDescription = new FieldDefinition
        {
            Title = "Report Description",
            InternalName = "ReportDescription",
            Description = "A description of the contents of the report",
            Group = "Reports",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("2a16b911-b094-46e6-a7cd-227eea3effdb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [ID], ID [1d22ea11-1e32-424e-89ab-9fedbadb6ce1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ID = new FieldDefinition
        {
            Title = "ID",
            InternalName = "ID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Counter,
            Id = new Guid("1d22ea11-1e32-424e-89ab-9fedbadb6ce1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Enabled], ID [7b2b1712-a73d-4ad7-a9d0-662f0291713d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleCheckEnabled = new FieldDefinition
        {
            Title = "Enabled",
            InternalName = "HealthRuleCheckEnabled",
            Description = "If selected, SharePoint will periodically look for this problem.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("7b2b1712-a73d-4ad7-a9d0-662f0291713d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Mobile Content], ID [53a2a512-d395-4852-8714-d4c27e7585f3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MobileContent = new FieldDefinition
        {
            Title = "Mobile Content",
            InternalName = "MobileContent",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("53a2a512-d395-4852-8714-d4c27e7585f3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [TimeZone], ID [6cc1c612-748a-48d8-88f2-944f477f301b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TimeZone = new FieldDefinition
        {
            Title = "TimeZone",
            InternalName = "TimeZone",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("6cc1c612-748a-48d8-88f2-944f477f301b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Profession], ID [f0753a13-44b1-4269-82af-5c34c57b0c67] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Profession = new FieldDefinition
        {
            Title = "Profession",
            InternalName = "Profession",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f0753a13-44b1-4269-82af-5c34c57b0c67"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reputation Score], ID [edd35d15-ae36-4b1b-91aa-0e288df6c612] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReputationScore = new FieldDefinition
        {
            Title = "Reputation Score",
            InternalName = "ReputationScore",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("edd35d15-ae36-4b1b-91aa-0e288df6c612"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [URL Path], ID [94f89715-e097-4e8b-ba79-ea02aa8b7adb] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FileRef = new FieldDefinition
        {
            Title = "URL Path",
            InternalName = "FileRef",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("94f89715-e097-4e8b-ba79-ea02aa8b7adb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Base Association Guid], ID [e9359d15-261b-48f6-a302-01419a68d4de] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition BaseAssociationGuid = new FieldDefinition
        {
            Title = "Base Association Guid",
            InternalName = "BaseAssociationGuid",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e9359d15-261b-48f6-a302-01419a68d4de"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Organizational ID Number], ID [0850ae15-19dd-431f-9c2f-3aff3ae292ce] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OrganizationalIDNumber = new FieldDefinition
        {
            Title = "Organizational ID Number",
            InternalName = "OrganizationalIDNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("0850ae15-19dd-431f-9c2f-3aff3ae292ce"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Threading], ID [58ca6516-51cd-41fb-a908-dd2a4aeea8bc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Threading = new FieldDefinition
        {
            Title = "Threading",
            InternalName = "Threading",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("58ca6516-51cd-41fb-a908-dd2a4aeea8bc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comment 2], ID [e2c93917-cf32-4b29-be5c-d71f1bac7714] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IMEComment2 = new FieldDefinition
        {
            Title = "Comment 2",
            InternalName = "IMEComment2",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e2c93917-cf32-4b29-be5c-d71f1bac7714"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Task Outcome], ID [55b29417-1042-47f0-9dff-ce8156667f96] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition TaskOutcome = new FieldDefinition
        {
            Title = "Task Outcome",
            InternalName = "TaskOutcome",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.OutcomeChoice,
            Id = new Guid("55b29417-1042-47f0-9dff-ce8156667f96"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Percent Expression], ID [d43e8a19-f4f3-4e6a-b8c1-02e972c3ed6f] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition PercentExpression = new FieldDefinition
        {
            Title = "Percent Expression",
            InternalName = "PercentExpression",
            Description = "Determine whether the expression is used to calculate percentage",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("d43e8a19-f4f3-4e6a-b8c1-02e972c3ed6f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Compatible Managed Properties], ID [bab0a619-d1ec-40d7-847b-3e4408080c17] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition CompatibleManagedProperties = new FieldDefinition
        {
            Title = "Compatible Managed Properties",
            InternalName = "CompatibleManagedProperties",
            Description = "Enter the names of the managed properties that you would like to use this Filter Display Template with. The managed properties with names that start with the values you enter will be able to use this Display Template. ",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("bab0a619-d1ec-40d7-847b-3e4408080c17"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [249a1c1a-5a3e-4173-abad-779b01892510] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition KpiDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "KpiDescription",
            Description = "The description provides information about the purpose of the goal.",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("249a1c1a-5a3e-4173-abad-779b01892510"),
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
        /// Corresponds to built-in field with Title [Content], ID [7650d41a-fa26-4c72-a641-af4e93dc7053] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Content = new FieldDefinition
        {
            Title = "Content",
            InternalName = "Content",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("7650d41a-fa26-4c72-a641-af4e93dc7053"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comment 3], ID [7c52f61a-e1e0-4341-9e2f-9b36cddfdd7c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IMEComment3 = new FieldDefinition
        {
            Title = "Comment 3",
            InternalName = "IMEComment3",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("7c52f61a-e1e0-4341-9e2f-9b36cddfdd7c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Recipients], ID [7111aa1b-e7ae-4b69-acaf-db669b76e03a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition V4CallTo = new FieldDefinition
        {
            Title = "Recipients",
            InternalName = "V4CallTo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.CallTo,
            Id = new Guid("7111aa1b-e7ae-4b69-acaf-db669b76e03a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Level], ID [43bdd51b-3c5b-4e78-90a8-fb2087f71e70] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _Level = new FieldDefinition
        {
            Title = "Level",
            InternalName = "_Level",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("43bdd51b-3c5b-4e78-90a8-fb2087f71e70"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Non-Working Day], ID [baf7091c-01fb-4831-a975-08254f87f234] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IsNonWorkingDay = new FieldDefinition
        {
            Title = "Non-Working Day",
            InternalName = "IsNonWorkingDay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("baf7091c-01fb-4831-a975-08254f87f234"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [ProgId], ID [c5c4b81c-f1d9-4b43-a6a2-090df32ebb68] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ProgId = new FieldDefinition
        {
            Title = "ProgId",
            InternalName = "ProgId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("c5c4b81c-f1d9-4b43-a6a2-090df32ebb68"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Priority], ID [d4a6af1d-c6d7-4045-8def-cefa25b9ec31] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingPriority = new FieldDefinition
        {
            Title = "Priority",
            InternalName = "RoutingPriority",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d4a6af1d-c6d7-4045-8def-cefa25b9ec31"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Copyright], ID [f08ab41d-9a03-49ae-9413-6cd284a15625] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition wic_System_Copyright = new FieldDefinition
        {
            Title = "Copyright",
            InternalName = "wic_System_Copyright",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f08ab41d-9a03-49ae-9413-6cd284a15625"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Warning Cell], ID [eeaabf1d-f6ae-4dc6-873f-7397a17c36f0] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition WarningCell = new FieldDefinition
        {
            Title = "Warning Cell",
            InternalName = "WarningCell",
            Description = "Address or name of the cell for the warning",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("eeaabf1d-f6ae-4dc6-873f-7397a17c36f0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Posted By], ID [b4ab471e-0262-462a-8b3f-c1dfc9e2d5fd] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PersonViewMinimal = new FieldDefinition
        {
            Title = "Posted By",
            InternalName = "PersonViewMinimal",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("b4ab471e-0262-462a-8b3f-c1dfc9e2d5fd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Frame Width], ID [59cd571e-e2d9-485d-bb5d-e70d12f8d0b7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoWidthInPixels = new FieldDefinition
        {
            Title = "Frame Width",
            InternalName = "VideoWidthInPixels",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("59cd571e-e2d9-485d-bb5d-e70d12f8d0b7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail To], ID [caa2cb1e-a124-4068-9496-14feef1a901f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailTo = new FieldDefinition
        {
            Title = "E-Mail To",
            InternalName = "EmailTo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("caa2cb1e-a124-4068-9496-14feef1a901f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Outcome], ID [dcde7b1f-918b-4ed5-819f-9798f8abac37] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Outcome = new FieldDefinition
        {
            Title = "Outcome",
            InternalName = "Outcome",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dcde7b1f-918b-4ed5-819f-9798f8abac37"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Template Id], ID [467e811f-0c12-4a93-bb04-42ff0c1c597b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TemplateId = new FieldDefinition
        {
            Title = "Template Id",
            InternalName = "TemplateId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("467e811f-0c12-4a93-bb04-42ff0c1c597b"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Relative Url], ID [467e811f-0c12-4a93-bb04-42ff0c1c597c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormRelativeUrl = new FieldDefinition
        {
            Title = "Form Relative Url",
            InternalName = "FormRelativeUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("467e811f-0c12-4a93-bb04-42ff0c1c597c"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Folder Child Count], ID [960ff01f-2b6d-4f1b-9c3f-e19ad8927341] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FolderChildCount = new FieldDefinition
        {
            Title = "Folder Child Count",
            InternalName = "FolderChildCount",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("960ff01f-2b6d-4f1b-9c3f-e19ad8927341"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User ID], ID [5928ff1f-daa1-406c-b4a9-190485a448cb] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition User = new FieldDefinition
        {
            Title = "User ID",
            InternalName = "User",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("5928ff1f-daa1-406c-b4a9-190485a448cb"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [ScopeId], ID [dddd2420-b270-4735-93b5-92b713d0944d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ScopeId = new FieldDefinition
        {
            Title = "ScopeId",
            InternalName = "ScopeId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("dddd2420-b270-4735-93b5-92b713d0944d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comment 1], ID [d2433b20-3f02-4432-817d-369f104a2dcd] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IMEComment1 = new FieldDefinition
        {
            Title = "Comment 1",
            InternalName = "IMEComment1",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d2433b20-3f02-4432-817d-369f104a2dcd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [App Created By], ID [6bfaba20-36bf-44b5-a1b2-eb6346d49716] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AppAuthor = new FieldDefinition
        {
            Title = "App Created By",
            InternalName = "AppAuthor",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("6bfaba20-36bf-44b5-a1b2-eb6346d49716"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Related Items], ID [1ad7c220-c893-4c15-b95c-b69b992bdee2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RelatedLinks = new FieldDefinition
        {
            Title = "Related Items",
            InternalName = "RelatedLinks",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("1ad7c220-c893-4c15-b95c-b69b992bdee2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Call Back], ID [274b7e21-284a-4c49-bec6-f1f2cb6fc344] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition CallBack = new FieldDefinition
        {
            Title = "Call Back",
            InternalName = "CallBack",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("274b7e21-284a-4c49-bec6-f1f2cb6fc344"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thumbnail Exists], ID [1f43cd21-53c5-44c5-8675-b8bb86083244] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ThumbnailExists = new FieldDefinition
        {
            Title = "Thumbnail Exists",
            InternalName = "ThumbnailExists",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("1f43cd21-53c5-44c5-8675-b8bb86083244"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form_URN], ID [17ca3a22-fdfe-46eb-99b5-9646baed3f16] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormURN = new FieldDefinition
        {
            Title = "Form_URN",
            InternalName = "FormURN",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("17ca3a22-fdfe-46eb-99b5-9646baed3f16"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Managed Property Mappings], ID [a0dd6c22-0988-453e-b3e2-77479dc9f014] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition ManagedPropertyMapping = new FieldDefinition
        {
            Title = "Managed Property Mappings",
            InternalName = "ManagedPropertyMapping",
            Description = "Enter the slots and the managed properties that map to the slots. This field will be used to determine which managed properties are retrieved from SharePoint Search when you are using this Display Template. Use the format \"slot name\":\"property name\", separated by commas.",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("a0dd6c22-0988-453e-b3e2-77479dc9f014"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indicator Value], ID [f0816223-fd98-41f9-aa57-b7f7db462faa] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition Value = new FieldDefinition
        {
            Title = "Indicator Value",
            InternalName = "Value",
            Description = "",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("f0816223-fd98-41f9-aa57-b7f7db462faa"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Parent Name], ID [28081524-7c2f-4f08-9319-9c737b495bc1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ParentName = new FieldDefinition
        {
            Title = "Report Parent Name",
            InternalName = "ParentName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("28081524-7c2f-4f08-9319-9c737b495bc1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date Picture Taken], ID [a5d2f824-bc53-422e-87fd-765939d863a5] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition ImageCreateDate = new FieldDefinition
        {
            Title = "Date Picture Taken",
            InternalName = "ImageCreateDate",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("a5d2f824-bc53-422e-87fd-765939d863a5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target List Template ID], ID [9f927425-78e9-49c3-b03b-65e1211394e1] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSTargetListTemplate = new FieldDefinition
        {
            Title = "Target List Template ID",
            InternalName = "DisplayTemplateJSTargetListTemplate",
            Description = "ID of the list template type this override applies to.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9f927425-78e9-49c3-b03b-65e1211394e1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [GUID], ID [ae069f25-3ac2-4256-b9c3-15dbc15da0e0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GUID = new FieldDefinition
        {
            Title = "GUID",
            InternalName = "GUID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("ae069f25-3ac2-4256-b9c3-15dbc15da0e0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Birthday], ID [c4c7d925-bc1b-4f37-826d-ac49b4fb1bc1] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Birthday = new FieldDefinition
        {
            Title = "Birthday",
            InternalName = "Birthday",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("c4c7d925-bc1b-4f37-826d-ac49b4fb1bc1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Document Created By], ID [4dd7e525-8d6b-4cb4-9d3e-44ee25f973eb] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Created_x0020_By = new FieldDefinition
        {
            Title = "Document Created By",
            InternalName = "Created_x0020_By",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4dd7e525-8d6b-4cb4-9d3e-44ee25f973eb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Web Preview], ID [bd716b26-546d-43f2-b229-62699581fa9f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Preview = new FieldDefinition
        {
            Title = "Web Preview",
            InternalName = "Preview",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("bd716b26-546d-43f2-b229-62699581fa9f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [9d30f126-ba48-446b-b8f9-83745f322ebe] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkFilenameNoMenu = new FieldDefinition
        {
            Title = "Name",
            InternalName = "LinkFilenameNoMenu",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("9d30f126-ba48-446b-b8f9-83745f322ebe"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Goal from workbook], ID [20906227-d1c8-430c-989a-30a62e3e40b2] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition GoalFromWorkBook = new FieldDefinition
        {
            Title = "Goal from workbook",
            InternalName = "GoalFromWorkBook",
            Description = "Whether the goal comes from the workbook or from the indicator definition",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("20906227-d1c8-430c-989a-30a62e3e40b2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Related Issues], ID [875fab27-6e95-463b-a4a6-82544f1027fb] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition RelatedIssues = new FieldDefinition
        {
            Title = "Related Issues",
            InternalName = "RelatedIssues",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("875fab27-6e95-463b-a4a6-82544f1027fb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Business Phone], ID [fd630629-c165-4513-b43c-fdb16b86a14d] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkPhone = new FieldDefinition
        {
            Title = "Business Phone",
            InternalName = "WorkPhone",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fd630629-c165-4513-b43c-fdb16b86a14d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Subject], ID [76a81629-44d4-4ce1-8d4d-6d7ebcd885fc] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition Subject = new FieldDefinition
        {
            Title = "Subject",
            InternalName = "Subject",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("76a81629-44d4-4ce1-8d4d-6d7ebcd885fc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Middle Name], ID [418c8d29-6f2e-44c3-8955-2cd7ec3e2151] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition MiddleName = new FieldDefinition
        {
            Title = "Middle Name",
            InternalName = "MiddleName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("418c8d29-6f2e-44c3-8955-2cd7ec3e2151"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [ISDN], ID [a579062a-6c1d-4ad3-9d5e-035f9f2c1882] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ISDNNumber = new FieldDefinition
        {
            Title = "ISDN",
            InternalName = "ISDNNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("a579062a-6c1d-4ad3-9d5e-035f9f2c1882"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thumbnail Preview], ID [9941082a-4160-46a1-a5b2-03394bfdf7ee] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ThumbnailOnForm = new FieldDefinition
        {
            Title = "Thumbnail Preview",
            InternalName = "ThumbnailOnForm",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("9941082a-4160-46a1-a5b2-03394bfdf7ee"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Version], ID [dce8262a-3ae9-45aa-aab4-83bd75fb738a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _UIVersionString = new FieldDefinition
        {
            Title = "Version",
            InternalName = "_UIVersionString",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dce8262a-3ae9-45aa-aab4-83bd75fb738a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Created By], ID [efca5f2b-de72-42a8-aefd-1257af8698a8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportCreatedBy = new FieldDefinition
        {
            Title = "Report Created By",
            InternalName = "ReportCreatedBy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("efca5f2b-de72-42a8-aefd-1257af8698a8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Number of Likes], ID [6e4d832b-f610-41a8-b3e0-239608efda41] and Group: [Content Feedback]'
        /// </summary>
        public static FieldDefinition LikesCount = new FieldDefinition
        {
            Title = "Number of Likes",
            InternalName = "LikesCount",
            Description = "",
            Group = "Content Feedback",
            FieldType = BuiltInFieldTypes.Likes,
            Id = new Guid("6e4d832b-f610-41a8-b3e0-239608efda41"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Calendar Sequence], ID [7a0cb12b-c70c-4f99-99f1-a232783a87d7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailCalendarSequence = new FieldDefinition
        {
            Title = "E-Mail Calendar Sequence",
            InternalName = "EmailCalendarSequence",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("7a0cb12b-c70c-4f99-99f1-a232783a87d7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indicator Comments], ID [0121cb2b-4515-44f2-9d5a-0dcb3bf556aa] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition KpiComments = new FieldDefinition
        {
            Title = "Indicator Comments",
            InternalName = "KpiComments",
            Description = "Comments describe the current status of the indicator, and may provide information about causes or problems.",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("0121cb2b-4515-44f2-9d5a-0dcb3bf556aa"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Detail Link], ID [9730102c-9470-4515-960e-6dfb2d89a68b] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition DetailLink = new FieldDefinition
        {
            Title = "Detail Link",
            InternalName = "DetailLink",
            Description = "Link for page for clicking through for details ",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("9730102c-9470-4515-960e-6dfb2d89a68b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Confirmed To], ID [1b89212c-1c67-487a-8c14-4d30bf4ef223] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ConfirmedTo = new FieldDefinition
        {
            Title = "Confirmed To",
            InternalName = "ConfirmedTo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("1b89212c-1c67-487a-8c14-4d30bf4ef223"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Member Status], ID [e236652c-cf8f-4917-8baa-30ffcccfb7e8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MemberStatusInt = new FieldDefinition
        {
            Title = "Member Status",
            InternalName = "MemberStatusInt",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("e236652c-cf8f-4917-8baa-30ffcccfb7e8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Anniversary], ID [9d76802c-13c4-484a-9872-d7f9641c4672] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Anniversary = new FieldDefinition
        {
            Title = "Anniversary",
            InternalName = "Anniversary",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("9d76802c-13c4-484a-9872-d7f9641c4672"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Body], ID [7662cd2c-f069-4dba-9e35-082cf976e170] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Body = new FieldDefinition
        {
            Title = "Body",
            InternalName = "Body",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("7662cd2c-f069-4dba-9e35-082cf976e170"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Account], ID [bfc6f32c-668c-43c4-a903-847cca2f9b3c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Name = new FieldDefinition
        {
            Title = "Account",
            InternalName = "Name",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("bfc6f32c-668c-43c4-a903-847cca2f9b3c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Data Source], ID [a9ea6e2d-bc5c-4ccc-bad8-8ddfe519710f] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition DataSource = new FieldDefinition
        {
            Title = "Data Source",
            InternalName = "DataSource",
            Description = "URL to the data source",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("a9ea6e2d-bc5c-4ccc-bad8-8ddfe519710f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Signed], ID [fbf29b2d-cae5-49aa-8e0a-29955b540122] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition xd_Signature = new FieldDefinition
        {
            Title = "Is Signed",
            InternalName = "xd_Signature",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("fbf29b2d-cae5-49aa-8e0a-29955b540122"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home from Offsite], ID [2ead592e-f05c-41a2-9817-e06dac25bc19] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GoingHome = new FieldDefinition
        {
            Title = "Home from Offsite",
            InternalName = "GoingHome",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("2ead592e-f05c-41a2-9817-e06dac25bc19"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Bit Rate], ID [cf42542f-df94-4136-a0ac-29326fccd565] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoRenditionBitRate = new FieldDefinition
        {
            Title = "Bit Rate",
            InternalName = "VideoRenditionBitRate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("cf42542f-df94-4136-a0ac-29326fccd565"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [External Participant], ID [16b6952f-3ce6-45e0-8f4e-42dac6e12441] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition OffsiteParticipant = new FieldDefinition
        {
            Title = "External Participant",
            InternalName = "OffsiteParticipant",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("16b6952f-3ce6-45e0-8f4e-42dac6e12441"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Show Combine View], ID [086f2b30-460c-4251-b75a-da88a5b205c1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShowCombineView = new FieldDefinition
        {
            Title = "Show Combine View",
            InternalName = "ShowCombineView",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("086f2b30-460c-4251-b75a-da88a5b205c1"),
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
        /// Corresponds to built-in field with Title [Location], ID [288f5f32-8462-4175-8f09-dd7ba29359a9] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Location = new FieldDefinition
        {
            Title = "Location",
            InternalName = "Location",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("288f5f32-8462-4175-8f09-dd7ba29359a9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Assistant's Phone], ID [f55de332-074e-4e71-a71a-b90abfad51ae] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition AssistantNumber = new FieldDefinition
        {
            Title = "Assistant's Phone",
            InternalName = "AssistantNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f55de332-074e-4e71-a71a-b90abfad51ae"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Modified By], ID [64016533-26ca-4ae6-8e1f-7cc34687e416] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportModifiedByDisplay = new FieldDefinition
        {
            Title = "Report Modified By",
            InternalName = "ReportModifiedByDisplay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("64016533-26ca-4ae6-8e1f-7cc34687e416"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date Occurred], ID [5602dc33-a60a-4dec-bd23-d18dfcef861d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Occurred = new FieldDefinition
        {
            Title = "Date Occurred",
            InternalName = "Occurred",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("5602dc33-a60a-4dec-bd23-d18dfcef861d"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Picture Height], ID [1944c034-d61b-42af-aa84-647f2e74ca70] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition ImageHeight = new FieldDefinition
        {
            Title = "Picture Height",
            InternalName = "ImageHeight",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("1944c034-d61b-42af-aa84-647f2e74ca70"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Save to report history], ID [90884f35-d2a5-48dc-a39f-7bcbc9781cf6] and Group: [Reports]'
        /// </summary>
        public static FieldDefinition SaveToReportHistory = new FieldDefinition
        {
            Title = "Save to report history",
            InternalName = "SaveToReportHistory",
            Description = "Every time this document is saved a copy will be added to the report history.",
            Group = "Reports",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("90884f35-d2a5-48dc-a39f-7bcbc9781cf6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Trend], ID [11a86235-9d18-4134-b58c-fa7243f4cbba] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition Trend = new FieldDefinition
        {
            Title = "Trend",
            InternalName = "Trend",
            Description = "The trend of the Indicator",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("11a86235-9d18-4134-b58c-fa7243f4cbba"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indicator Status], ID [3c497036-038f-41db-aec7-c9849649b135] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition KpiStatus = new FieldDefinition
        {
            Title = "Indicator Status",
            InternalName = "KpiStatus",
            Description = "The status of the Indicator",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("3c497036-038f-41db-aec7-c9849649b135"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Mark-up HREF], ID [566da236-762b-4a76-ad1f-b08b3c703fce] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XomlUrl = new FieldDefinition
        {
            Title = "Workflow Mark-up HREF",
            InternalName = "XomlUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("566da236-762b-4a76-ad1f-b08b3c703fce"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Title], ID [bc91a437-52e7-49e1-8c4e-4698904b2b6d] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkTitleNoMenu = new FieldDefinition
        {
            Title = "Title",
            InternalName = "LinkTitleNoMenu",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("bc91a437-52e7-49e1-8c4e-4698904b2b6d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Completed], ID [ebf1c037-47eb-4355-998d-47ce9f2cc047] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Checkmark = new FieldDefinition
        {
            Title = "Completed",
            InternalName = "Checkmark",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Calculated,
            Id = new Guid("ebf1c037-47eb-4355-998d-47ce9f2cc047"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Length (seconds)], ID [de38f937-8578-435e-8cd3-50be3ea59253] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MediaLengthInSeconds = new FieldDefinition
        {
            Title = "Length (seconds)",
            InternalName = "MediaLengthInSeconds",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("de38f937-8578-435e-8cd3-50be3ea59253"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Assigned To], ID [53101f38-dd2e-458c-b245-0c236cc13d1a] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition AssignedTo = new FieldDefinition
        {
            Title = "Assigned To",
            InternalName = "AssignedTo",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("53101f38-dd2e-458c-b245-0c236cc13d1a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Parent Item ID], ID [7d014138-1886-41f0-834f-ba9f4e72f33b] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition ParentItemID = new FieldDefinition
        {
            Title = "Parent Item ID",
            InternalName = "ParentItemID",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("7d014138-1886-41f0-834f-ba9f4e72f33b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thumbnail], ID [ac7bb138-02dc-40eb-b07a-84c15575b6e9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Thumbnail = new FieldDefinition
        {
            Title = "Thumbnail",
            InternalName = "Thumbnail",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("ac7bb138-02dc-40eb-b07a-84c15575b6e9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [UDC Purpose], ID [8ee23f39-e2d1-4b46-8945-42386b24829d] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition Purpose = new FieldDefinition
        {
            Title = "UDC Purpose",
            InternalName = "Purpose",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("8ee23f39-e2d1-4b46-8945-42386b24829d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Visibility], ID [a05a8639-088a-4aea-b8a9-afc888971c81] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NoCodeVisibility = new FieldDefinition
        {
            Title = "Visibility",
            InternalName = "NoCodeVisibility",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("a05a8639-088a-4aea-b8a9-afc888971c81"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Source Name (Converted Document)], ID [774eab3a-855f-4a34-99da-69dc21043bec] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ParentLeafName = new FieldDefinition
        {
            Title = "Source Name (Converted Document)",
            InternalName = "ParentLeafName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("774eab3a-855f-4a34-99da-69dc21043bec"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Business Phone 2], ID [6547d03a-76d3-4d74-9d34-f51b837c0879] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Business2Number = new FieldDefinition
        {
            Title = "Business Phone 2",
            InternalName = "Business2Number",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("6547d03a-76d3-4d74-9d34-f51b837c0879"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [NoCrawl], ID [b0e12a3b-cf63-47d1-8418-4ef850d87a3c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NoCrawl = new FieldDefinition
        {
            Title = "NoCrawl",
            InternalName = "NoCrawl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("b0e12a3b-cf63-47d1-8418-4ef850d87a3c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Has Custom E-mail Body], ID [47f68c3b-8930-406f-bde2-4a8c669ee87c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HasCustomEmailBody = new FieldDefinition
        {
            Title = "Has Custom E-mail Body",
            InternalName = "HasCustomEmailBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("47f68c3b-8930-406f-bde2-4a8c669ee87c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [38bea83b-350a-1a6e-f34a-93a6af31338b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PostCategory = new FieldDefinition
        {
            Title = "Category",
            InternalName = "PostCategory",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("38bea83b-350a-1a6e-f34a-93a6af31338b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Shortest Thread-Index], ID [4753e73b-5b5d-4bbc-8e09-c9683b0d40a7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShortestThreadIndex = new FieldDefinition
        {
            Title = "Shortest Thread-Index",
            InternalName = "ShortestThreadIndex",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("4753e73b-5b5d-4bbc-8e09-c9683b0d40a7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Edit Menu Table Start], ID [1344423c-c7f9-4134-88e4-ad842e2d723c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _EditMenuTableStart2 = new FieldDefinition
        {
            Title = "Edit Menu Table Start",
            InternalName = "_EditMenuTableStart2",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("1344423c-c7f9-4134-88e4-ad842e2d723c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Telex], ID [e7be7f3c-c436-481d-8865-669e5146f53c] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition TelexNumber = new FieldDefinition
        {
            Title = "Telex",
            InternalName = "TelexNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e7be7f3c-c436-481d-8865-669e5146f53c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSPublishError], ID [321a8c3c-0ec7-473b-bcc6-67f3c2dae20d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSPublishError = new FieldDefinition
        {
            Title = "WSPublishError",
            InternalName = "WSPublishError",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("321a8c3c-0ec7-473b-bcc6-67f3c2dae20d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Restrict to Content Type ID], ID [8b02a33c-accd-4b73-bcae-6932c7aab812] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RestrictContentTypeId = new FieldDefinition
        {
            Title = "Restrict to Content Type ID",
            InternalName = "RestrictContentTypeId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("8b02a33c-accd-4b73-bcae-6932c7aab812"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Priority], ID [a8eb573e-9e11-481a-a8c9-1104a54b2fbd] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition Priority = new FieldDefinition
        {
            Title = "Priority",
            InternalName = "Priority",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("a8eb573e-9e11-481a-a8c9-1104a54b2fbd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Associated Service], ID [48b4a73e-8853-44ac-83a8-3a4bd59ce9ec] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Service = new FieldDefinition
        {
            Title = "Associated Service",
            InternalName = "Service",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("48b4a73e-8853-44ac-83a8-3a4bd59ce9ec"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User Field 3], ID [a03eb53e-f123-4af9-9355-f92bd75c00b3] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition UserField3 = new FieldDefinition
        {
            Title = "User Field 3",
            InternalName = "UserField3",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("a03eb53e-f123-4af9-9355-f92bd75c00b3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indentation], ID [26c4f53e-733a-4202-814b-377492b6c841] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Indentation = new FieldDefinition
        {
            Title = "Indentation",
            InternalName = "Indentation",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("26c4f53e-733a-4202-814b-377492b6c841"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Subject], ID [3ac9353f-613f-42bd-98e1-530e9fd1cbf6] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkDiscussionTitleNoMenu = new FieldDefinition
        {
            Title = "Subject",
            InternalName = "LinkDiscussionTitleNoMenu",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("3ac9353f-613f-42bd-98e1-530e9fd1cbf6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Body], ID [fbba993f-afee-4e00-b9be-36bc660dcdd1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MessageBody = new FieldDefinition
        {
            Title = "Body",
            InternalName = "MessageBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("fbba993f-afee-4e00-b9be-36bc660dcdd1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [% Complete], ID [d2311440-1ed6-46ea-b46d-daa643dc3886] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition PercentComplete = new FieldDefinition
        {
            Title = "% Complete",
            InternalName = "PercentComplete",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("d2311440-1ed6-46ea-b46d-daa643dc3886"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Crawler XSL File], ID [3c318a40-0d51-408d-ba71-16fa845b9fe5] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition CrawlerXSLFile = new FieldDefinition
        {
            Title = "Crawler XSL File",
            InternalName = "CrawlerXSLFile",
            Description = "Add an XSL file to be used to generate HTML when crawlers are viewing the page.",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("3c318a40-0d51-408d-ba71-16fa845b9fe5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home Address Street], ID [8c66e340-0985-4d68-af03-3050ece4862b] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomeAddressStreet = new FieldDefinition
        {
            Title = "Home Address Street",
            InternalName = "HomeAddressStreet",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("8c66e340-0985-4d68-af03-3050ece4862b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [UI Version], ID [7841bf41-43d0-4434-9f50-a673baef7631] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _UIVersion = new FieldDefinition
        {
            Title = "UI Version",
            InternalName = "_UIVersion",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("7841bf41-43d0-4434-9f50-a673baef7631"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Message ID], ID [2ef29342-2f5f-4052-90d3-8192e0705e51] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MessageId = new FieldDefinition
        {
            Title = "Message ID",
            InternalName = "MessageId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2ef29342-2f5f-4052-90d3-8192e0705e51"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Modified], ID [cc33f143-e697-42db-9c83-8db4e6928e9d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportModified = new FieldDefinition
        {
            Title = "Report Modified",
            InternalName = "ReportModified",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("cc33f143-e697-42db-9c83-8db4e6928e9d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Quoted Text Was Expanded], ID [e393d344-2e8c-425b-a8c3-89ac3144c9a2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition QuotedTextWasExpanded = new FieldDefinition
        {
            Title = "Quoted Text Was Expanded",
            InternalName = "QuotedTextWasExpanded",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("e393d344-2e8c-425b-a8c3-89ac3144c9a2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [List Type], ID [81dde544-1e25-4765-b5fd-ba613198d850] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ListType = new FieldDefinition
        {
            Title = "List Type",
            InternalName = "ListType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("81dde544-1e25-4765-b5fd-ba613198d850"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Subject], ID [b4e31c47-f962-4f9f-9132-eb555a1a026c] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkDiscussionTitle2 = new FieldDefinition
        {
            Title = "Subject",
            InternalName = "LinkDiscussionTitle2",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("b4e31c47-f962-4f9f-9132-eb555a1a026c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Show Repair View], ID [11851948-b05e-41be-9d9f-bc3bf55d1de3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShowRepairView = new FieldDefinition
        {
            Title = "Show Repair View",
            InternalName = "ShowRepairView",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("11851948-b05e-41be-9d9f-bc3bf55d1de3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Status], ID [1dab9b48-2d1a-47b3-878c-8e84f0d211ba] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Status = new FieldDefinition
        {
            Title = "Status",
            InternalName = "_Status",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("1dab9b48-2d1a-47b3-878c-8e84f0d211ba"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Task Type], ID [8d96aa48-9dff-46cf-8538-84c747ffa877] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TaskType = new FieldDefinition
        {
            Title = "Task Type",
            InternalName = "TaskType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("8d96aa48-9dff-46cf-8538-84c747ffa877"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Full Body], ID [9c4be348-663a-4172-a38a-9714b2634c17] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FullBody = new FieldDefinition
        {
            Title = "Full Body",
            InternalName = "FullBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("9c4be348-663a-4172-a38a-9714b2634c17"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Compatible UI Version(s)], ID [8e334549-c2bd-4110-9f61-672971be6504] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition UIVersion = new FieldDefinition
        {
            Title = "Compatible UI Version(s)",
            InternalName = "UIVersion",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.MultiChoice,
            Id = new Guid("8e334549-c2bd-4110-9f61-672971be6504"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WorkspaceUrl], ID [881eac4a-55a5-48b6-a28e-8329d7486120] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Workspace = new FieldDefinition
        {
            Title = "WorkspaceUrl",
            InternalName = "Workspace",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("881eac4a-55a5-48b6-a28e-8329d7486120"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Data], ID [78eae64a-f5f2-49af-b416-3247b76f46a1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormData = new FieldDefinition
        {
            Title = "Form Data",
            InternalName = "FormData",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("78eae64a-f5f2-49af-b416-3247b76f46a1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [System Task], ID [af0a3d4b-3ceb-449e-9bf4-51103f2032e3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SystemTask = new FieldDefinition
        {
            Title = "System Task",
            InternalName = "SystemTask",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("af0a3d4b-3ceb-449e-9bf4-51103f2032e3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [7615464b-559e-4302-b8e2-8f440b913101] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition BaseName = new FieldDefinition
        {
            Title = "Name",
            InternalName = "BaseName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("7615464b-559e-4302-b8e2-8f440b913101"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comments], ID [691b9a4b-512e-4341-b3f1-68914130d5b2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShortComment = new FieldDefinition
        {
            Title = "Comments",
            InternalName = "ShortComment",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("691b9a4b-512e-4341-b3f1-68914130d5b2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Required Field], ID [de1baa4b-2117-473b-aa0c-4d824034142d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RequiredField = new FieldDefinition
        {
            Title = "Required Field",
            InternalName = "RequiredField",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("de1baa4b-2117-473b-aa0c-4d824034142d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Custom Router], ID [dc7d1e4c-c725-4ffb-995b-4ff324656e91] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingCustomRouter = new FieldDefinition
        {
            Title = "Custom Router",
            InternalName = "RoutingCustomRouter",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dc7d1e4c-c725-4ffb-995b-4ff324656e91"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail], ID [fce16b4c-fe53-4793-aaab-b4892e736d15] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition EMail = new FieldDefinition
        {
            Title = "E-Mail",
            InternalName = "EMail",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fce16b4c-fe53-4793-aaab-b4892e736d15"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Type], ID [081c6e4c-5c14-4f20-b23e-1a71ceb6a67c] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition DocIcon = new FieldDefinition
        {
            Title = "Type",
            InternalName = "DocIcon",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("081c6e4c-5c14-4f20-b23e-1a71ceb6a67c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Assistant's Name], ID [2aea194d-e399-4f05-95af-94f87b1f2687] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition AssistantsName = new FieldDefinition
        {
            Title = "Assistant's Name",
            InternalName = "AssistantsName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2aea194d-e399-4f05-95af-94f87b1f2687"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Category], ID [65572d4d-445a-43f1-9c77-3358222a2c93] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormCategory = new FieldDefinition
        {
            Title = "Form Category",
            InternalName = "FormCategory",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("65572d4d-445a-43f1-9c77-3358222a2c93"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Radio Phone], ID [d1aede4f-1352-48d9-81e2-b10097c359c1] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition RadioNumber = new FieldDefinition
        {
            Title = "Radio Phone",
            InternalName = "RadioNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d1aede4f-1352-48d9-81e2-b10097c359c1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Display], ID [90244050-709c-4837-9316-93863fbd3da6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IMEDisplay = new FieldDefinition
        {
            Title = "Display",
            InternalName = "IMEDisplay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("90244050-709c-4837-9316-93863fbd3da6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Keywords], ID [b66e9b50-a28e-469b-b1a0-af0e45486874] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition Keywords = new FieldDefinition
        {
            Title = "Keywords",
            InternalName = "Keywords",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("b66e9b50-a28e-469b-b1a0-af0e45486874"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Failing Services], ID [e2b0b450-6795-4b86-86b7-3c21ab1797fb] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportServices = new FieldDefinition
        {
            Title = "Failing Services",
            InternalName = "HealthReportServices",
            Description = "The services for which the health analyzer rule failed.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("e2b0b450-6795-4b86-86b7-3c21ab1797fb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Job Title], ID [c4e0f350-52cc-4ede-904c-dd71a3d11f7d] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition JobTitle = new FieldDefinition
        {
            Title = "Job Title",
            InternalName = "JobTitle",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c4e0f350-52cc-4ede-904c-dd71a3d11f7d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [ab065451-14d6-485a-88c3-414c908d50d3] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition CategoryDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "CategoryDescription",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ab065451-14d6-485a-88c3-414c908d50d3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [End Date], ID [8a121252-85a9-443d-8217-a1b57020fadf] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition _EndDate = new FieldDefinition
        {
            Title = "End Date",
            InternalName = "_EndDate",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("8a121252-85a9-443d-8217-a1b57020fadf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Vacation Length], ID [44e16d52-da1b-4e72-8bdb-89a3b77ec8b0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NumberOfVacation = new FieldDefinition
        {
            Title = "Vacation Length",
            InternalName = "NumberOfVacation",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("44e16d52-da1b-4e72-8bdb-89a3b77ec8b0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Body], ID [8cbb9252-1035-4156-9c35-f54e9056c65a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailBody = new FieldDefinition
        {
            Title = "E-Mail Body",
            InternalName = "EmailBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("8cbb9252-1035-4156-9c35-f54e9056c65a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [6df9bd52-550e-4a30-bc31-a4366832a87d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Category = new FieldDefinition
        {
            Title = "Category",
            InternalName = "Category",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("6df9bd52-550e-4a30-bc31-a4366832a87d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Append-Only Comments], ID [6df9bd52-550e-4a30-bc31-a4366832a87e] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition V3Comments = new FieldDefinition
        {
            Title = "Append-Only Comments",
            InternalName = "V3Comments",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("6df9bd52-550e-4a30-bc31-a4366832a87e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [6df9bd52-550e-4a30-bc31-a4366832a87f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Comment = new FieldDefinition
        {
            Title = "Description",
            InternalName = "Comment",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("6df9bd52-550e-4a30-bc31-a4366832a87f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Related Content], ID [58ddda52-c2a3-4650-9178-3bbc1f6e36da] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowLink = new FieldDefinition
        {
            Title = "Related Content",
            InternalName = "WorkflowLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("58ddda52-c2a3-4650-9178-3bbc1f6e36da"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Department], ID [05fdf852-4b64-4096-9b2b-d2a62a86bc59] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Department = new FieldDefinition
        {
            Title = "Department",
            InternalName = "Department",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("05fdf852-4b64-4096-9b2b-d2a62a86bc59"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Relation], ID [5e75c854-6e9d-405d-b6c1-f8725bae5822] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Relation = new FieldDefinition
        {
            Title = "Relation",
            InternalName = "_Relation",
            Description = "References to related resources",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("5e75c854-6e9d-405d-b6c1-f8725bae5822"),
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
        /// Corresponds to built-in field with Title [Version], ID [6b6b1455-09ee-43b7-beea-4dc97456de2f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleVersion = new FieldDefinition
        {
            Title = "Version",
            InternalName = "HealthRuleVersion",
            Description = "Version",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("6b6b1455-09ee-43b7-beea-4dc97456de2f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [ZIP/Postal Code], ID [9a631556-3dac-49db-8d2f-fb033b0fdc24] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkZip = new FieldDefinition
        {
            Title = "ZIP/Postal Code",
            InternalName = "WorkZip",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9a631556-3dac-49db-8d2f-fb033b0fdc24"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [2fd53156-ff9d-4cc3-b0ac-fe8a7bc82283] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DLC_Description = new FieldDefinition
        {
            Title = "Description",
            InternalName = "DLC_Description",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2fd53156-ff9d-4cc3-b0ac-fe8a7bc82283"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Recurrence], ID [f2e63656-135e-4f1c-8fc2-ccbe74071901] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition fRecurrence = new FieldDefinition
        {
            Title = "Recurrence",
            InternalName = "fRecurrence",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Recurrence,
            Id = new Guid("f2e63656-135e-4f1c-8fc2-ccbe74071901"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Updated], ID [59956c56-30dd-4cb1-bf12-ef693b42679c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DiscussionLastUpdated = new FieldDefinition
        {
            Title = "Last Updated",
            InternalName = "DiscussionLastUpdated",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("59956c56-30dd-4cb1-bf12-ef693b42679c"),
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
        /// Corresponds to built-in field with Title [Home Address City], ID [5aeabc56-57c6-4861-bc12-bd72c30fc6bd] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomeAddressCity = new FieldDefinition
        {
            Title = "Home Address City",
            InternalName = "HomeAddressCity",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("5aeabc56-57c6-4861-bc12-bd72c30fc6bd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Posting Information], ID [f90bce56-87dc-4d73-bfcb-03fcaf670500] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition StatusBar = new FieldDefinition
        {
            Title = "Posting Information",
            InternalName = "StatusBar",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("f90bce56-87dc-4d73-bfcb-03fcaf670500"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Callback Number], ID [344e9657-b17f-4344-a834-ff7c056bcc5e] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition CallbackNumber = new FieldDefinition
        {
            Title = "Callback Number",
            InternalName = "CallbackNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("344e9657-b17f-4344-a834-ff7c056bcc5e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Issue ID], ID [03f89857-27c9-4b58-aaab-620647deda9b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LinkIssueIDNoMenu = new FieldDefinition
        {
            Title = "Issue ID",
            InternalName = "LinkIssueIDNoMenu",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("03f89857-27c9-4b58-aaab-620647deda9b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home Fax], ID [c189a857-e6b0-488f-83a0-f4ee0a3ad01e] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomeFaxNumber = new FieldDefinition
        {
            Title = "Home Fax",
            InternalName = "HomeFaxNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c189a857-e6b0-488f-83a0-f4ee0a3ad01e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Default CSS File], ID [cc10b158-50b4-4f02-8f3a-b9b6c3102628] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DefaultCssFile = new FieldDefinition
        {
            Title = "Default CSS File",
            InternalName = "DefaultCssFile",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("cc10b158-50b4-4f02-8f3a-b9b6c3102628"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Topic Last Updated By], ID [5d45db58-9ae3-4541-9bd0-759872d0d8d6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TopicLastRatedOrLikedBy = new FieldDefinition
        {
            Title = "Topic Last Updated By",
            InternalName = "TopicLastRatedOrLikedBy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("5d45db58-9ae3-4541-9bd0-759872d0d8d6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Most recent indicator data update], ID [fd3e3a59-bf10-4c99-b678-5dd7fcc6cb28] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition LastUpdated = new FieldDefinition
        {
            Title = "Most recent indicator data update",
            InternalName = "LastUpdated",
            Description = "The last indicator update from its datasource provider",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("fd3e3a59-bf10-4c99-b678-5dd7fcc6cb28"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Replies], ID [51139f59-4bac-45cb-8047-9c633eed1db0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NumberOfReplies = new FieldDefinition
        {
            Title = "Replies",
            InternalName = "NumberOfReplies",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("51139f59-4bac-45cb-8047-9c633eed1db0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Total Work], ID [f3c4a259-19a2-44b8-ab3d-e9145d07d538] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition TotalWork = new FieldDefinition
        {
            Title = "Total Work",
            InternalName = "TotalWork",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("f3c4a259-19a2-44b8-ab3d-e9145d07d538"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Default Encoding User Override], ID [28bb615a-bb92-43eb-9770-4f2926228dee] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetUserOverrideEncoding = new FieldDefinition
        {
            Title = "Default Encoding User Override",
            InternalName = "VideoSetUserOverrideEncoding",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("28bb615a-bb92-43eb-9770-4f2926228dee"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Active], ID [3c4e7a5b-b7d5-4779-a14a-490803e63923] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingEnabled = new FieldDefinition
        {
            Title = "Active",
            InternalName = "RoutingEnabled",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("3c4e7a5b-b7d5-4779-a14a-490803e63923"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Expires], ID [6a09e75b-8d17-4698-94a8-371eda1af1ac] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Expires = new FieldDefinition
        {
            Title = "Expires",
            InternalName = "Expires",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("6a09e75b-8d17-4698-94a8-371eda1af1ac"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Description], ID [1fff255c-6c88-4a76-957b-ae24bf07b78c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormDescription = new FieldDefinition
        {
            Title = "Form Description",
            InternalName = "FormDescription",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("1fff255c-6c88-4a76-957b-ae24bf07b78c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Issue Status], ID [3f277a5c-c7ae-4bbe-9d44-0456fb548f94] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition IssueStatus = new FieldDefinition
        {
            Title = "Issue Status",
            InternalName = "IssueStatus",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("3f277a5c-c7ae-4bbe-9d44-0456fb548f94"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [SIP Address], ID [829c275d-8744-4d9b-a42f-53f53eb60559] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SipAddress = new FieldDefinition
        {
            Title = "SIP Address",
            InternalName = "SipAddress",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("829c275d-8744-4d9b-a42f-53f53eb60559"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home Phone 2], ID [8c5a385d-2fff-42da-a4c5-f6a904f2e491] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Home2Number = new FieldDefinition
        {
            Title = "Home Phone 2",
            InternalName = "Home2Number",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("8c5a385d-2fff-42da-a4c5-f6a904f2e491"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Duration], ID [4d54445d-1c84-4a6d-b8db-a51ded4e1acc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Duration = new FieldDefinition
        {
            Title = "Duration",
            InternalName = "Duration",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("4d54445d-1c84-4a6d-b8db-a51ded4e1acc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author's Number of Best Replies], ID [ef72ef5d-9920-48f0-b2af-4e368feaabc6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorNumOfBestResponsesLookup = new FieldDefinition
        {
            Title = "Author's Number of Best Replies",
            InternalName = "AuthorNumOfBestResponsesLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("ef72ef5d-9920-48f0-b2af-4e368feaabc6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Repair Automatically], ID [1e41a55e-ef71-4740-b65a-d11e24c1d00d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleAutoRepairEnabled = new FieldDefinition
        {
            Title = "Repair Automatically",
            InternalName = "HealthRuleAutoRepairEnabled",
            Description = "If Repair Automatically is selected, SharePoint will attempt to repair errors as soon as they are found.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("1e41a55e-ef71-4740-b65a-d11e24c1d00d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Direct to Offsite], ID [6570d35e-7f0a-4123-93c9-f53ffa5810d3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GoFromHome = new FieldDefinition
        {
            Title = "Direct to Offsite",
            InternalName = "GoFromHome",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("6570d35e-7f0a-4123-93c9-f53ffa5810d3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Item Type], ID [30bb605f-5bae-48fe-b4e3-1f81d9772af9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FSObjType = new FieldDefinition
        {
            Title = "Item Type",
            InternalName = "FSObjType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("30bb605f-5bae-48fe-b4e3-1f81d9772af9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Status], ID [da25725f-5b12-4b26-8ed0-e560e4a87fff] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MemberStatus = new FieldDefinition
        {
            Title = "Status",
            InternalName = "MemberStatus",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("da25725f-5b12-4b26-8ed0-e560e4a87fff"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Resource Identifier], ID [3c76805f-ad45-483a-9c85-7ac24506ce1a] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Identifier = new FieldDefinition
        {
            Title = "Resource Identifier",
            InternalName = "_Identifier",
            Description = "An identifying string or number, usually conforming to a formal identification system",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3c76805f-ad45-483a-9c85-7ac24506ce1a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Completed], ID [35363960-d998-4aad-b7e8-058dfe2c669e] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Completed = new FieldDefinition
        {
            Title = "Completed",
            InternalName = "Completed",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("35363960-d998-4aad-b7e8-058dfe2c669e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail From], ID [e7cb6f60-f676-4b1d-89a3-975b6bc78cad] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailFrom = new FieldDefinition
        {
            Title = "E-Mail From",
            InternalName = "EmailFrom",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e7cb6f60-f676-4b1d-89a3-975b6bc78cad"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSEnabled], ID [a569e161-e19e-4360-9ae1-20dfda097cb3] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition WSEnabled = new FieldDefinition
        {
            Title = "WSEnabled",
            InternalName = "WSEnabled",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("a569e161-e19e-4360-9ae1-20dfda097cb3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Web Image URL], ID [a1ca0063-779f-49f9-999c-a4a2e3645b07] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EncodedAbsWebImgUrl = new FieldDefinition
        {
            Title = "Web Image URL",
            InternalName = "EncodedAbsWebImgUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("a1ca0063-779f-49f9-999c-a4a2e3645b07"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Posted by], ID [1805e563-22cf-44ed-96f5-58ebb8a6cb80] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MemberLookup = new FieldDefinition
        {
            Title = "Posted by",
            InternalName = "MemberLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("1805e563-22cf-44ed-96f5-58ebb8a6cb80"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [owshiddenversion], ID [d4e44a66-ee3a-4d02-88c9-4ec5ff3f4cd5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition owshiddenversion = new FieldDefinition
        {
            Title = "owshiddenversion",
            InternalName = "owshiddenversion",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("d4e44a66-ee3a-4d02-88c9-4ec5ff3f4cd5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Calendar UID], ID [f4e00567-8a9d-451b-82d4-a4447f9bd9a5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailCalendarUid = new FieldDefinition
        {
            Title = "E-Mail Calendar UID",
            InternalName = "EmailCalendarUid",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("f4e00567-8a9d-451b-82d4-a4447f9bd9a5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Rated By], ID [4d64b067-08c3-43dc-a87b-8b8e01673313] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RatedBy = new FieldDefinition
        {
            Title = "Rated By",
            InternalName = "RatedBy",
            Description = "Users rated the item.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("4d64b067-08c3-43dc-a87b-8b8e01673313"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSEventSource], ID [d8225d68-5ddb-4e66-8069-2694e8f628fb] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSEventSource = new FieldDefinition
        {
            Title = "WSEventSource",
            InternalName = "WSEventSource",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d8225d68-5ddb-4e66-8069-2694e8f628fb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Item ID], ID [8e234c69-02b0-42d9-8046-d5f49bf0174f] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition WorkflowItemId = new FieldDefinition
        {
            Title = "Workflow Item ID",
            InternalName = "WorkflowItemId",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("8e234c69-02b0-42d9-8046-d5f49bf0174f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Correct Body To Show], ID [b0204f69-2253-43d2-99ad-c0df00031b66] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition CorrectBodyToShow = new FieldDefinition
        {
            Title = "Correct Body To Show",
            InternalName = "CorrectBodyToShow",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("b0204f69-2253-43d2-99ad-c0df00031b66"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Configuration Url], ID [0f2f686a-3921-432e-85fd-9c535bf671b2] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSConfigurationUrl = new FieldDefinition
        {
            Title = "Configuration Url",
            InternalName = "DisplayTemplateJSConfigurationUrl",
            Description = "URL of custom page for configuring standalone view options.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("0f2f686a-3921-432e-85fd-9c535bf671b2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author's Number of Posts], ID [17828e6a-633b-443a-a503-1db661f1f5f7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorNumOfPostsLookup = new FieldDefinition
        {
            Title = "Author's Number of Posts",
            InternalName = "AuthorNumOfPostsLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("17828e6a-633b-443a-a503-1db661f1f5f7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [TaxKeywordTaxHTField], ID [1390a86a-23da-45f0-8efe-ef36edadfb39] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition TaxKeywordTaxHTField = new FieldDefinition
        {
            Title = "TaxKeywordTaxHTField",
            InternalName = "TaxKeywordTaxHTField",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("1390a86a-23da-45f0-8efe-ef36edadfb39"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Goal Cell], ID [4181b96a-3125-4f75-b823-3a4d675c6b19] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition GoalCell = new FieldDefinition
        {
            Title = "Goal Cell",
            InternalName = "GoalCell",
            Description = "Address or name of the cell for the goal",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4181b96a-3125-4f75-b823-3a4d675c6b19"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Billing Information], ID [4f03f66b-fb1e-4ed2-ab8e-f6ed3fe14844] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition BillingInformation = new FieldDefinition
        {
            Title = "Billing Information",
            InternalName = "BillingInformation",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4f03f66b-fb1e-4ed2-ab8e-f6ed3fe14844"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Start], ID [05e6336c-d22e-478e-9414-366762883b3f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Start = new FieldDefinition
        {
            Title = "Start",
            InternalName = "Start",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("05e6336c-d22e-478e-9414-366762883b3f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Has Copy Destinations], ID [26d0756c-986a-48a7-af35-bf18ab85ff4a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _HasCopyDestinations = new FieldDefinition
        {
            Title = "Has Copy Destinations",
            InternalName = "_HasCopyDestinations",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("26d0756c-986a-48a7-af35-bf18ab85ff4a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [8553196d-ec8d-4564-9861-3dbe931050c8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FileLeafRef = new FieldDefinition
        {
            Title = "Name",
            InternalName = "FileLeafRef",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.File,
            Id = new Guid("8553196d-ec8d-4564-9861-3dbe931050c8"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Name Phonetic], ID [fdc8216d-dabf-441d-8ac0-f6c626fbdc24] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition LastNamePhonetic = new FieldDefinition
        {
            Title = "Last Name Phonetic",
            InternalName = "LastNamePhonetic",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fdc8216d-dabf-441d-8ac0-f6c626fbdc24"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Copy Source], ID [6b4e226d-3d88-4a36-808d-a129bf52bccf] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _CopySource = new FieldDefinition
        {
            Title = "Copy Source",
            InternalName = "_CopySource",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("6b4e226d-3d88-4a36-808d-a129bf52bccf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [b0bd6f6d-ed80-4ff8-8be0-ef9238a16835] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportLinkFilename = new FieldDefinition
        {
            Title = "Name",
            InternalName = "ReportLinkFilename",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("b0bd6f6d-ed80-4ff8-8be0-ef9238a16835"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Goal Sheet], ID [69ac2b6e-f626-49b4-b0cc-c2b9580e8719] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition GoalSheet = new FieldDefinition
        {
            Title = "Goal Sheet",
            InternalName = "GoalSheet",
            Description = "Name of the sheet for the goal",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("69ac2b6e-f626-49b4-b0cc-c2b9580e8719"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [FTP Site], ID [d733736e-4204-4812-9565-191567b27e33] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition FTPSite = new FieldDefinition
        {
            Title = "FTP Site",
            InternalName = "FTPSite",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("d733736e-4204-4812-9565-191567b27e33"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Web Part], ID [4499086f-9ac1-41df-86c3-d8c1f8fc769a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XSLStyleWPType = new FieldDefinition
        {
            Title = "Target Web Part",
            InternalName = "XSLStyleWPType",
            Description = "Specify the type of web part this style is intended for.  For default list views, choose XSLTListViewWebPart.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("4499086f-9ac1-41df-86c3-d8c1f8fc769a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indentation Level], ID [68227570-72dd-4816-b6b6-4b81ff99a393] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IndentLevel = new FieldDefinition
        {
            Title = "Indentation Level",
            InternalName = "IndentLevel",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("68227570-72dd-4816-b6b6-4b81ff99a393"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow History Parent Instance], ID [de21c770-a12b-4f88-af4b-aeebd897c8c2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowInstance = new FieldDefinition
        {
            Title = "Workflow History Parent Instance",
            InternalName = "WorkflowInstance",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("de21c770-a12b-4f88-af4b-aeebd897c8c2"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Resource Type], ID [edecec70-f6e2-4c3c-a4c7-f61a515dfaa9] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _ResourceType = new FieldDefinition
        {
            Title = "Resource Type",
            InternalName = "_ResourceType",
            Description = "A set of categories, functions, genres or aggregation levels",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("edecec70-f6e2-4c3c-a4c7-f61a515dfaa9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [HiddenParticipants], ID [453c2d71-c41e-46bc-97c1-a5a9535053a3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Participants = new FieldDefinition
        {
            Title = "HiddenParticipants",
            InternalName = "Participants",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("453c2d71-c41e-46bc-97c1-a5a9535053a3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Resolved Date], ID [c4995c71-4c5c-4e9f-afc1-a9033f2bfde5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ResolvedDate = new FieldDefinition
        {
            Title = "Resolved Date",
            InternalName = "ResolvedDate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("c4995c71-4c5c-4e9f-afc1-a9033f2bfde5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Confidential], ID [9b0e6471-c5c5-42ef-9ade-63170bf28819] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Confidential = new FieldDefinition
        {
            Title = "Confidential",
            InternalName = "Confidential",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("9b0e6471-c5c5-42ef-9ade-63170bf28819"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Post], ID [4c481e72-f3fa-46d7-98dd-a258c3df5403] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DiscussionTitleOrBody = new FieldDefinition
        {
            Title = "Post",
            InternalName = "DiscussionTitleOrBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("4c481e72-f3fa-46d7-98dd-a258c3df5403"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Value Expression], ID [93df9772-1e34-40c5-8c54-4bf3cdd56b34] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition ValueExpression = new FieldDefinition
        {
            Title = "Value Expression",
            InternalName = "ValueExpression",
            Description = "Expression used to determine the value of the Indicator.",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("93df9772-1e34-40c5-8c54-4bf3cdd56b34"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Control Type], ID [0e49b273-3102-4b7d-b609-2e05dd1a17d9] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSTargetControlType = new FieldDefinition
        {
            Title = "Target Control Type",
            InternalName = "DisplayTemplateJSTargetControlType",
            Description = "Type of control defined in this Display Template.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("0e49b273-3102-4b7d-b609-2e05dd1a17d9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Warning Sheet], ID [3e223474-75ff-466b-b53f-9b641ce74b6c] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition WarningSheet = new FieldDefinition
        {
            Title = "Warning Sheet",
            InternalName = "WarningSheet",
            Description = "Name of the sheet for the warning",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3e223474-75ff-466b-b53f-9b641ce74b6c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Threading Controls], ID [c55a4674-640b-4bae-8738-ce0439e6f6d4] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ThreadingControls = new FieldDefinition
        {
            Title = "Threading Controls",
            InternalName = "ThreadingControls",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("c55a4674-640b-4bae-8738-ce0439e6f6d4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contact Information], ID [e1a85174-b8d0-4962-9ce6-758f8b612725] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ContactInfo = new FieldDefinition
        {
            Title = "Contact Information",
            InternalName = "ContactInfo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.ContactInfo,
            Id = new Guid("e1a85174-b8d0-4962-9ce6-758f8b612725"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Notification], ID [cf68a174-123b-413e-9ec1-b43e3a3175d7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WhatsNew = new FieldDefinition
        {
            Title = "Notification",
            InternalName = "WhatsNew",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("cf68a174-123b-413e-9ec1-b43e3a3175d7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Pager], ID [f79bf074-daf7-4c06-a314-15b287fdf4c9] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition PagerNumber = new FieldDefinition
        {
            Title = "Pager",
            InternalName = "PagerNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f79bf074-daf7-4c06-a314-15b287fdf4c9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Name], ID [1a03fa74-8c63-40cc-bd06-73b580bd8743] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LinkTemplateName = new FieldDefinition
        {
            Title = "Form Name",
            InternalName = "LinkTemplateName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("1a03fa74-8c63-40cc-bd06-73b580bd8743"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form ID], ID [1a03fa74-8c63-40cc-bd06-73b580bd8744] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormId = new FieldDefinition
        {
            Title = "Form ID",
            InternalName = "FormId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("1a03fa74-8c63-40cc-bd06-73b580bd8744"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Body Was Expanded], ID [af82aa75-3039-4573-84a8-73ffdfd22733] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition BodyWasExpanded = new FieldDefinition
        {
            Title = "Body Was Expanded",
            InternalName = "BodyWasExpanded",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("af82aa75-3039-4573-84a8-73ffdfd22733"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Rule Settings], ID [cf4ff575-f1f5-4c5b-b595-54bbcccd0c62] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleReportLink = new FieldDefinition
        {
            Title = "Rule Settings",
            InternalName = "HealthRuleReportLink",
            Description = "A link to the settings page that controls if and when this rule is run.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("cf4ff575-f1f5-4c5b-b595-54bbcccd0c62"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Event Type], ID [5d1d4e76-091a-4e03-ae83-6a59847731c0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EventType = new FieldDefinition
        {
            Title = "Event Type",
            InternalName = "EventType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("5d1d4e76-091a-4e03-ae83-6a59847731c0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Check In Comment], ID [58014f77-5463-437b-ab67-eec79532da67] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _CheckinComment = new FieldDefinition
        {
            Title = "Check In Comment",
            InternalName = "_CheckinComment",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("58014f77-5463-437b-ab67-eec79532da67"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Picture], ID [d9339777-b964-489a-bf09-2ac3c3fe5f0d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Picture = new FieldDefinition
        {
            Title = "Picture",
            InternalName = "Picture",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("d9339777-b964-489a-bf09-2ac3c3fe5f0d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Vacation Type], ID [dfd58778-bf8e-4769-8265-09ac03159eed] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Vacation = new FieldDefinition
        {
            Title = "Vacation Type",
            InternalName = "Vacation",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("dfd58778-bf8e-4769-8265-09ac03159eed"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hobbies], ID [203fa378-6eb8-4ed9-a4f9-221a4c1fbf46] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Hobbies = new FieldDefinition
        {
            Title = "Hobbies",
            InternalName = "Hobbies",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("203fa378-6eb8-4ed9-a4f9-221a4c1fbf46"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Limited Body], ID [61b97279-cbc0-4aa9-a362-f1ff249c1706] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LimitedBody = new FieldDefinition
        {
            Title = "Limited Body",
            InternalName = "LimitedBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("61b97279-cbc0-4aa9-a362-f1ff249c1706"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contributor], ID [370b7779-0344-4b9f-8f2d-dc1c62eae801] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Contributor = new FieldDefinition
        {
            Title = "Contributor",
            InternalName = "_Contributor",
            Description = "One or more people or organizations that contributed to this resource",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("370b7779-0344-4b9f-8f2d-dc1c62eae801"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [5cc6dc79-3710-4374-b433-61cb4a686c12] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkFilename = new FieldDefinition
        {
            Title = "Name",
            InternalName = "LinkFilename",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("5cc6dc79-3710-4374-b433-61cb4a686c12"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Resolved By], ID [b4fa187b-eb65-478e-8bc6-93b0da320f03] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ResolvedBy = new FieldDefinition
        {
            Title = "Resolved By",
            InternalName = "ResolvedBy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("b4fa187b-eb65-478e-8bc6-93b0da320f03"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Relink], ID [5d36727b-bcb2-47d2-a231-1f0bc63b7439] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RepairDocument = new FieldDefinition
        {
            Title = "Relink",
            InternalName = "RepairDocument",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("5d36727b-bcb2-47d2-a231-1f0bc63b7439"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Owner], ID [2de1df7b-48e1-4c8e-be0f-f00e504b9948] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetOwner = new FieldDefinition
        {
            Title = "Owner",
            InternalName = "VideoSetOwner",
            Description = "The owner of the Video",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("2de1df7b-48e1-4c8e-be0f-f00e504b9948"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Value Cell], ID [3b32f47b-f1a1-45ff-b5ad-7b28b84c720a] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition ValueCell = new FieldDefinition
        {
            Title = "Value Cell",
            InternalName = "ValueCell",
            Description = "Address or name of the cell for the value",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3b32f47b-f1a1-45ff-b5ad-7b28b84c720a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Version], ID [94ad6f7c-09a1-42ca-974f-d24e080160c2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormVersion = new FieldDefinition
        {
            Title = "Form Version",
            InternalName = "FormVersion",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("94ad6f7c-09a1-42ca-974f-d24e080160c2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [URL], ID [c29e077d-f466-4d8e-8bbe-72b66c5f205c] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition URL = new FieldDefinition
        {
            Title = "URL",
            InternalName = "URL",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("c29e077d-f466-4d8e-8bbe-72b66c5f205c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Modified], ID [3892917d-92f2-4263-ae0c-22670474069d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportModifiedDisplay = new FieldDefinition
        {
            Title = "Report Modified",
            InternalName = "ReportModifiedDisplay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("3892917d-92f2-4263-ae0c-22670474069d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Actual Work], ID [b0b3407e-1c33-40ed-a37c-2430b7a5d081] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition ActualWork = new FieldDefinition
        {
            Title = "Actual Work",
            InternalName = "ActualWork",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("b0b3407e-1c33-40ed-a37c-2430b7a5d081"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Post], ID [c7e9537e-bde4-4923-a100-adbd9e0a0a0d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition BodyAndMore = new FieldDefinition
        {
            Title = "Post",
            InternalName = "BodyAndMore",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("c7e9537e-bde4-4923-a100-adbd9e0a0a0d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSGUID], ID [6391dc7e-e7f3-4faf-9890-550fd5d3022f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSGUID = new FieldDefinition
        {
            Title = "WSGUID",
            InternalName = "WSGUID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("6391dc7e-e7f3-4faf-9890-550fd5d3022f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Item Child Count], ID [b824e17e-a1b3-426e-aecf-f0184d900485] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ItemChildCount = new FieldDefinition
        {
            Title = "Item Child Count",
            InternalName = "ItemChildCount",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("b824e17e-a1b3-426e-aecf-f0184d900485"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Group Type], ID [c86a2f7f-7680-4a0b-8907-39c4f4855a35] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Group = new FieldDefinition
        {
            Title = "Group Type",
            InternalName = "Group",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("c86a2f7f-7680-4a0b-8907-39c4f4855a35"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [City], ID [6ca7bd7f-b490-402e-af1b-2813cf087b1e] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkCity = new FieldDefinition
        {
            Title = "City",
            InternalName = "WorkCity",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("6ca7bd7f-b490-402e-af1b-2813cf087b1e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Published], ID [b1b53d80-23d6-e31b-b235-3a286b9f10ea] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PublishedDate = new FieldDefinition
        {
            Title = "Published",
            InternalName = "PublishedDate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("b1b53d80-23d6-e31b-b235-3a286b9f10ea"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Association ID], ID [8d426880-8d96-459b-ae48-e8b3836d8b9d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowAssociation = new FieldDefinition
        {
            Title = "Workflow Association ID",
            InternalName = "WorkflowAssociation",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("8d426880-8d96-459b-ae48-e8b3836d8b9d"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Message], ID [6529a881-d745-4117-a552-3dcc7110e9b8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Detail = new FieldDefinition
        {
            Title = "Message",
            InternalName = "Detail",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("6529a881-d745-4117-a552-3dcc7110e9b8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Shortest Thread-Index Id], ID [2bec4782-695f-406d-9e50-f1d39a2b8eb6] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition ShortestThreadIndexId = new FieldDefinition
        {
            Title = "Shortest Thread-Index Id",
            InternalName = "ShortestThreadIndexId",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2bec4782-695f-406d-9e50-f1d39a2b8eb6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Answered], ID [32b1ca82-a25b-48d1-b78d-3a956ba07c41] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IsAnswered = new FieldDefinition
        {
            Title = "Is Answered",
            InternalName = "IsAnswered",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("32b1ca82-a25b-48d1-b78d-3a956ba07c41"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Company Main Phone], ID [27cb1283-bda2-4ae8-bcff-71725b674dbb] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition CompanyNumber = new FieldDefinition
        {
            Title = "Company Main Phone",
            InternalName = "CompanyNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("27cb1283-bda2-4ae8-bcff-71725b674dbb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Discussions], ID [d2264183-83dc-4d08-a57d-974686192d7a] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition TopicCount = new FieldDefinition
        {
            Title = "Discussions",
            InternalName = "TopicCount",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("d2264183-83dc-4d08-a57d-974686192d7a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [93490584-b6a8-4996-aa00-ead5f59aae0d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AdminTaskDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "AdminTaskDescription",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("93490584-b6a8-4996-aa00-ead5f59aae0d"),
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
        /// Corresponds to built-in field with Title [MasterSeriesItemID], ID [9b2bed84-7769-40e3-9b1d-7954a4053834] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MasterSeriesItemID = new FieldDefinition
        {
            Title = "MasterSeriesItemID",
            InternalName = "MasterSeriesItemID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("9b2bed84-7769-40e3-9b1d-7954a4053834"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [HTML File Type], ID [0c5e0085-eb30-494b-9cdd-ece1d3c649a2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HTML_x0020_File_x0020_Type = new FieldDefinition
        {
            Title = "HTML File Type",
            InternalName = "HTML_x0020_File_x0020_Type",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("0c5e0085-eb30-494b-9cdd-ece1d3c649a2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Template Level], ID [fa181e85-8465-42fd-bd81-4afea427d3fe] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateLevel = new FieldDefinition
        {
            Title = "Template Level",
            InternalName = "DisplayTemplateLevel",
            Description = "Select the level of data that this display template expects and is designed to display.  This determines where this template will appear as a selectable option in configuration UIs.",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("fa181e85-8465-42fd-bd81-4afea427d3fe"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Country/Region], ID [3f3a5c85-9d5a-4663-b925-8b68a678ea3a] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkCountry = new FieldDefinition
        {
            Title = "Country/Region",
            InternalName = "WorkCountry",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3f3a5c85-9d5a-4663-b925-8b68a678ea3a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Select], ID [5f47e085-2150-41dc-b661-442f3027f552] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SelectFilename = new FieldDefinition
        {
            Title = "Select",
            InternalName = "SelectFilename",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("5f47e085-2150-41dc-b661-442f3027f552"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [URL], ID [84b0fe85-6b16-40c3-8507-e56c5bbc482e] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IMEUrl = new FieldDefinition
        {
            Title = "URL",
            InternalName = "IMEUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("84b0fe85-6b16-40c3-8507-e56c5bbc482e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Initials], ID [7a282f86-69d9-40ff-ae1c-c746cf21256b] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Initials = new FieldDefinition
        {
            Title = "Initials",
            InternalName = "Initials",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("7a282f86-69d9-40ff-ae1c-c746cf21256b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [About Me], ID [e241f186-9b94-415c-9f66-255ce7f86235] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Notes = new FieldDefinition
        {
            Title = "About Me",
            InternalName = "Notes",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("e241f186-9b94-415c-9f66-255ce7f86235"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hidden], ID [e8a80787-5f99-459a-af8d-b830157ed45f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition UserInfoHidden = new FieldDefinition
        {
            Title = "Hidden",
            InternalName = "UserInfoHidden",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("e8a80787-5f99-459a-af8d-b830157ed45f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [76d1cc87-56de-432c-8a2a-16e5ba5331b3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NameOrTitle = new FieldDefinition
        {
            Title = "Name",
            InternalName = "NameOrTitle",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("76d1cc87-56de-432c-8a2a-16e5ba5331b3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Gender], ID [23550288-91b5-4e7f-81f9-1a92661c4838] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Gender = new FieldDefinition
        {
            Title = "Gender",
            InternalName = "Gender",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("23550288-91b5-4e7f-81f9-1a92661c4838"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Best Replies], ID [1bc74b88-bb81-4be5-961d-9cf75dfe0911] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NumberOfBestResponses = new FieldDefinition
        {
            Title = "Best Replies",
            InternalName = "NumberOfBestResponses",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("1bc74b88-bb81-4be5-961d-9cf75dfe0911"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Company Phonetic], ID [034aae88-6e9a-4e41-bc8a-09b6c15fcdf4] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition CompanyPhonetic = new FieldDefinition
        {
            Title = "Company Phonetic",
            InternalName = "CompanyPhonetic",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("034aae88-6e9a-4e41-bc8a-09b6c15fcdf4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow List ID], ID [1bfee788-69b7-4765-b109-d4d9c31d1ac1] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition WorkflowListId = new FieldDefinition
        {
            Title = "Workflow List ID",
            InternalName = "WorkflowListId",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("1bfee788-69b7-4765-b109-d4d9c31d1ac1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Submission Content Type (Internal)], ID [baf14289-0687-448c-b13c-c98dc4183e06] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingContentTypeInternal = new FieldDefinition
        {
            Title = "Submission Content Type (Internal)",
            InternalName = "RoutingContentTypeInternal",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("baf14289-0687-448c-b13c-c98dc4183e06"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Required Fields], ID [acb9088a-a171-4b99-aa7a-10388586bc74] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XSLStyleRequiredFields = new FieldDefinition
        {
            Title = "Required Fields",
            InternalName = "XSLStyleRequiredFields",
            Description = "The required fields for this style in the format: field1; field2;",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("acb9088a-a171-4b99-aa7a-10388586bc74"),
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
        /// Corresponds to built-in field with Title [Author's Number of Replies], ID [0e9d978a-e4bc-4efb-a214-75fc81bd8096] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorNumOfRepliesLookup = new FieldDefinition
        {
            Title = "Author's Number of Replies",
            InternalName = "AuthorNumOfRepliesLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("0e9d978a-e4bc-4efb-a214-75fc81bd8096"),
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
        /// Corresponds to built-in field with Title [List ID], ID [f44e428b-61c8-4100-a911-a3a635f43bb5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition List = new FieldDefinition
        {
            Title = "List ID",
            InternalName = "List",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("f44e428b-61c8-4100-a911-a3a635f43bb5"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Overtime], ID [35d79e8b-3701-4659-9c27-c070ed3c2bfa] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Overtime = new FieldDefinition
        {
            Title = "Overtime",
            InternalName = "Overtime",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("35d79e8b-3701-4659-9c27-c070ed3c2bfa"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Reply By], ID [7f15088c-1448-41c7-a125-18a3a90ce543] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LastReplyBy = new FieldDefinition
        {
            Title = "Last Reply By",
            InternalName = "LastReplyBy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("7f15088c-1448-41c7-a125-18a3a90ce543"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Task Group], ID [50d8f08c-8e99-4948-97bf-2be41fa34a0d] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition TaskGroup = new FieldDefinition
        {
            Title = "Task Group",
            InternalName = "TaskGroup",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("50d8f08c-8e99-4948-97bf-2be41fa34a0d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSDescription], ID [f519008d-1b5b-42a8-8dab-e1a642fe5787] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSDescription = new FieldDefinition
        {
            Title = "WSDescription",
            InternalName = "WSDescription",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f519008d-1b5b-42a8-8dab-e1a642fe5787"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Start Date], ID [64cd368d-2f95-4bfc-a1f9-8d4324ecb007] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition StartDate = new FieldDefinition
        {
            Title = "Start Date",
            InternalName = "StartDate",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("64cd368d-2f95-4bfc-a1f9-8d4324ecb007"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Address], ID [fc2e188e-ba91-48c9-9dd3-16431afddd50] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkAddress = new FieldDefinition
        {
            Title = "Address",
            InternalName = "WorkAddress",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("fc2e188e-ba91-48c9-9dd3-16431afddd50"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Address City], ID [90fa9a8e-aac0-4828-9cb4-78f98416affa] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherAddressCity = new FieldDefinition
        {
            Title = "Other Address City",
            InternalName = "OtherAddressCity",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("90fa9a8e-aac0-4828-9cb4-78f98416affa"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Related Company], ID [3914f98e-6d99-4218-9ba3-af7370b9e7bc] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition TaskCompanies = new FieldDefinition
        {
            Title = "Related Company",
            InternalName = "TaskCompanies",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3914f98e-6d99-4218-9ba3-af7370b9e7bc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Default Encoding], ID [1f300f90-c9d2-41f5-8ebb-7f2829a4c977] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetDefaultEncoding = new FieldDefinition
        {
            Title = "Default Encoding",
            InternalName = "VideoSetDefaultEncoding",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("1f300f90-c9d2-41f5-8ebb-7f2829a4c977"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Custom ID Number], ID [81368791-7cbc-4230-981a-a7669ade9801] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition CustomerID = new FieldDefinition
        {
            Title = "Custom ID Number",
            InternalName = "CustomerID",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("81368791-7cbc-4230-981a-a7669ade9801"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Show Download Link], ID [9cb4d391-367f-4342-8f17-ac808799784a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetShowDownloadLink = new FieldDefinition
        {
            Title = "Show Download Link",
            InternalName = "VideoSetShowDownloadLink",
            Description = "Specifies whether a button appears on the video player page that allows the user to download the video being played.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("9cb4d391-367f-4342-8f17-ac808799784a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Status], ID [ac3a1092-34ad-42b2-8d47-a79d01d9f516] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DecisionStatus = new FieldDefinition
        {
            Title = "Status",
            InternalName = "DecisionStatus",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("ac3a1092-34ad-42b2-8d47-a79d01d9f516"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Type], ID [7dd0a092-8704-4ed2-8253-ac309150ac59] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleType = new FieldDefinition
        {
            Title = "Type",
            InternalName = "HealthRuleType",
            Description = "The rule type specifies the fully-qualified assembly name of the rule class.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("7dd0a092-8704-4ed2-8253-ac309150ac59"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Gifted Badge], ID [abe62893-898d-48f9-9a52-3778b420f81c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GiftedBadgeLookup = new FieldDefinition
        {
            Title = "Gifted Badge",
            InternalName = "GiftedBadgeLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("abe62893-898d-48f9-9a52-3778b420f81c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Location], ID [e2a07293-596a-4c59-9089-5c4f9339077f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Whereabout = new FieldDefinition
        {
            Title = "Location",
            InternalName = "Whereabout",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Whereabout,
            Id = new Guid("e2a07293-596a-4c59-9089-5c4f9339077f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Trimmed Body], ID [6d0f8993-5050-41f3-be6c-18902d282357] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TrimmedBody = new FieldDefinition
        {
            Title = "Trimmed Body",
            InternalName = "TrimmedBody",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("6d0f8993-5050-41f3-be6c-18902d282357"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Connection Type], ID [939dfb93-3107-44c6-a98f-dd88dca3f8cf] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ConnectionType = new FieldDefinition
        {
            Title = "Connection Type",
            InternalName = "ConnectionType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("939dfb93-3107-44c6-a98f-dd88dca3f8cf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSDisplayName], ID [9b590294-0151-44cf-9d56-24d8bb80f802] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSDisplayName = new FieldDefinition
        {
            Title = "WSDisplayName",
            InternalName = "WSDisplayName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9b590294-0151-44cf-9d56-24d8bb80f802"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [More Link], ID [fb6c2494-1b14-49b0-a7ca-0506d6e85a62] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MoreLink = new FieldDefinition
        {
            Title = "More Link",
            InternalName = "MoreLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("fb6c2494-1b14-49b0-a7ca-0506d6e85a62"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Property Bag], ID [687c7f94-686a-42d3-9b67-2782eac4b4f8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MetaInfo = new FieldDefinition
        {
            Title = "Property Bag",
            InternalName = "MetaInfo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("687c7f94-686a-42d3-9b67-2782eac4b4f8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Content Type ID], ID [58eb8694-8bd6-4f98-8097-374bd97ffec4] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition CustomContentTypeId = new FieldDefinition
        {
            Title = "Content Type ID",
            InternalName = "CustomContentTypeId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("58eb8694-8bd6-4f98-8097-374bd97ffec4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Data], ID [38269294-165e-448a-a6b9-f0e09688f3f9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Data = new FieldDefinition
        {
            Title = "Data",
            InternalName = "Data",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("38269294-165e-448a-a6b9-f0e09688f3f9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Phone], ID [96e02495-f428-48bc-9f13-06d98ba58c34] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherNumber = new FieldDefinition
        {
            Title = "Other Phone",
            InternalName = "OtherNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("96e02495-f428-48bc-9f13-06d98ba58c34"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Control Type (Search)], ID [cab85295-b195-4ac2-8323-87c602e6ac9d] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition TargetControlType = new FieldDefinition
        {
            Title = "Target Control Type (Search)",
            InternalName = "TargetControlType",
            Description = "Select the search controls that will use this Display Template.",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.MultiChoice,
            Id = new Guid("cab85295-b195-4ac2-8323-87c602e6ac9d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Computer Network Name], ID [86a78395-c8ad-429e-abff-be09417b523e] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ComputerNetworkName = new FieldDefinition
        {
            Title = "Computer Network Name",
            InternalName = "ComputerNetworkName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("86a78395-c8ad-429e-abff-be09417b523e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Revision], ID [16b4ab96-0ce5-4c82-a836-f3117e8996ff] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Revision = new FieldDefinition
        {
            Title = "Revision",
            InternalName = "_Revision",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("16b4ab96-0ce5-4c82-a836-f3117e8996ff"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Created], ID [a533d496-5aeb-4027-9542-ee6ba9a8c9e3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportCreatedDisplay = new FieldDefinition
        {
            Title = "Report Created",
            InternalName = "ReportCreatedDisplay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("a533d496-5aeb-4027-9542-ee6ba9a8c9e3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Question], ID [7aead996-f9f9-4682-9e0e-f5634ab352c8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IsQuestion = new FieldDefinition
        {
            Title = "Question",
            InternalName = "IsQuestion",
            Description = "I am asking a question and want to get answers from other members.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("7aead996-f9f9-4682-9e0e-f5634ab352c8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Predecessors], ID [c3a92d97-2b77-4a25-9698-3ab54874bc6f] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition Predecessors = new FieldDefinition
        {
            Title = "Predecessors",
            InternalName = "Predecessors",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("c3a92d97-2b77-4a25-9698-3ab54874bc6f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author's last activity date], ID [5f060e98-3a10-42d0-baf6-1d60889fc585] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorLastActivityLookup = new FieldDefinition
        {
            Title = "Author's last activity date",
            InternalName = "AuthorLastActivityLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("5f060e98-3a10-42d0-baf6-1d60889fc585"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Location], ID [afaa4198-9797-4e45-9825-8f7e7b0f5dd5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GbwLocation = new FieldDefinition
        {
            Title = "Location",
            InternalName = "GbwLocation",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("afaa4198-9797-4e45-9825-8f7e7b0f5dd5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Discussion Title], ID [f0218b98-d0d6-4fc1-b15b-aabeb89f32a9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DiscussionTitleLookup = new FieldDefinition
        {
            Title = "Discussion Title",
            InternalName = "DiscussionTitleLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("f0218b98-d0d6-4fc1-b15b-aabeb89f32a9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reports Lookup], ID [69062c99-d89f-4162-bbc5-b1acf8bfe123] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AbuseReportsLookup = new FieldDefinition
        {
            Title = "Reports Lookup",
            InternalName = "AbuseReportsLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("69062c99-d89f-4162-bbc5-b1acf8bfe123"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Out], ID [fde05b9b-52bf-43dc-9b96-bb35fa7aa05d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Out = new FieldDefinition
        {
            Title = "Out",
            InternalName = "Out",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fde05b9b-52bf-43dc-9b96-bb35fa7aa05d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Select], ID [b1f7969b-ea65-42e1-8b54-b588292635f2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SelectTitle = new FieldDefinition
        {
            Title = "Select",
            InternalName = "SelectTitle",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("b1f7969b-ea65-42e1-8b54-b588292635f2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Allow Editing], ID [7266b59c-030b-4ca3-bc09-bb8e76ad969b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AllowEditing = new FieldDefinition
        {
            Title = "Allow Editing",
            InternalName = "AllowEditing",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("7266b59c-030b-4ca3-bc09-bb8e76ad969b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Status], ID [bf80df9c-32dc-4257-bcf9-08c2ee6ca1b1] and Group: [Reports]'
        /// </summary>
        public static FieldDefinition ReportStatus = new FieldDefinition
        {
            Title = "Report Status",
            InternalName = "ReportStatus",
            Description = "Status of the report",
            Group = "Reports",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("bf80df9c-32dc-4257-bcf9-08c2ee6ca1b1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Source URL], ID [c63a459d-54ba-4ab7-933a-dcf1c6fadec2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _SourceUrl = new FieldDefinition
        {
            Title = "Source URL",
            InternalName = "_SourceUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c63a459d-54ba-4ab7-933a-dcf1c6fadec2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reports], ID [c3fc749d-c4a7-478b-a915-21c1c68f7199] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AbuseReportsCount = new FieldDefinition
        {
            Title = "Reports",
            InternalName = "AbuseReportsCount",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("c3fc749d-c4a7-478b-a915-21c1c68f7199"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Locale], ID [96c27c9d-33f5-4f8e-893e-684014bc7090] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormLocale = new FieldDefinition
        {
            Title = "Form Locale",
            InternalName = "FormLocale",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("96c27c9d-33f5-4f8e-893e-684014bc7090"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Response], ID [3329f39d-70ed-4858-b8c8-c5237634bf08] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AttendeeStatus = new FieldDefinition
        {
            Title = "Response",
            InternalName = "AttendeeStatus",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("3329f39d-70ed-4858-b8c8-c5237634bf08"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indicator Warning Threshold], ID [e84a049e-230d-4751-8d5c-8f615e968df2] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition Warning = new FieldDefinition
        {
            Title = "Indicator Warning Threshold",
            InternalName = "Warning",
            Description = "",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("e84a049e-230d-4751-8d5c-8f615e968df2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User Field 2], ID [182d1b9e-1718-4e11-b279-38f7ed0a20d6] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition UserField2 = new FieldDefinition
        {
            Title = "User Field 2",
            InternalName = "UserField2",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("182d1b9e-1718-4e11-b279-38f7ed0a20d6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Template ID], ID [bfb1589e-2016-4b98-ae62-e91979c3224f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowTemplate = new FieldDefinition
        {
            Title = "Workflow Template ID",
            InternalName = "WorkflowTemplate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("bfb1589e-2016-4b98-ae62-e91979c3224f"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Aggregated Like Count], ID [16582f9f-ba8c-42f7-8a63-9994650bb6c8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DescendantLikesCount = new FieldDefinition
        {
            Title = "Aggregated Like Count",
            InternalName = "DescendantLikesCount",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("16582f9f-ba8c-42f7-8a63-9994650bb6c8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [HTML File Link], ID [cd1ecb9f-dd4e-4f29-ab9e-e9ff40048d64] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition xd_ProgID = new FieldDefinition
        {
            Title = "HTML File Link",
            InternalName = "xd_ProgID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("cd1ecb9f-dd4e-4f29-ab9e-e9ff40048d64"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Merge], ID [e52012a0-51eb-4c0c-8dfb-9b8a0ebedcb6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Combine = new FieldDefinition
        {
            Title = "Merge",
            InternalName = "Combine",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("e52012a0-51eb-4c0c-8dfb-9b8a0ebedcb6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Warning from workbook], ID [3c6188a0-2761-4e0a-9fc0-ee32d47e4d49] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition WarningFromWorkBook = new FieldDefinition
        {
            Title = "Warning from workbook",
            InternalName = "WarningFromWorkBook",
            Description = "Whether the warning comes from the workbook or from the indicator definition",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("3c6188a0-2761-4e0a-9fc0-ee32d47e4d49"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Contact Photo], ID [1020c8a0-837a-4f1b-baa1-e35aff6da169] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition _Photo = new FieldDefinition
        {
            Title = "Contact Photo",
            InternalName = "_Photo",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("1020c8a0-837a-4f1b-baa1-e35aff6da169"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author's membership date], ID [6ab3e5a0-98f4-442e-8fff-730e653ea881] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorMemberSinceLookup = new FieldDefinition
        {
            Title = "Author's membership date",
            InternalName = "AuthorMemberSinceLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("6ab3e5a0-98f4-442e-8fff-730e653ea881"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Associated List Id], ID [b75067a2-e23b-499f-aa07-4ceb6c79e0b3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AssociatedListId = new FieldDefinition
        {
            Title = "Associated List Id",
            InternalName = "AssociatedListId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("b75067a2-e23b-499f-aa07-4ceb6c79e0b3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Schedule], ID [26761ba3-729d-4bfc-9658-77b55e01f8d5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleSchedule = new FieldDefinition
        {
            Title = "Schedule",
            InternalName = "HealthRuleSchedule",
            Description = "Determines when the rules are checked automatically",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("26761ba3-729d-4bfc-9658-77b55e01f8d5"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Suffix], ID [d886eba3-d018-4103-a322-d5780127ef8a] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Suffix = new FieldDefinition
        {
            Title = "Suffix",
            InternalName = "Suffix",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d886eba3-d018-4103-a322-d5780127ef8a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSPublishState], ID [40270da4-0a34-4c14-8c30-59e065a28a4d] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition WSPublishState = new FieldDefinition
        {
            Title = "WSPublishState",
            InternalName = "WSPublishState",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("40270da4-0a34-4c14-8c30-59e065a28a4d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Arrive Late], ID [df7f27a4-d87b-4a97-947b-13d1d4f7e6de] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Late = new FieldDefinition
        {
            Title = "Arrive Late",
            InternalName = "Late",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("df7f27a4-d87b-4a97-947b-13d1d4f7e6de"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [cbb92da4-fd46-4c7d-af6c-3128c2a5576e] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DocumentSetDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "DocumentSetDescription",
            Description = "A description of the Document Set",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("cbb92da4-fd46-4c7d-af6c-3128c2a5576e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Instance ID], ID [50a54da4-1528-4e67-954a-e2d24f1e9efb] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition InstanceID = new FieldDefinition
        {
            Title = "Instance ID",
            InternalName = "InstanceID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("50a54da4-1528-4e67-954a-e2d24f1e9efb"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [File Size], ID [78a07ba4-bda8-4357-9e0f-580d64487583] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FileSizeDisplay = new FieldDefinition
        {
            Title = "File Size",
            InternalName = "FileSizeDisplay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("78a07ba4-bda8-4357-9e0f-580d64487583"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Show in Catalog], ID [4ef69ca4-4179-4d27-9e6c-f9544d45dfdc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShowInCatalog = new FieldDefinition
        {
            Title = "Show in Catalog",
            InternalName = "ShowInCatalog",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("4ef69ca4-4179-4d27-9e6c-f9544d45dfdc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Recipients], ID [e0f298a5-7e3e-4895-9ff8-90d88ec4526d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition V4SendTo = new FieldDefinition
        {
            Title = "Recipients",
            InternalName = "V4SendTo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("e0f298a5-7e3e-4895-9ff8-90d88ec4526d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [External Participant Reason], ID [4a799ba5-f449-4796-b43e-aa5186c3c414] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition OffsiteParticipantReason = new FieldDefinition
        {
            Title = "External Participant Reason",
            InternalName = "OffsiteParticipantReason",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4a799ba5-f449-4796-b43e-aa5186c3c414"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Post Date], ID [539458a6-152c-460f-a915-53722c6eb4a6] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition LastPostDate = new FieldDefinition
        {
            Title = "Last Post Date",
            InternalName = "LastPostDate",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("539458a6-152c-460f-a915-53722c6eb4a6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Due Date], ID [c1e86ea6-7603-493c-ab5d-db4bbfe8f96a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DueDate = new FieldDefinition
        {
            Title = "Due Date",
            InternalName = "DueDate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("c1e86ea6-7603-493c-ab5d-db4bbfe8f96a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Category], ID [d8921da7-c09b-4a06-b644-dffebf73c736] and Group: [Reports]'
        /// </summary>
        public static FieldDefinition ReportCategory = new FieldDefinition
        {
            Title = "Report Category",
            InternalName = "ReportCategory",
            Description = "Category of the report",
            Group = "Reports",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("d8921da7-c09b-4a06-b644-dffebf73c736"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [People In Video], ID [bcd999a7-9dca-4824-a515-878bee641ed3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PeopleInMedia = new FieldDefinition
        {
            Title = "People In Video",
            InternalName = "PeopleInMedia",
            Description = "The people appearing in the video.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("bcd999a7-9dca-4824-a515-878bee641ed3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Car Phone], ID [92a011a9-fd1b-42e0-b6fa-afcfee1928fa] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition CarNumber = new FieldDefinition
        {
            Title = "Car Phone",
            InternalName = "CarNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("92a011a9-fd1b-42e0-b6fa-afcfee1928fa"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [References], ID [124527a9-fc10-48ff-8d44-960a7db405f8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailReferences = new FieldDefinition
        {
            Title = "References",
            InternalName = "EmailReferences",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("124527a9-fc10-48ff-8d44-960a7db405f8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [First Name Phonetic], ID [ea8f7ca9-2a0e-4a89-b8bf-c51a6af62c73] and Group: [Extended Columns]'
        /// </summary>
        public static FieldDefinition FirstNamePhonetic = new FieldDefinition
        {
            Title = "First Name Phonetic",
            InternalName = "FirstNamePhonetic",
            Description = "",
            Group = "Extended Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ea8f7ca9-2a0e-4a89-b8bf-c51a6af62c73"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Referred By], ID [9b4cc5a9-1119-43e4-b2a8-412c4031f92b] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ReferredBy = new FieldDefinition
        {
            Title = "Referred By",
            InternalName = "ReferredBy",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9b4cc5a9-1119-43e4-b2a8-412c4031f92b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Compatible Search Data Types], ID [dcb8e2a9-42d1-495f-9fda-4bf9c706bc46] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition CompatibleSearchDataTypes = new FieldDefinition
        {
            Title = "Compatible Search Data Types",
            InternalName = "CompatibleSearchDataTypes",
            Description = "Select the Search managed property data types that this Filter Display Template will be used with. If you don't enter any values, the Display Template will be available for all data types. ",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.MultiChoice,
            Id = new Guid("dcb8e2a9-42d1-495f-9fda-4bf9c706bc46"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Failing Servers], ID [84a318aa-9035-4529-98b9-e08bb20a5da0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportServers = new FieldDefinition
        {
            Title = "Failing Servers",
            InternalName = "HealthReportServers",
            Description = "The servers on which the health analyzer rule failed during the most recent execution.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("84a318aa-9035-4529-98b9-e08bb20a5da0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Edit], ID [503f1caa-358e-4918-9094-4a2cdc4bc034] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Edit = new FieldDefinition
        {
            Title = "Edit",
            InternalName = "Edit",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("503f1caa-358e-4918-9094-4a2cdc4bc034"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Remedy], ID [8aa22caa-8000-44c9-b343-a7705bbed863] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportRemedy = new FieldDefinition
        {
            Title = "Remedy",
            InternalName = "HealthReportRemedy",
            Description = "Steps which may be taken to resolve the reported problem",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("8aa22caa-8000-44c9-b343-a7705bbed863"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Multiple UI Languages], ID [fb005daa-caf9-4ecd-84d5-6bdd2eb3dce7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MUILanguages = new FieldDefinition
        {
            Title = "Multiple UI Languages",
            InternalName = "MUILanguages",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fb005daa-caf9-4ecd-84d5-6bdd2eb3dce7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Time Out], ID [fe3344ab-b468-471f-8fa5-9b506c7d1557] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Until = new FieldDefinition
        {
            Title = "Time Out",
            InternalName = "Until",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("fe3344ab-b468-471f-8fa5-9b506c7d1557"),
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
        /// Corresponds to built-in field with Title [Preview], ID [8c0d0aac-9b76-4951-927a-2490abe13c0b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PreviewOnForm = new FieldDefinition
        {
            Title = "Preview",
            InternalName = "PreviewOnForm",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("8c0d0aac-9b76-4951-927a-2490abe13c0b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Standalone], ID [d63173ac-b914-4f90-9cf8-4ff4352e41a3] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSTemplateType = new FieldDefinition
        {
            Title = "Standalone",
            InternalName = "DisplayTemplateJSTemplateType",
            Description = "Option to include this override during view selection.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("d63173ac-b914-4f90-9cf8-4ff4352e41a3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Duration], ID [80289bac-fd36-4848-b67a-bc8b5b621ec2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DLC_Duration = new FieldDefinition
        {
            Title = "Duration",
            InternalName = "DLC_Duration",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("80289bac-fd36-4848-b67a-bc8b5b621ec2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Order], ID [ca4addac-796f-4b23-b093-d2a3f65c0774] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Order = new FieldDefinition
        {
            Title = "Order",
            InternalName = "Order",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("ca4addac-796f-4b23-b093-d2a3f65c0774"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [View Style ID], ID [4630e6ac-e543-4667-935a-2cc665e9b755] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XSLStyleBaseView = new FieldDefinition
        {
            Title = "View Style ID",
            InternalName = "XSLStyleBaseView",
            Description = "The view style ID for this style",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4630e6ac-e543-4667-935a-2cc665e9b755"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Rules HREF], ID [ad97fbac-70af-4860-a078-5ee704946f93] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RulesUrl = new FieldDefinition
        {
            Title = "Workflow Rules HREF",
            InternalName = "RulesUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ad97fbac-70af-4860-a078-5ee704946f93"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indicator Goal Threshold], ID [6e4b3aad-350d-42fa-bd61-d1de715b45db] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition Goal = new FieldDefinition
        {
            Title = "Indicator Goal Threshold",
            InternalName = "Goal",
            Description = "",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("6e4b3aad-350d-42fa-bd61-d1de715b45db"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Participants], ID [8137f7ad-9170-4c1d-a17b-4ca7f557bc88] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ParticipantsPicker = new FieldDefinition
        {
            Title = "Participants",
            InternalName = "ParticipantsPicker",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("8137f7ad-9170-4c1d-a17b-4ca7f557bc88"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Condition XML], ID [ff4470ae-85d6-49ab-a501-e5772848f6c7] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingConditions = new FieldDefinition
        {
            Title = "Condition XML",
            InternalName = "RoutingConditions",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("ff4470ae-85d6-49ab-a501-e5772848f6c7"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Properties used in Conditions], ID [ff4470ae-85d6-49ab-a501-e5772848f6c8] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingConditionProperties = new FieldDefinition
        {
            Title = "Properties used in Conditions",
            InternalName = "RoutingConditionProperties",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("ff4470ae-85d6-49ab-a501-e5772848f6c8"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Rule Name], ID [7ba87dae-e90b-431b-8a02-8fc26e453880] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingRuleName = new FieldDefinition
        {
            Title = "Rule Name",
            InternalName = "RoutingRuleName",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("7ba87dae-e90b-431b-8a02-8fc26e453880"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Publisher], ID [2eedd0ae-4281-4b77-99be-68f8b3ad8a7a] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Publisher = new FieldDefinition
        {
            Title = "Publisher",
            InternalName = "_Publisher",
            Description = "The person, organization or service that published this resource",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2eedd0ae-4281-4b77-99be-68f8b3ad8a7a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thumbnail URL], ID [b9e6f3ae-5632-4b13-b636-9d1a2bd67120] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EncodedAbsThumbnailUrl = new FieldDefinition
        {
            Title = "Thumbnail URL",
            InternalName = "EncodedAbsThumbnailUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("b9e6f3ae-5632-4b13-b636-9d1a2bd67120"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Preview Image URL], ID [f39d44af-d3f3-4ae6-b43f-ac7330b5e9bd] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AlternateThumbnailUrl = new FieldDefinition
        {
            Title = "Preview Image URL",
            InternalName = "AlternateThumbnailUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("f39d44af-d3f3-4ae6-b43f-ac7330b5e9bd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [View Guid], ID [2c7db8af-02b0-4177-b77f-15c942c08427] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition ViewGuid = new FieldDefinition
        {
            Title = "View Guid",
            InternalName = "ViewGuid",
            Description = "Guid of the view",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2c7db8af-02b0-4177-b77f-15c942c08427"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Name], ID [db364cb0-8c0c-46e7-a996-684e1f2caeb2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportName = new FieldDefinition
        {
            Title = "Name",
            InternalName = "ReportName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("db364cb0-8c0c-46e7-a996-684e1f2caeb2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Update Error], ID [e247cbb0-abb3-4759-b9f4-0128e37dd34a] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition UpdateError = new FieldDefinition
        {
            Title = "Update Error",
            InternalName = "UpdateError",
            Description = "Contains the error message from last update, if any",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("e247cbb0-abb3-4759-b9f4-0128e37dd34a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Event Type], ID [20a1a5b1-fddf-4420-ac68-9701490e09af] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Event = new FieldDefinition
        {
            Title = "Event Type",
            InternalName = "Event",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("20a1a5b1-fddf-4420-ac68-9701490e09af"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Site Admin], ID [9ba260b2-85a1-4a32-ad7a-63eaceffe6b4] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IsSiteAdmin = new FieldDefinition
        {
            Title = "Is Site Admin",
            InternalName = "IsSiteAdmin",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("9ba260b2-85a1-4a32-ad7a-63eaceffe6b4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Library], ID [bda383b2-0bc3-4b10-936e-d7e48974e230] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingTargetLibrary = new FieldDefinition
        {
            Title = "Target Library",
            InternalName = "RoutingTargetLibrary",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("bda383b2-0bc3-4b10-936e-d7e48974e230"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Office], ID [26169ab2-4bd2-4870-b077-10f49c8a5822] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Office = new FieldDefinition
        {
            Title = "Office",
            InternalName = "Office",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("26169ab2-4bd2-4870-b077-10f49c8a5822"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Wiki Content], ID [c33527b4-d920-4587-b791-45024d00068a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WikiField = new FieldDefinition
        {
            Title = "Wiki Content",
            InternalName = "WikiField",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("c33527b4-d920-4587-b791-45024d00068a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Parent ID], ID [fd447db5-3908-4b47-8f8c-a5895ed0aa6a] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition ParentID = new FieldDefinition
        {
            Title = "Parent ID",
            InternalName = "ParentID",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("fd447db5-3908-4b47-8f8c-a5895ed0aa6a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Fax], ID [aad15eb6-d7fd-47b8-abd4-adc0fe33a6ba] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherFaxNumber = new FieldDefinition
        {
            Title = "Other Fax",
            InternalName = "OtherFaxNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("aad15eb6-d7fd-47b8-abd4-adc0fe33a6ba"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Subject], ID [072e9bb6-a643-44ce-b6fb-8b574a792556] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailSubject = new FieldDefinition
        {
            Title = "E-Mail Subject",
            InternalName = "EmailSubject",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("072e9bb6-a643-44ce-b6fb-8b574a792556"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSEventSourceGUID], ID [73d6d2b7-4ff3-46b4-95e1-0ab6c9b1ec9c] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSEventSourceGUID = new FieldDefinition
        {
            Title = "WSEventSourceGUID",
            InternalName = "WSEventSourceGUID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("73d6d2b7-4ff3-46b4-95e1-0ab6c9b1ec9c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSEventContextKeys], ID [4ca54ab8-9667-427e-b1ec-b1fb4a9f3e19] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSEventContextKeys = new FieldDefinition
        {
            Title = "WSEventContextKeys",
            InternalName = "WSEventContextKeys",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("4ca54ab8-9667-427e-b1ec-b1fb4a9f3e19"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Picture Size], ID [922551b8-c7e0-46a6-b7e3-3cf02917f68a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ImageSize = new FieldDefinition
        {
            Title = "Picture Size",
            InternalName = "ImageSize",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("922551b8-c7e0-46a6-b7e3-3cf02917f68a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Resolved], ID [a6fd2bb9-c701-4168-99cc-242e42f7671a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Resolved = new FieldDefinition
        {
            Title = "Resolved",
            InternalName = "Resolved",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("a6fd2bb9-c701-4168-99cc-242e42f7671a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Time In], ID [4cd541b9-c8ee-468f-bee6-33f3b9baa722] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition From = new FieldDefinition
        {
            Title = "Time In",
            InternalName = "From",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("4cd541b9-c8ee-468f-bee6-33f3b9baa722"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Embed Code], ID [ac836bb9-18e1-4f52-b614-e8885543c4c6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetEmbedCode = new FieldDefinition
        {
            Title = "Embed Code",
            InternalName = "VideoSetEmbedCode",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("ac836bb9-18e1-4f52-b614-e8885543c4c6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Version], ID [78be84b9-d70c-447b-8275-8dcd768b6f92] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Version = new FieldDefinition
        {
            Title = "Version",
            InternalName = "_Version",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("78be84b9-d70c-447b-8275-8dcd768b6f92"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Virus Status], ID [4a389cb9-54dd-4287-a71a-90ff362028bc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VirusStatus = new FieldDefinition
        {
            Title = "Virus Status",
            InternalName = "VirusStatus",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("4a389cb9-54dd-4287-a71a-90ff362028bc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Best Response Id], ID [a8b93fba-7396-443d-9884-ee332caa4560] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition BestAnswerId = new FieldDefinition
        {
            Title = "Best Response Id",
            InternalName = "BestAnswerId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("a8b93fba-7396-443d-9884-ee332caa4560"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Property for Automatic Folder Creation], ID [2a647aba-7c69-482b-97b2-dc94f2fb39dc] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingAutoFolderProp = new FieldDefinition
        {
            Title = "Property for Automatic Folder Creation",
            InternalName = "RoutingAutoFolderProp",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2a647aba-7c69-482b-97b2-dc94f2fb39dc"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Calendar Date Stamp], ID [32f182ba-284e-4a87-93c3-936a6585af39] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailCalendarDateStamp = new FieldDefinition
        {
            Title = "E-Mail Calendar Date Stamp",
            InternalName = "EmailCalendarDateStamp",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("32f182ba-284e-4a87-93c3-936a6585af39"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Modified By], ID [078b9dba-eb8c-4ec5-bfdd-8d220a3fcc5d] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition MyEditor = new FieldDefinition
        {
            Title = "Modified By",
            InternalName = "MyEditor",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("078b9dba-eb8c-4ec5-bfdd-8d220a3fcc5d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Member], ID [9e1a17bc-4b5a-498b-a0f7-e5d1ed43c349] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Member = new FieldDefinition
        {
            Title = "Member",
            InternalName = "Member",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("9e1a17bc-4b5a-498b-a0f7-e5d1ed43c349"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Version], ID [f1e020bc-ba26-443f-bf2f-b68715017bbc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowVersion = new FieldDefinition
        {
            Title = "Workflow Version",
            InternalName = "WorkflowVersion",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("f1e020bc-ba26-443f-bf2f-b68715017bbc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Address State Or Province], ID [f45883bc-8733-4b77-ab5d-43613986aa12] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherAddressStateOrProvince = new FieldDefinition
        {
            Title = "Other Address State Or Province",
            InternalName = "OtherAddressStateOrProvince",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f45883bc-8733-4b77-ab5d-43613986aa12"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Frame Height], ID [84cd09bd-85a9-461f-86e3-4c3c1738ad6b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoHeightInPixels = new FieldDefinition
        {
            Title = "Frame Height",
            InternalName = "VideoHeightInPixels",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("84cd09bd-85a9-461f-86e3-4c3c1738ad6b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Content Languages], ID [58073ebd-b204-4899-bc77-54402c61e9e9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ContentLanguages = new FieldDefinition
        {
            Title = "Content Languages",
            InternalName = "ContentLanguages",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("58073ebd-b204-4899-bc77-54402c61e9e9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-mail 3], ID [8bd27dbd-29a0-4ccd-bcb4-03fe70c538b1] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Email3 = new FieldDefinition
        {
            Title = "E-mail 3",
            InternalName = "Email3",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("8bd27dbd-29a0-4ccd-bcb4-03fe70c538b1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Principal Count], ID [dcc67ebd-247f-4bee-8626-85ff6f69fbb6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PrincipalCount = new FieldDefinition
        {
            Title = "Principal Count",
            InternalName = "PrincipalCount",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("dcc67ebd-247f-4bee-8626-85ff6f69fbb6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Less Link], ID [076193bd-865b-4de7-9633-1f12069a6fff] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LessLink = new FieldDefinition
        {
            Title = "Less Link",
            InternalName = "LessLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("076193bd-865b-4de7-9633-1f12069a6fff"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Edit Menu Table Start], ID [3c6303be-e21f-4366-80d7-d6d0a3b22c7a] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _EditMenuTableStart = new FieldDefinition
        {
            Title = "Edit Menu Table Start",
            InternalName = "_EditMenuTableStart",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("3c6303be-e21f-4366-80d7-d6d0a3b22c7a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hide Reputation], ID [501c11be-6dbf-4e6d-b322-b1882da0c8c0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HideReputation = new FieldDefinition
        {
            Title = "Hide Reputation",
            InternalName = "HideReputation",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("501c11be-6dbf-4e6d-b322-b1882da0c8c0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Parent Folder Id], ID [a9ec25bf-5a22-4658-bd19-484e52efbe1a] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition ParentFolderId = new FieldDefinition
        {
            Title = "Parent Folder Id",
            InternalName = "ParentFolderId",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("a9ec25bf-5a22-4658-bd19-484e52efbe1a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Gifted Badge Text], ID [797192bf-c571-4f18-9a85-be0acf22da05] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GiftedBadgeText = new FieldDefinition
        {
            Title = "Gifted Badge Text",
            InternalName = "GiftedBadgeText",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("797192bf-c571-4f18-9a85-be0acf22da05"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date], ID [492b1ac0-c594-4013-a2b6-ea70f5a8a506] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition V4HolidayDate = new FieldDefinition
        {
            Title = "Date",
            InternalName = "V4HolidayDate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("492b1ac0-c594-4013-a2b6-ea70f5a8a506"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Coverage], ID [3b1d59c0-26b1-4de6-abbd-3edb4e2c6eca] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Coverage = new FieldDefinition
        {
            Title = "Coverage",
            InternalName = "_Coverage",
            Description = "The extent or scope",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3b1d59c0-26b1-4de6-abbd-3edb4e2c6eca"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [File Size], ID [8fca95c0-9b7d-456f-8dae-b41ee2728b85] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition File_x0020_Size = new FieldDefinition
        {
            Title = "File Size",
            InternalName = "File_x0020_Size",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("8fca95c0-9b7d-456f-8dae-b41ee2728b85"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Root Post], ID [bd2216c1-a2f3-48c0-b21c-dc297d0cc658] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IsRootPost = new FieldDefinition
        {
            Title = "Is Root Post",
            InternalName = "IsRootPost",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("bd2216c1-a2f3-48c0-b21c-dc297d0cc658"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [HashTags], ID [333b1bc2-0532-4872-96f1-bbbdead35a56] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition HashTags = new FieldDefinition
        {
            Title = "HashTags",
            InternalName = "HashTags",
            Description = "Some description",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.TaxonomyFieldTypeMulti,
            Id = new Guid("333b1bc2-0532-4872-96f1-bbbdead35a56"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Pending Modification Time], ID [4d2444c2-0e97-476c-a2a3-e9e4a9c73009] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PendingModTime = new FieldDefinition
        {
            Title = "Pending Modification Time",
            InternalName = "PendingModTime",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("4d2444c2-0e97-476c-a2a3-e9e4a9c73009"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Order], ID [cf935cc2-a00c-4ad3-bca1-0865ab15afc1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AdminTaskOrder = new FieldDefinition
        {
            Title = "Order",
            InternalName = "AdminTaskOrder",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("cf935cc2-a00c-4ad3-bca1-0865ab15afc1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date Completed], ID [24bfa3c2-e6a0-4651-80e9-3db44bf52147] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition DateCompleted = new FieldDefinition
        {
            Title = "Date Completed",
            InternalName = "DateCompleted",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("24bfa3c2-e6a0-4651-80e9-3db44bf52147"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Due Date], ID [cd21b4c2-6841-4f9e-a23a-738a65f99889] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition TaskDueDate = new FieldDefinition
        {
            Title = "Due Date",
            InternalName = "TaskDueDate",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("cd21b4c2-6841-4f9e-a23a-738a65f99889"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Address Street], ID [dff5dfc2-e2b7-4a19-bde7-76dabc90a3d2] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherAddressStreet = new FieldDefinition
        {
            Title = "Other Address Street",
            InternalName = "OtherAddressStreet",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dff5dfc2-e2b7-4a19-bde7-76dabc90a3d2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Label], ID [fd7ef3c2-486e-40cd-b651-6be6d1abbe25] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoRenditionLabel = new FieldDefinition
        {
            Title = "Label",
            InternalName = "VideoRenditionLabel",
            Description = "The text displayed in the video player for this rendition",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("fd7ef3c2-486e-40cd-b651-6be6d1abbe25"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date], ID [335e22c3-b8a4-4234-9790-7a03eeb7b0d4] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HolidayDate = new FieldDefinition
        {
            Title = "Date",
            InternalName = "HolidayDate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("335e22c3-b8a4-4234-9790-7a03eeb7b0d4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Task Status], ID [c15b34c3-ce7d-490a-b133-3f4de8801b76] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition TaskStatus = new FieldDefinition
        {
            Title = "Task Status",
            InternalName = "TaskStatus",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("c15b34c3-ce7d-490a-b133-3f4de8801b76"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Modified By], ID [f70965c3-6ac6-4e9e-914c-3c1b4e219b6f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportModifiedBy = new FieldDefinition
        {
            Title = "Report Modified By",
            InternalName = "ReportModifiedBy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("f70965c3-6ac6-4e9e-914c-3c1b4e219b6f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Comments], ID [52578fc3-1f01-4f4d-b016-94ccbcf428cf] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Comments = new FieldDefinition
        {
            Title = "Comments",
            InternalName = "_Comments",
            Description = "A summary of this resource",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("52578fc3-1f01-4f4d-b016-94ccbcf428cf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Subject], ID [46045bc4-283a-4826-b3dd-7a78d790b266] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkDiscussionTitle = new FieldDefinition
        {
            Title = "Subject",
            InternalName = "LinkDiscussionTitle",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("46045bc4-283a-4826-b3dd-7a78d790b266"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Modified Link], ID [fc6862c4-6aac-4f08-b60e-3a8454f26040] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportModifiedLink = new FieldDefinition
        {
            Title = "Report Modified Link",
            InternalName = "ReportModifiedLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("fc6862c4-6aac-4f08-b60e-3a8454f26040"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Replies to reach next level], ID [5e74a6c4-8771-4273-88fc-682cf6839410] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NumberOfRepliesToReachNextLevel = new FieldDefinition
        {
            Title = "Replies to reach next level",
            InternalName = "NumberOfRepliesToReachNextLevel",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("5e74a6c4-8771-4273-88fc-682cf6839410"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Show Embed Link], ID [6e4ee0c4-4d06-4c04-8d02-58d10fdf912d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetShowEmbedLink = new FieldDefinition
        {
            Title = "Show Embed Link",
            InternalName = "VideoSetShowEmbedLink",
            Description = "Specifies whether a button appears on the video player that allows the user to get an embed code for the video being played.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("6e4ee0c4-4d06-4c04-8d02-58d10fdf912d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Severity], ID [505423c5-f085-48b9-9432-12073d643ba5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportSeverity = new FieldDefinition
        {
            Title = "Severity",
            InternalName = "HealthReportSeverity",
            Description = "The severity of the reported problem",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("505423c5-f085-48b9-9432-12073d643ba5"),
            Required = true
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
        /// Corresponds to built-in field with Title [Home Address Postal Code], ID [c0e4b4c6-6245-4846-8561-b8c6c01fefc1] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomeAddressPostalCode = new FieldDefinition
        {
            Title = "Home Address Postal Code",
            InternalName = "HomeAddressPostalCode",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c0e4b4c6-6245-4846-8561-b8c6c01fefc1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Topic page URL], ID [f841e7c6-0491-449f-86df-9dae475e2132] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TopicPageUrl = new FieldDefinition
        {
            Title = "Topic page URL",
            InternalName = "TopicPageUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f841e7c6-0491-449f-86df-9dae475e2132"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Template Link], ID [4b1bf6c6-4f39-45ac-acd5-16fe7a214e5e] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition TemplateUrl = new FieldDefinition
        {
            Title = "Template Link",
            InternalName = "TemplateUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4b1bf6c6-4f39-45ac-acd5-16fe7a214e5e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Printed], ID [b835f7c6-88a0-45d5-80c9-7ab4b2888b2b] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _LastPrinted = new FieldDefinition
        {
            Title = "Last Printed",
            InternalName = "_LastPrinted",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("b835f7c6-88a0-45d5-80c9-7ab4b2888b2b"),
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
        /// Corresponds to built-in field with Title [Encoded Absolute URL], ID [7177cfc7-f399-4d4d-905d-37dd51bc90bf] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EncodedAbsUrl = new FieldDefinition
        {
            Title = "Encoded Absolute URL",
            InternalName = "EncodedAbsUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("7177cfc7-f399-4d4d-905d-37dd51bc90bf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Discussion Subject], ID [c5abfdc7-3435-4183-9207-3d1146895cf8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DiscussionTitle = new FieldDefinition
        {
            Title = "Discussion Subject",
            InternalName = "DiscussionTitle",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("c5abfdc7-3435-4183-9207-3d1146895cf8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Parent ID], ID [1be428c8-2c2d-4e02-970b-6663eb1d7080] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ParentId = new FieldDefinition
        {
            Title = "Parent ID",
            InternalName = "ParentId",
            Description = "The Parent Id of this report",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("1be428c8-2c2d-4e02-970b-6663eb1d7080"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Title], ID [82642ec8-ef9b-478f-acf9-31f7d45fbc31] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition LinkTitle = new FieldDefinition
        {
            Title = "Title",
            InternalName = "LinkTitle",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("82642ec8-ef9b-478f-acf9-31f7d45fbc31"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Activity], ID [cba948c8-9e42-44a0-b9f1-a39d91b28cb0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LastActivity = new FieldDefinition
        {
            Title = "Last Activity",
            InternalName = "LastActivity",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("cba948c8-9e42-44a0-b9f1-a39d91b28cb0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [WSEventType], ID [b4ba57c8-ab73-49fa-b6af-a4f824d84c14] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WSEventType = new FieldDefinition
        {
            Title = "WSEventType",
            InternalName = "WSEventType",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("b4ba57c8-ab73-49fa-b6af-a4f824d84c14"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Modified], ID [173f76c8-aebd-446a-9bc9-769a2bd2c18f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Last_x0020_Modified = new FieldDefinition
        {
            Title = "Modified",
            InternalName = "Last_x0020_Modified",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("173f76c8-aebd-446a-9bc9-769a2bd2c18f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Fax Number], ID [9d1cacc8-f452-4bc1-a751-050595ad96e1] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkFax = new FieldDefinition
        {
            Title = "Fax Number",
            InternalName = "WorkFax",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("9d1cacc8-f452-4bc1-a751-050595ad96e1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-mail 2], ID [e232d6c8-9f49-4be2-bb28-b90570bcf167] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Email2 = new FieldDefinition
        {
            Title = "E-mail 2",
            InternalName = "Email2",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e232d6c8-9f49-4be2-bb28-b90570bcf167"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Scope], ID [e59f08c9-fa34-4f94-a00a-f6458b1d3c56] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleScope = new FieldDefinition
        {
            Title = "Scope",
            InternalName = "HealthRuleScope",
            Description = "The rule scope determines where the rule file is run. If the rule scope is set to All, the rule file will run on all computers with the specified service. If the rule scope is set to Any, the rule file will run on the first available computer with the specified service.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("e59f08c9-fa34-4f94-a00a-f6458b1d3c56"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Government ID Number], ID [da31d3c9-f9da-4c35-88d4-60aafa4c3f19] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition GovernmentIDNumber = new FieldDefinition
        {
            Title = "Government ID Number",
            InternalName = "GovernmentIDNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("da31d3c9-f9da-4c35-88d4-60aafa4c3f19"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Selection Checkbox], ID [7ebf72ca-a307-4c18-9e5b-9d89e1dae74f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SelectedFlag = new FieldDefinition
        {
            Title = "Selection Checkbox",
            InternalName = "SelectedFlag",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("7ebf72ca-a307-4c18-9e5b-9d89e1dae74f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User Field 4], ID [adefa4ca-14c3-4694-b531-f51b706efe9d] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition UserField4 = new FieldDefinition
        {
            Title = "User Field 4",
            InternalName = "UserField4",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("adefa4ca-14c3-4694-b531-f51b706efe9d"),
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
        /// Corresponds to built-in field with Title [Mobile Number], ID [bf03d3ca-aa6e-4845-809a-b4378b37ce08] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition MobilePhone = new FieldDefinition
        {
            Title = "Mobile Number",
            InternalName = "MobilePhone",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("bf03d3ca-aa6e-4845-809a-b4378b37ce08"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workflow Name], ID [e506d6ca-c2da-4164-b858-306f1c41c9ec] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition WorkflowName = new FieldDefinition
        {
            Title = "Workflow Name",
            InternalName = "WorkflowName",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e506d6ca-c2da-4164-b858-306f1c41c9ec"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Icon], ID [57468ccb-0c02-422c-ba0a-61a44ba41784] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSIconUrl = new FieldDefinition
        {
            Title = "Icon",
            InternalName = "DisplayTemplateJSIconUrl",
            Description = "Icon to be displayed for this override.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("57468ccb-0c02-422c-ba0a-61a44ba41784"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Rights Management], ID [ada3f0cb-6f95-4588-bb08-d97cc0623522] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _RightsManagement = new FieldDefinition
        {
            Title = "Rights Management",
            InternalName = "_RightsManagement",
            Description = "Information about rights held in or over this resource",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("ada3f0cb-6f95-4588-bb08-d97cc0623522"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Messages], ID [9161f6cb-a8e6-47b8-9d24-89415de691f7] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RelevantMessages = new FieldDefinition
        {
            Title = "E-Mail Messages",
            InternalName = "RelevantMessages",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("9161f6cb-a8e6-47b8-9d24-89415de691f7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Parent Item Editor], ID [ff90fecb-7f46-44f5-9698-db44a81b2a8b] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition ParentItemEditor = new FieldDefinition
        {
            Title = "Parent Item Editor",
            InternalName = "ParentItemEditor",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("ff90fecb-7f46-44f5-9698-db44a81b2a8b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date], ID [2139e5cc-6c75-4a65-b84c-00fe93027db3] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Date = new FieldDefinition
        {
            Title = "Date",
            InternalName = "Date",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("2139e5cc-6c75-4a65-b84c-00fe93027db3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Preview Exists], ID [3ca8efcd-96e8-414f-ba90-4c8c4a8bfef8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PreviewExists = new FieldDefinition
        {
            Title = "Preview Exists",
            InternalName = "PreviewExists",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("3ca8efcd-96e8-414f-ba90-4c8c4a8bfef8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Server Relative URL], ID [105f76ce-724a-4bba-aece-f81f2fce58f5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ServerUrl = new FieldDefinition
        {
            Title = "Server Relative URL",
            InternalName = "ServerUrl",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("105f76ce-724a-4bba-aece-f81f2fce58f5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [0fc9cace-c5c2-465d-ae88-b67f2964ca93] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Category = new FieldDefinition
        {
            Title = "Category",
            InternalName = "_Category",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("0fc9cace-c5c2-465d-ae88-b67f2964ca93"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [7fc04acf-6b4f-418c-8dc5-ecfb0085bb51] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition GbwCategory = new FieldDefinition
        {
            Title = "Category",
            InternalName = "GbwCategory",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("7fc04acf-6b4f-418c-8dc5-ecfb0085bb51"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Check Double Booking], ID [d8cd5bcf-3768-4d6c-a8aa-fefa3c793d8d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Overbook = new FieldDefinition
        {
            Title = "Check Double Booking",
            InternalName = "Overbook",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Overbook,
            Id = new Guid("d8cd5bcf-3768-4d6c-a8aa-fefa3c793d8d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Form Name], ID [66b691cf-07a3-4ca6-ac6d-27fa969c8569] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FormName = new FieldDefinition
        {
            Title = "Form Name",
            InternalName = "FormName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("66b691cf-07a3-4ca6-ac6d-27fa969c8569"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Department], ID [c814b2cf-84c6-4f56-b4a4-c766938a97c5] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ol_Department = new FieldDefinition
        {
            Title = "Department",
            InternalName = "ol_Department",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("c814b2cf-84c6-4f56-b4a4-c766938a97c5"),
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
        /// Corresponds to built-in field with Title [Workflow Instance ID], ID [de8beacf-5505-47cd-80a6-aa44e7ffe2f4] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowInstanceID = new FieldDefinition
        {
            Title = "Workflow Instance ID",
            InternalName = "WorkflowInstanceID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Guid,
            Id = new Guid("de8beacf-5505-47cd-80a6-aa44e7ffe2f4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Service], ID [2d6e61d0-be31-460c-ab8b-77d8b369f517] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthRuleService = new FieldDefinition
        {
            Title = "Service",
            InternalName = "HealthRuleService",
            Description = "The rule service determines where the rule file is run. The rule file will run only on computers with running instances of the service specified by the rule service. If the rule service is left blank, this rule will run on all computers.",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2d6e61d0-be31-460c-ab8b-77d8b369f517"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [RecurrenceData], ID [d12572d0-0a1e-4438-89b5-4d0430be7603] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RecurrenceData = new FieldDefinition
        {
            Title = "RecurrenceData",
            InternalName = "RecurrenceData",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("d12572d0-0a1e-4438-89b5-4d0430be7603"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Primary Item ID], ID [92b8e9d0-a11b-418f-bf1c-c44aaa73075d] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Item = new FieldDefinition
        {
            Title = "Primary Item ID",
            InternalName = "Item",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("92b8e9d0-a11b-418f-bf1c-c44aaa73075d"),
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
        /// Corresponds to built-in field with Title [External Link], ID [1c2cc9d2-3c9f-4a46-8088-17287d608270] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetExternalLink = new FieldDefinition
        {
            Title = "External Link",
            InternalName = "VideoSetExternalLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("1c2cc9d2-3c9f-4a46-8088-17287d608270"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Web Page], ID [a71affd2-dcc7-4529-81bc-2fe593154a5f] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WebPage = new FieldDefinition
        {
            Title = "Web Page",
            InternalName = "WebPage",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("a71affd2-dcc7-4529-81bc-2fe593154a5f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Submission Content Type], ID [592102d3-4bce-4640-a49e-b6f23d480b7d] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingContentType = new FieldDefinition
        {
            Title = "Submission Content Type",
            InternalName = "RoutingContentType",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("592102d3-4bce-4640-a49e-b6f23d480b7d"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [State/Province], ID [ceac61d3-dda9-468b-b276-f4a6bb93f14f] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition WorkState = new FieldDefinition
        {
            Title = "State/Province",
            InternalName = "WorkState",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ceac61d3-dda9-468b-b276-f4a6bb93f14f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Confirmations], ID [ef7465d3-5d54-487b-b081-ade80acae88e] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Confirmations = new FieldDefinition
        {
            Title = "Confirmations",
            InternalName = "Confirmations",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ef7465d3-5d54-487b-b081-ade80acae88e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hours Worked], ID [3bdf7bd3-f229-419e-8e12-3dfecb49ed38] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ScheduledWork = new FieldDefinition
        {
            Title = "Hours Worked",
            InternalName = "ScheduledWork",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3bdf7bd3-f229-419e-8e12-3dfecb49ed38"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [URL], ID [2a9ab6d3-268a-4c1c-9897-e5f018f87e64] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition URLwMenu = new FieldDefinition
        {
            Title = "URL",
            InternalName = "URLwMenu",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("2a9ab6d3-268a-4c1c-9897-e5f018f87e64"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [First Name], ID [4a722dd4-d406-4356-93f9-2550b8f50dd0] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition FirstName = new FieldDefinition
        {
            Title = "First Name",
            InternalName = "FirstName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4a722dd4-d406-4356-93f9-2550b8f50dd0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Return], ID [ee394fd4-4c11-4d8e-baff-83270c1921aa] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition In = new FieldDefinition
        {
            Title = "Return",
            InternalName = "In",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ee394fd4-4c11-4d8e-baff-83270c1921aa"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [XMLTZone], ID [c4b72ed6-45aa-4422-bff1-2b6750d30819] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XMLTZone = new FieldDefinition
        {
            Title = "XMLTZone",
            InternalName = "XMLTZone",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("c4b72ed6-45aa-4422-bff1-2b6750d30819"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Nickname], ID [6b0a2cd7-a7f9-41ca-b932-f3bebb603793] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition Nickname = new FieldDefinition
        {
            Title = "Nickname",
            InternalName = "Nickname",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("6b0a2cd7-a7f9-41ca-b932-f3bebb603793"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home Address Country/Region], ID [897ecfd7-4293-4782-b463-bd68440a5fed] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomeAddressCountry = new FieldDefinition
        {
            Title = "Home Address Country/Region",
            InternalName = "HomeAddressCountry",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("897ecfd7-4293-4782-b463-bd68440a5fed"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Taxonomy Catch All Column1], ID [8f6b6dd8-9357-4019-8172-966fcd502ed2] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition TaxCatchAllLabel = new FieldDefinition
        {
            Title = "Taxonomy Catch All Column1",
            InternalName = "TaxCatchAllLabel",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("8f6b6dd8-9357-4019-8172-966fcd502ed2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Personal Website], ID [5aa071d9-3254-40fb-82df-5cedeff0c41e] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition PersonalWebsite = new FieldDefinition
        {
            Title = "Personal Website",
            InternalName = "PersonalWebsite",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("5aa071d9-3254-40fb-82df-5cedeff0c41e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thread Topic], ID [769b99d9-d361-4948-b687-f01332391629] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ThreadTopic = new FieldDefinition
        {
            Title = "Thread Topic",
            InternalName = "ThreadTopic",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("769b99d9-d361-4948-b687-f01332391629"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Severity], ID [89efcbd9-9796-41f0-b569-65325f1882dc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportSeverityIcon = new FieldDefinition
        {
            Title = "Severity",
            InternalName = "HealthReportSeverityIcon",
            Description = "The severity of the reported problem",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("89efcbd9-9796-41f0-b569-65325f1882dc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Owner], ID [2e8881da-0332-4ad9-a565-45b5b8b2702f] and Group: [Reports]'
        /// </summary>
        public static FieldDefinition ReportOwner = new FieldDefinition
        {
            Title = "Owner",
            InternalName = "ReportOwner",
            Description = "Owner of this document",
            Group = "Reports",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("2e8881da-0332-4ad9-a565-45b5b8b2702f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Event Address], ID [493896da-0a4f-46ec-a68e-9cfd1a5fc19b] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition ol_EventAddress = new FieldDefinition
        {
            Title = "Event Address",
            InternalName = "ol_EventAddress",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("493896da-0a4f-46ec-a68e-9cfd1a5fc19b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Indicator], ID [291aacda-4cfb-4d59-968b-5e2ea0c9eab7] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition KPI = new FieldDefinition
        {
            Title = "Indicator",
            InternalName = "KPI",
            Description = "Name of Microsoft SQL Server 2005 Analysis Services KPI",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("291aacda-4cfb-4d59-968b-5e2ea0c9eab7"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Active], ID [af5036db-36f4-46c8-bde7-a677bd0ef280] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IsActive = new FieldDefinition
        {
            Title = "Is Active",
            InternalName = "IsActive",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("af5036db-36f4-46c8-bde7-a677bd0ef280"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Source], ID [b0a3c1db-faf1-48f0-9be1-47d2fc8cb5d6] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Source = new FieldDefinition
        {
            Title = "Source",
            InternalName = "_Source",
            Description = "References to resources from which this resource was derived",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("b0a3c1db-faf1-48f0-9be1-47d2fc8cb5d6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Format], ID [36111fdd-2c65-41ac-b7ef-48b9b8da4526] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _Format = new FieldDefinition
        {
            Title = "Format",
            InternalName = "_Format",
            Description = "Media-type, file format or dimensions",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("36111fdd-2c65-41ac-b7ef-48b9b8da4526"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Day], ID [61fc45dd-b33d-4679-8646-be9e6584fadd] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition DayOfWeek = new FieldDefinition
        {
            Title = "Day",
            InternalName = "DayOfWeek",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("61fc45dd-b33d-4679-8646-be9e6584fadd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author's membership status], ID [c42ab4dd-eb3a-4df5-9c80-0bd400520c15] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorMemberStatusIntLookup = new FieldDefinition
        {
            Title = "Author's membership status",
            InternalName = "AuthorMemberStatusIntLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("c42ab4dd-eb3a-4df5-9c80-0bd400520c15"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Unique Id], ID [4b7403de-8d94-43e8-9f0f-137a3e298126] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition UniqueId = new FieldDefinition
        {
            Title = "Unique Id",
            InternalName = "UniqueId",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("4b7403de-8d94-43e8-9f0f-137a3e298126"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Spouse/Domestic Partner], ID [f590b1de-8e28-4c17-91bc-bf4096024b7e] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition SpouseName = new FieldDefinition
        {
            Title = "Spouse/Domestic Partner",
            InternalName = "SpouseName",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f590b1de-8e28-4c17-91bc-bf4096024b7e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Last Post By], ID [497e00df-75c8-4e61-ac5c-a143b6a0fddc] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition LastPostBy = new FieldDefinition
        {
            Title = "Last Post By",
            InternalName = "LastPostBy",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("497e00df-75c8-4e61-ac5c-a143b6a0fddc"),
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
        /// Corresponds to built-in field with Title [Deleted], ID [4ed6dfdf-86a8-4894-bd1b-4fa28042be53] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Deleted = new FieldDefinition
        {
            Title = "Deleted",
            InternalName = "Deleted",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("4ed6dfdf-86a8-4894-bd1b-4fa28042be53"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Date Created], ID [9f8b4ee0-84b7-42c6-a094-5cbde2115eb9] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition _DCDateCreated = new FieldDefinition
        {
            Title = "Date Created",
            InternalName = "_DCDateCreated",
            Description = "The date on which this resource was created",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("9f8b4ee0-84b7-42c6-a094-5cbde2115eb9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Renditions Information], ID [f1393ce1-ac10-4696-987d-cfdcc40ad342] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetRenditionsInfo = new FieldDefinition
        {
            Title = "Renditions Information",
            InternalName = "VideoSetRenditionsInfo",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("f1393ce1-ac10-4696-987d-cfdcc40ad342"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thumbnail Time Index], ID [e4cd7ce1-9e29-497b-886e-619e5442acad] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetThumbnailTimeIndex = new FieldDefinition
        {
            Title = "Thumbnail Time Index",
            InternalName = "VideoSetThumbnailTimeIndex",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("e4cd7ce1-9e29-497b-886e-619e5442acad"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Created], ID [fef4b2e1-4b89-4929-981b-c1967e0b3178] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportCreated = new FieldDefinition
        {
            Title = "Report Created",
            InternalName = "ReportCreated",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("fef4b2e1-4b89-4929-981b-c1967e0b3178"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Resources], ID [a4e7b3e1-1b0a-4ffa-8426-c94d4cb8cc57] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Facilities = new FieldDefinition
        {
            Title = "Resources",
            InternalName = "Facilities",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Facilities,
            Id = new Guid("a4e7b3e1-1b0a-4ffa-8426-c94d4cb8cc57"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Extended Properties], ID [1c5518e2-1e99-49fe-bfc6-1a8de3ba16e2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ExtendedProperties = new FieldDefinition
        {
            Title = "Extended Properties",
            InternalName = "ExtendedProperties",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("1c5518e2-1e99-49fe-bfc6-1a8de3ba16e2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reply], ID [87cda0e2-fc57-4eec-a696-b0de2f61f361] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReplyNoGif = new FieldDefinition
        {
            Title = "Reply",
            InternalName = "ReplyNoGif",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("87cda0e2-fc57-4eec-a696-b0de2f61f361"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Document Modified By], ID [822c78e3-1ea9-4943-b449-57863ad33ca9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Modified_x0020_By = new FieldDefinition
        {
            Title = "Document Modified By",
            InternalName = "Modified_x0020_By",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("822c78e3-1ea9-4943-b449-57863ad33ca9"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Action], ID [7b016ee5-70aa-4abb-8aa3-01795b4efe6f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AdminTaskAction = new FieldDefinition
        {
            Title = "Action",
            InternalName = "AdminTaskAction",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("7b016ee5-70aa-4abb-8aa3-01795b4efe6f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Scope], ID [df8bd7e5-b3db-4a94-afb4-7296397d829d] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSTargetScope = new FieldDefinition
        {
            Title = "Target Scope",
            InternalName = "DisplayTemplateJSTargetScope",
            Description = "URL of the website this override applies to.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("df8bd7e5-b3db-4a94-afb4-7296397d829d"),
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
        /// Corresponds to built-in field with Title [Is Current Version], ID [c101c3e7-122d-4d4d-bc34-58e94a38c816] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _IsCurrentVersion = new FieldDefinition
        {
            Title = "Is Current Version",
            InternalName = "_IsCurrentVersion",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("c101c3e7-122d-4d4d-bc34-58e94a38c816"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [3f44dee7-b4ba-4e0f-9a4c-84f4420dfaf6] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition CategoriesLookup = new FieldDefinition
        {
            Title = "Category",
            InternalName = "CategoriesLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("3f44dee7-b4ba-4e0f-9a4c-84f4420dfaf6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [HashTags_0], ID [790722e8-f13c-4f84-90b2-36161bfe7873] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition j33b1bc20532487296f1bbbdead35a56 = new FieldDefinition
        {
            Title = "HashTags_0",
            InternalName = "j33b1bc20532487296f1bbbdead35a56",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("790722e8-f13c-4f84-90b2-36161bfe7873"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Language], ID [d81529e8-384c-4ca6-9c43-c86a256e6a44] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Language = new FieldDefinition
        {
            Title = "Language",
            InternalName = "Language",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("d81529e8-384c-4ca6-9c43-c86a256e6a44"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Shared File Index], ID [034998e9-bf1c-4288-bbbd-00eacfc64410] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _SharedFileIndex = new FieldDefinition
        {
            Title = "Shared File Index",
            InternalName = "_SharedFileIndex",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("034998e9-bf1c-4288-bbbd-00eacfc64410"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Formatted indicator value], ID [3366aae9-30a9-43f5-a2cb-1a6ee44e2ce4] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition FormattedValue = new FieldDefinition
        {
            Title = "Formatted indicator value",
            InternalName = "FormattedValue",
            Description = "The value of the indicator, formatted by its datasource provider",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3366aae9-30a9-43f5-a2cb-1a6ee44e2ce4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Approver Comments], ID [34ad21eb-75bd-4544-8c73-0e08330291fe] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _ModerationComments = new FieldDefinition
        {
            Title = "Approver Comments",
            InternalName = "_ModerationComments",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("34ad21eb-75bd-4544-8c73-0e08330291fe"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Home Phone], ID [2ab923eb-9880-4b47-9965-ebf93ae15487] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition HomePhone = new FieldDefinition
        {
            Title = "Home Phone",
            InternalName = "HomePhone",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2ab923eb-9880-4b47-9965-ebf93ae15487"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Liked By], ID [2cdcd5eb-846d-4f4d-9aaf-73e8e73c7312] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LikedBy = new FieldDefinition
        {
            Title = "Liked By",
            InternalName = "LikedBy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("2cdcd5eb-846d-4f4d-9aaf-73e8e73c7312"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Overtime on Holiday], ID [dc9100ec-251d-4e81-a6cb-d967a065ba24] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HolidayNightWork = new FieldDefinition
        {
            Title = "Overtime on Holiday",
            InternalName = "HolidayNightWork",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dc9100ec-251d-4e81-a6cb-d967a065ba24"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Include child indicators], ID [f6df49ec-807b-4d40-a7da-a17684121f92] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition IncludeHierarchy = new FieldDefinition
        {
            Title = "Include child indicators",
            InternalName = "IncludeHierarchy",
            Description = "To include child KPIs or not",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("f6df49ec-807b-4d40-a7da-a17684121f92"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Description], ID [b76b58ec-0549-4f00-9575-2fd28bd55010] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition VideoSetDescription = new FieldDefinition
        {
            Title = "Description",
            InternalName = "VideoSetDescription",
            Description = "A summary of the Video",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("b76b58ec-0549-4f00-9575-2fd28bd55010"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Explanation], ID [b4c8faec-5d60-49ee-a5fb-6165f5c3e6a9] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportExplanation = new FieldDefinition
        {
            Title = "Explanation",
            InternalName = "HealthReportExplanation",
            Description = "A detailed explanation of the reported problem",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("b4c8faec-5d60-49ee-a5fb-6165f5c3e6a9"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Auto Update], ID [96226eed-ec6f-4f0e-add5-9cfe66a441a0] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition AutoUpdate = new FieldDefinition
        {
            Title = "Auto Update",
            InternalName = "AutoUpdate",
            Description = "Whether to fetch the backend data every time",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("96226eed-ec6f-4f0e-add5-9cfe66a441a0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Display Folder], ID [dd83a5ed-83c5-47b7-823a-415c6ea1b8a3] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition DisplayFolder = new FieldDefinition
        {
            Title = "Display Folder",
            InternalName = "DisplayFolder",
            Description = "The Display Folder of the Indicator",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dd83a5ed-83c5-47b7-823a-415c6ea1b8a3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Approval Status], ID [fdc3b2ed-5bf2-4835-a4bc-b885f3396a61] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _ModerationStatus = new FieldDefinition
        {
            Title = "Approval Status",
            InternalName = "_ModerationStatus",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.ModStat,
            Id = new Guid("fdc3b2ed-5bf2-4835-a4bc-b885f3396a61"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [URL], ID [aeaf07ee-d2fb-448b-a7a3-cf7e062d6c2a] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition URLNoMenu = new FieldDefinition
        {
            Title = "URL",
            InternalName = "URLNoMenu",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("aeaf07ee-d2fb-448b-a7a3-cf7e062d6c2a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Author's Reputation Score], ID [559f0dee-4484-4c86-a699-82d122b63717] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AuthorReputationLookup = new FieldDefinition
        {
            Title = "Author's Reputation Score",
            InternalName = "AuthorReputationLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("559f0dee-4484-4c86-a699-82d122b63717"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Effective Permissions Mask], ID [ba3c27ee-4791-4867-8821-ff99000bac98] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PermMask = new FieldDefinition
        {
            Title = "Effective Permissions Mask",
            InternalName = "PermMask",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("ba3c27ee-4791-4867-8821-ff99000bac98"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Posted By], ID [adfe65ee-74bb-4771-bec5-d691d9a6a14e] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition PersonImage = new FieldDefinition
        {
            Title = "Posted By",
            InternalName = "PersonImage",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("adfe65ee-74bb-4771-bec5-d691d9a6a14e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Edit Menu Table End], ID [2ea78cef-1bf9-4019-960a-02c41636cb47] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition _EditMenuTableEnd = new FieldDefinition
        {
            Title = "Edit Menu Table End",
            InternalName = "_EditMenuTableEnd",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("2ea78cef-1bf9-4019-960a-02c41636cb47"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Replies], ID [d42630f0-0084-4b16-b876-80fe8cf88879] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition ReplyCount = new FieldDefinition
        {
            Title = "Replies",
            InternalName = "ReplyCount",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("d42630f0-0084-4b16-b876-80fe8cf88879"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hidden Template], ID [0a9ec8f0-0340-4e24-9b35-ca86a6ded5ab] and Group: [Display Template Columns]'
        /// </summary>
        public static FieldDefinition TemplateHidden = new FieldDefinition
        {
            Title = "Hidden Template",
            InternalName = "TemplateHidden",
            Description = "Hide this Display Template where people select from an available list of search Display Templates.",
            Group = "Display Template Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("0a9ec8f0-0340-4e24-9b35-ca86a6ded5ab"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Popularity], ID [898232f1-83c0-41df-9f1a-64b08a03f62d] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Popularity = new FieldDefinition
        {
            Title = "Popularity",
            InternalName = "Popularity",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("898232f1-83c0-41df-9f1a-64b08a03f62d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Thread Index], ID [cef73bf1-edf6-4dd9-9098-a07d83984700] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ThreadIndex = new FieldDefinition
        {
            Title = "Thread Index",
            InternalName = "ThreadIndex",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.ThreadIndex,
            Id = new Guid("cef73bf1-edf6-4dd9-9098-a07d83984700"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Discussions], ID [178d4af1-459b-4f61-bb41-b347986ee37b] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition NumberOfDiscussions = new FieldDefinition
        {
            Title = "Discussions",
            InternalName = "NumberOfDiscussions",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Number,
            Id = new Guid("178d4af1-459b-4f61-bb41-b347986ee37b"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Mobile Number], ID [2a464df1-44c1-4851-949d-fcd270f0ccf2] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition CellPhone = new FieldDefinition
        {
            Title = "Mobile Number",
            InternalName = "CellPhone",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("2a464df1-44c1-4851-949d-fcd270f0ccf2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category Picture], ID [7cc564f1-abd4-4a2f-bd9b-85dd1d071bdc] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition CategoryImage = new FieldDefinition
        {
            Title = "Category Picture",
            InternalName = "CategoryImage",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.URL,
            Id = new Guid("7cc564f1-abd4-4a2f-bd9b-85dd1d071bdc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [TTY-TDD Phone], ID [f54697f1-0357-4c5a-a711-0cb654bc73e4] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition TTYTDDNumber = new FieldDefinition
        {
            Title = "TTY-TDD Phone",
            InternalName = "TTYTDDNumber",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("f54697f1-0357-4c5a-a711-0cb654bc73e4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Role], ID [eeaeaaf1-4110-465b-905e-df1073a7e0e6] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition Role = new FieldDefinition
        {
            Title = "Role",
            InternalName = "Role",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("eeaeaaf1-4110-465b-905e-df1073a7e0e6"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Mileage], ID [3126c2f1-063e-4892-828f-0696ec6e105f] and Group: [Core Task and Issue Columns]'
        /// </summary>
        public static FieldDefinition Mileage = new FieldDefinition
        {
            Title = "Mileage",
            InternalName = "Mileage",
            Description = "",
            Group = "Core Task and Issue Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("3126c2f1-063e-4892-828f-0696ec6e105f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [a63505f2-f42c-4d94-b03b-78ba2c73d40e] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition HealthReportCategory = new FieldDefinition
        {
            Title = "Category",
            InternalName = "HealthReportCategory",
            Description = "The category of the reported problem",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("a63505f2-f42c-4d94-b03b-78ba2c73d40e"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Send E-mail Notification], ID [cb2413f2-7de9-4afc-8587-1ca3f563f624] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SendEmailNotification = new FieldDefinition
        {
            Title = "Send E-mail Notification",
            InternalName = "SendEmailNotification",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("cb2413f2-7de9-4afc-8587-1ca3f563f624"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [End Time], ID [2684f9f2-54be-429f-ba06-76754fc056bf] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EndDate = new FieldDefinition
        {
            Title = "End Time",
            InternalName = "EndDate",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("2684f9f2-54be-429f-ba06-76754fc056bf"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [App Modified By], ID [e08400f3-c779-4ed2-a18c-ab7f34caa318] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AppEditor = new FieldDefinition
        {
            Title = "App Modified By",
            InternalName = "AppEditor",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("e08400f3-c779-4ed2-a18c-ab7f34caa318"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [File Type], ID [c53a03f3-f930-4ef2-b166-e0f2210c13c0] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition FileType = new FieldDefinition
        {
            Title = "File Type",
            InternalName = "FileType",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("c53a03f3-f930-4ef2-b166-e0f2210c13c0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Value Sheet], ID [d096d5f3-b399-462b-9a32-83f82d9237d4] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition ValueSheet = new FieldDefinition
        {
            Title = "Value Sheet",
            InternalName = "ValueSheet",
            Description = "Name of the Sheet for the value",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("d096d5f3-b399-462b-9a32-83f82d9237d4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Headers], ID [e6985df4-cf66-4313-bcda-d89744d3b02f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailHeaders = new FieldDefinition
        {
            Title = "E-Mail Headers",
            InternalName = "EmailHeaders",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("e6985df4-cf66-4313-bcda-d89744d3b02f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Cc], ID [a6af6df4-feb5-4dbf-bef6-d81230d4a071] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailCc = new FieldDefinition
        {
            Title = "E-Mail Cc",
            InternalName = "EmailCc",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("a6af6df4-feb5-4dbf-bef6-d81230d4a071"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Formatted indicator goal], ID [e9df93f4-7951-474b-8cc4-e240c2f5e600] and Group: [Status Indicators]'
        /// </summary>
        public static FieldDefinition FormattedGoal = new FieldDefinition
        {
            Title = "Formatted indicator goal",
            InternalName = "FormattedGoal",
            Description = "The goal of the indicator, formatted by its datasource provider",
            Group = "Status Indicators",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e9df93f4-7951-474b-8cc4-e240c2f5e600"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Attachments], ID [67df98f4-9dec-48ff-a553-29bece9c5bf4] and Group: [Base Columns]'
        /// </summary>
        public static FieldDefinition Attachments = new FieldDefinition
        {
            Title = "Attachments",
            InternalName = "Attachments",
            Description = "",
            Group = "Base Columns",
            FieldType = BuiltInFieldTypes.Attachments,
            Id = new Guid("67df98f4-9dec-48ff-a553-29bece9c5bf4"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [All Day Event], ID [7d95d1f4-f5fd-4a70-90cd-b35abc9b5bc8] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition fAllDayEvent = new FieldDefinition
        {
            Title = "All Day Event",
            InternalName = "fAllDayEvent",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.AllDayEvent,
            Id = new Guid("7d95d1f4-f5fd-4a70-90cd-b35abc9b5bc8"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [History], ID [27c603f5-4dbe-4522-894a-ae77715dc532] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportHistoryLink = new FieldDefinition
        {
            Title = "History",
            InternalName = "ReportHistoryLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("27c603f5-4dbe-4522-894a-ae77715dc532"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User Field 1], ID [566656f5-17b3-4291-98a5-5074aadf77b3] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition UserField1 = new FieldDefinition
        {
            Title = "User Field 1",
            InternalName = "UserField1",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("566656f5-17b3-4291-98a5-5074aadf77b3"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Path], ID [56605df6-8fa1-47e4-a04c-5b384d59609f] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FileDirRef = new FieldDefinition
        {
            Title = "Path",
            InternalName = "FileDirRef",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("56605df6-8fa1-47e4-a04c-5b384d59609f"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Aliases], ID [186175f6-e318-4e9a-b5f7-4f7c751585a0] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingAliases = new FieldDefinition
        {
            Title = "Aliases",
            InternalName = "RoutingAliases",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("186175f6-e318-4e9a-b5f7-4f7c751585a0"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Content Type ID], ID [ed095cf7-534e-460b-965f-f14269e70f5a] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSTargetContentType = new FieldDefinition
        {
            Title = "Target Content Type ID",
            InternalName = "DisplayTemplateJSTargetContentType",
            Description = "ID of the content type this override applies to.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("ed095cf7-534e-460b-965f-f14269e70f5a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Hidden], ID [3d0684f7-ca97-413d-9d03-d00f480059ae] and Group: [JavaScript Display Template Columns]'
        /// </summary>
        public static FieldDefinition DisplayTemplateJSTemplateHidden = new FieldDefinition
        {
            Title = "Hidden",
            InternalName = "DisplayTemplateJSTemplateHidden",
            Description = "Hide this Display Template option when creating new views.",
            Group = "JavaScript Display Template Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("3d0684f7-ca97-413d-9d03-d00f480059ae"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [IM Address], ID [4cbd96f7-09c6-4b5e-ad42-1cbe123de63a] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition IMAddress = new FieldDefinition
        {
            Title = "IM Address",
            InternalName = "IMAddress",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("4cbd96f7-09c6-4b5e-ad42-1cbe123de63a"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Target Path], ID [10a4d7f7-ab3a-426f-b5cc-ab1eb03a94f4] and Group: [Document and Record Management Columns]'
        /// </summary>
        public static FieldDefinition RoutingTargetPath = new FieldDefinition
        {
            Title = "Target Path",
            InternalName = "RoutingTargetPath",
            Description = "",
            Group = "Document and Record Management Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("10a4d7f7-ab3a-426f-b5cc-ab1eb03a94f4"),
            Required = true
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Is Featured Discussion], ID [5a034ff8-d7a4-4d69-ab26-5f5a043b572d] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition IsFeatured = new FieldDefinition
        {
            Title = "Is Featured Discussion",
            InternalName = "IsFeatured",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Boolean,
            Id = new Guid("5a034ff8-d7a4-4d69-ab26-5f5a043b572d"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Sort Type], ID [423874f8-c300-4bfb-b7a1-42e2159e3b19] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SortBehavior = new FieldDefinition
        {
            Title = "Sort Type",
            InternalName = "SortBehavior",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("423874f8-c300-4bfb-b7a1-42e2159e3b19"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Other Address Postal Code], ID [0557c3f8-60c4-4dfb-b5ba-bf3c4e4386b1] and Group: [Core Contact and Calendar Columns]'
        /// </summary>
        public static FieldDefinition OtherAddressPostalCode = new FieldDefinition
        {
            Title = "Other Address Postal Code",
            InternalName = "OtherAddressPostalCode",
            Description = "",
            Group = "Core Contact and Calendar Columns",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("0557c3f8-60c4-4dfb-b5ba-bf3c4e4386b1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Free/Busy], ID [393003f9-6ccb-4ea9-9623-704aa4748dec] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition FreeBusy = new FieldDefinition
        {
            Title = "Free/Busy",
            InternalName = "FreeBusy",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.FreeBusy,
            Id = new Guid("393003f9-6ccb-4ea9-9623-704aa4748dec"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Workspace], ID [08fc65f9-48eb-4e99-bd61-5946c439e691] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkspaceLink = new FieldDefinition
        {
            Title = "Workspace",
            InternalName = "WorkspaceLink",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.CrossProjectLink,
            Id = new Guid("08fc65f9-48eb-4e99-bd61-5946c439e691"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Picture Width], ID [7e68a0f9-af76-404c-9613-6f82bc6dc28c] and Group: [Core Document Columns]'
        /// </summary>
        public static FieldDefinition ImageWidth = new FieldDefinition
        {
            Title = "Picture Width",
            InternalName = "ImageWidth",
            Description = "",
            Group = "Core Document Columns",
            FieldType = BuiltInFieldTypes.Integer,
            Id = new Guid("7e68a0f9-af76-404c-9613-6f82bc6dc28c"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Taxonomy Catch All Column], ID [f3b0adf9-c1a2-4b02-920d-943fba4b3611] and Group: [Custom Columns]'
        /// </summary>
        public static FieldDefinition TaxCatchAll = new FieldDefinition
        {
            Title = "Taxonomy Catch All Column",
            InternalName = "TaxCatchAll",
            Description = "",
            Group = "Custom Columns",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("f3b0adf9-c1a2-4b02-920d-943fba4b3611"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Outcome], ID [18e1c6fa-ae37-4102-890a-cfb0974ef494] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition WorkflowOutcome = new FieldDefinition
        {
            Title = "Outcome",
            InternalName = "WorkflowOutcome",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("18e1c6fa-ae37-4102-890a-cfb0974ef494"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [E-Mail Sender], ID [4ce600fb-a927-4911-bfc1-11076b76b522] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition EmailSender = new FieldDefinition
        {
            Title = "E-Mail Sender",
            InternalName = "EmailSender",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("4ce600fb-a927-4911-bfc1-11076b76b522"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Title], ID [e6f528fb-2e22-483d-9c80-f2536acdc6de] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition SurveyTitle = new FieldDefinition
        {
            Title = "Title",
            InternalName = "SurveyTitle",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("e6f528fb-2e22-483d-9c80-f2536acdc6de"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User ratings], ID [434f51fb-ffd2-4a0e-a03b-ca3131ac67ba] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Ratings = new FieldDefinition
        {
            Title = "User ratings",
            InternalName = "Ratings",
            Description = "User ratings for the item",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Note,
            Id = new Guid("434f51fb-ffd2-4a0e-a03b-ca3131ac67ba"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Source Version (Converted Document)], ID [bc1a8efb-0f4c-49f8-a38f-7fe22af3d3e0] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ParentVersionString = new FieldDefinition
        {
            Title = "Source Version (Converted Document)",
            InternalName = "ParentVersionString",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("bc1a8efb-0f4c-49f8-a38f-7fe22af3d3e0"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Category], ID [dfffbbfb-0cc3-4ce7-8cb3-a2958fb726a1] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition XSLStyleCategory = new FieldDefinition
        {
            Title = "Category",
            InternalName = "XSLStyleCategory",
            Description = "The category or content types compatible with this style",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("dfffbbfb-0cc3-4ce7-8cb3-a2958fb726a1"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Reporter Lookup], ID [cd4df6fb-0da8-4ac9-b551-ed4fa6cd88fd] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition AbuseReportsReporterLookup = new FieldDefinition
        {
            Title = "Reporter Lookup",
            InternalName = "AbuseReportsReporterLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("cd4df6fb-0da8-4ac9-b551-ed4fa6cd88fd"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Related Items], ID [d2a04afc-9a05-48c8-a7fa-fa98f9496141] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RelatedItems = new FieldDefinition
        {
            Title = "Related Items",
            InternalName = "RelatedItems",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.RelatedItems,
            Id = new Guid("d2a04afc-9a05-48c8-a7fa-fa98f9496141"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [User Name], ID [211a8cfc-93b7-4173-9254-0bfe2d1643da] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition UserName = new FieldDefinition
        {
            Title = "User Name",
            InternalName = "UserName",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.User,
            Id = new Guid("211a8cfc-93b7-4173-9254-0bfe2d1643da"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [POS], ID [f3cdbcfd-f456-45f4-9000-b6f34bb95d84] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition IMEPos = new FieldDefinition
        {
            Title = "POS",
            InternalName = "IMEPos",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Choice,
            Id = new Guid("f3cdbcfd-f456-45f4-9000-b6f34bb95d84"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Leaving Early], ID [a2a86efe-c28e-4dde-ab56-0afa31664bbc] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition LeaveEarly = new FieldDefinition
        {
            Title = "Leaving Early",
            InternalName = "LeaveEarly",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Text,
            Id = new Guid("a2a86efe-c28e-4dde-ab56-0afa31664bbc"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Report Created By], ID [8caf7ffe-9d2c-406c-9743-7a252b5c8ae5] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ReportCreatedByDisplay = new FieldDefinition
        {
            Title = "Report Created By",
            InternalName = "ReportCreatedByDisplay",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Computed,
            Id = new Guid("8caf7ffe-9d2c-406c-9743-7a252b5c8ae5"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Shortest Thread-Index Id Lookup], ID [8ffccefe-998b-4896-a6df-32d566f69141] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition ShortestThreadIndexIdLookup = new FieldDefinition
        {
            Title = "Shortest Thread-Index Id Lookup",
            InternalName = "ShortestThreadIndexIdLookup",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("8ffccefe-998b-4896-a6df-32d566f69141"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Created], ID [998b5cff-4a35-47a7-92f3-3914aa6aa4a2] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition Created_x0020_Date = new FieldDefinition
        {
            Title = "Created",
            InternalName = "Created_x0020_Date",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.Lookup,
            Id = new Guid("998b5cff-4a35-47a7-92f3-3914aa6aa4a2"),
            Required = false
        };

        /// <summary>
        /// Corresponds to built-in field with Title [Recurrence ID], ID [dfcc8fff-7c4c-45d6-94ed-14ce0719efef] and Group: [_Hidden]'
        /// </summary>
        public static FieldDefinition RecurrenceID = new FieldDefinition
        {
            Title = "Recurrence ID",
            InternalName = "RecurrenceID",
            Description = "",
            Group = "_Hidden",
            FieldType = BuiltInFieldTypes.DateTime,
            Id = new Guid("dfcc8fff-7c4c-45d6-94ed-14ce0719efef"),
            Required = false
        };



        #endregion
    }
}
