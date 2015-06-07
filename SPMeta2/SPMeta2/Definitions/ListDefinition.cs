using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint list.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPList", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.List", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ListDefinition : DefinitionBase
    {
        public ListDefinition()
        {
            Description = string.Empty;
            Hidden = false;
        }

        #region properties


        /// <summary>
        /// Title of the target list.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        public string Title { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? IrmEnabled { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? IrmExpire { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool? IrmReject { get; set; }

        [ExpectValidation]
        [DataMember]
        public string DraftVersionVisibility { get; set; }

        /// <summary>
        /// Description of the target list.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        /// <summary>
        /// URL of the target list.
        /// Don't use "list/my-list-name" as URL is calculated by TemplateType/TemplateName properties.
        /// Provision automatically adds '/lists/' if necessary.
        /// 
        /// Use ListDefinition.GetListUrl() methods to resolve particular list URL.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "List Url")]
        [DataMember]
        [IdentityKey]
        [Obsolete("Obsolete. Will be removed from the SPMeta2 API. Use CustomUrl property specifying URL web relative URL with/wihtout 'Lists' prefix as you need.")]
        public string Url { get; set; }

        /// <summary>
        /// Provided fll conteolober the URL.
        /// If provided, this value will be used withouy any changes or calculations.
        /// </summary>
        [ExpectValidation]
        [ExpectRequired(GroupName = "List Url")]
        [DataMember]
        [IdentityKey]
        public string CustomUrl { get; set; }

        /// <summary>
        /// Template type of the target list.
        /// BuiltInListTemplateTypeId class can be used to utilize out of the box SharePoint list types.
        /// 
        /// Skip TemplateType and use TemplateName property to create list based on custom template.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "List Template")]
        [DataMember]
        public int TemplateType { get; set; }

        /// <summary>
        /// Template name of the target list. 
        /// This property is used to allow list creation based on custom templates.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "List Template")]
        [DataMember]
        public string TemplateName { get; set; }

        /// <summary>
        /// Enables content type support on the target list.
        /// </summary>
        /// 
        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool ContentTypesEnabled { get; set; }

        /// <summary>
        /// Reserved for the future. Is not used.
        /// </summary>
        /// 
        [DataMember]
        public bool NeedBreakRoleInheritance { get; set; }

        /// <summary>
        /// Reserved for the future. Is not used.
        /// </summary>
        /// 
        [DataMember]
        public bool? NeedToCopyRoleAssignmets { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? EnableAttachments { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? EnableFolderCreation { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? EnableMinorVersions { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? EnableModeration { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? EnableVersioning { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? ForceCheckout { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]

        [DataMember]
        public bool? Hidden { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? NoCrawl { get; set; }

        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? OnQuickLaunch { get; set; }

        /// <summary>
        /// The maximum number of major versions allowed for an item in a document library that uses version control with major versions only.
        /// CSOM is not supported yet as M2 s build with SP2013 SP1+ assemblies.
        /// https://officespdev.uservoice.com/forums/224641-general/suggestions/6016131-majorversionlimit-majorwithminorversionslimit-pr
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public int? MajorVersionLimit { get; set; }

        /// <summary>
        /// The maximum number of major versions that are allowed for an item in a document library that uses version control with both major and minor versions.
        /// CSOM is not supported yet as M2 s build with SP2013 SP1+ assemblies.
        /// https://officespdev.uservoice.com/forums/224641-general/suggestions/6016131-majorversionlimit-majorwithminorversionslimit-pr
        /// </summary>
        [DataMember]
        [ExpectValidation]

        public int? MajorWithMinorVersionsLimit { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title: [{0}] Url: [{1}] TemplateType:[{2}] TemplateName:[{3}]",
                            new[] {
                                Title,
                                Url,
                                TemplateType.ToString(),
                                TemplateName                                
                            });
        }

        #endregion
    }


    // ListDefinitionSyntax.GetListUrl() should be OM-independent #477
    // https://github.com/SubPointSolutions/spmeta2/issues/477

    //public static class ListDefinitionExtensions
    //{
    //    static ListDefinitionExtensions()
    //    {
    //        InitKnownListTypes();
    //    }

    //    private static void InitKnownListTypes()
    //    {
    //        // GenericList
    //        KnownListTypes.Add(100);

    //        // Survey
    //        KnownListTypes.Add(102);

    //        // Links
    //        KnownListTypes.Add(103);

    //        // Announcements
    //        KnownListTypes.Add(104);

    //        // Contacts
    //        KnownListTypes.Add(105);

    //        // Events
    //        KnownListTypes.Add(106);

    //        // Tasks
    //        KnownListTypes.Add(107);

    //        // DiscussionBoard
    //        KnownListTypes.Add(108);

    //        // DiscussionBoard
    //        KnownListTypes.Add(108);

    //        // SolutionCatalog
    //        KnownListTypes.Add(121);

    //        // WorkflowHistory
    //        KnownListTypes.Add(140);

    //        // WorkflowHistory
    //        KnownListTypes.Add(140);

    //        // GanttTasks
    //        KnownListTypes.Add(150);

    //        // HelpLibrary
    //        KnownListTypes.Add(151);

    //        // TasksWithTimelineAndHierarchy
    //        KnownListTypes.Add(171);

    //        // MaintenanceLogs
    //        KnownListTypes.Add(175);

    //        // Meetings
    //        KnownListTypes.Add(200);

    //        // Agenda
    //        KnownListTypes.Add(201);

    //        // MeetingUser
    //        KnownListTypes.Add(202);

    //        // Decision
    //        KnownListTypes.Add(204);

    //        // MeetingObjective
    //        KnownListTypes.Add(207);

    //        // TextBox
    //        KnownListTypes.Add(210);

    //        // ThingsToBring
    //        KnownListTypes.Add(211);

    //        // ThingsToBring
    //        KnownListTypes.Add(211);

    //        // ExternalList
    //        KnownListTypes.Add(600);

    //        // IssueTracking
    //        KnownListTypes.Add(1100);

    //        // AdminTasks
    //        KnownListTypes.Add(1200);
    //    }

    //    public static List<int> KnownListTypes = new List<int>();

    //    public static string GetListUrl(this ListDefinition listDefinition)
    //    {
    //        // don't change CustomUrl - suppoed to be the same
    //        if (!string.IsNullOrEmpty(listDefinition.CustomUrl))
    //            return listDefinition.CustomUrl;

    //        // OOTB SharePoint sub fodlers
    //        if (listDefinition.Url.ToUpper().Contains("_CATALOGS"))
    //            return listDefinition.Url;

    //        if (listDefinition.Url.ToUpper().Contains("LISTS"))
    //            return listDefinition.Url;

    //        // calculate URL based on the list type
    //        if (KnownListTypes.Contains(listDefinition.TemplateType))
    //            return string.Format("Lists/{0}", listDefinition.Url);

    //        return listDefinition.Url;
    //    }
    //}
}
