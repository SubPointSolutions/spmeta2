using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy list view.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPView", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.View", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    [ExpectAddHostExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    // this not going to work due to IsDefault prop on view
    //[ExpectManyInstances]

    public class ListViewDefinition : DefinitionBase
    {
        #region constructors

        public ListViewDefinition()
        {
            Fields = new Collection<string>();
            IsPaged = true;
            RowLimit = 30;

            Url = string.Empty;
            Query = string.Empty;

            Type = BuiltInViewType.Html;

            TitleResource = new List<ValueForUICulture>();

            InlineEdit = null;
            TabularView = null;

            Types = new Collection<string>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target list view.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Title { get; set; }


        /// <summary>
        /// Corresponds to NameResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> TitleResource { get; set; }

        /// <summary>
        /// Allows to define URL of the target view.
        /// 
        /// If not empty, them LIstView will be created with "Url" value title and then renamed with "Title" values.
        /// It helps to create "english" urls in non-english locales.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string Url { get; set; }

        /// <summary>
        /// RowLimit of the target list view.
        /// </summary>

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public int RowLimit { get; set; }

        /// <summary>
        /// Corresponds to Scope property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdateAsViewScope]
        [DataMember]
        public string Scope { get; set; }

        /// <summary>
        /// CAML Query of the target list view.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdateAsCamlQuery]
        [DataMember]
        [ExpectNullable]

        [CamlPropertyCapability]
        public string Query { get; set; }

        /// <summary>
        /// IsPaged flag of the target list view.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string ViewData { get; set; }

        /// <summary>
        /// IsPaged flag of the target list view.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool IsPaged { get; set; }

        /// <summary>
        /// Gets or sets whether the list view should include bulk operation checkboxes if the current list view supports them.
        /// </summary>
        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? TabularView { get; set; }

        /// <summary>
        /// ISDefault flag of the target list view.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool IsDefault { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? MobileDefaultView { get; set; }
        

        /// <summary>
        /// Gets or sets whether the list view should include parent folder item.
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? IncludeRootFolder { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a string that specifies whether the view is in inline edit mode.
        /// </summary>
        [ExpectValidation]
        //[ExpectUpdate]
        [DataMember]
        public bool? InlineEdit { get; set; }

        /// <summary>
        /// Set of the internal field names of the target list view.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdateAsInternalFieldName]
        [DataMember]
        public Collection<string> Fields { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string JSLink { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public bool? DefaultViewForContentType { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ContentTypeName { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsIntRange(MinValue = 12, MaxValue = 20)]
        public int? ViewStyleId { get; set; }

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// If set, used insted of .Type property.
        /// Allows to define multiple types of view, such as 'ViewType.Calendar | ViewType.Recurrence'
        /// </summary>
        [ExpectValidation]
        //[ExpectRequired]
        [DataMember]

        public Collection<string> Types { get; set; }

        /// <summary>
        /// Gets or sets field references for one or more aggregate, or total, columns used in a view.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public string Aggregations { get; set; }

        /// <summary>
        /// Gets or sets a string that specifies whether aggregate, or total, columns are used in the view.
        /// A string that specifies "On" if an aggregate column is used in the view; otherwise, an empty string.
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public string AggregationsStatus { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                         .AddRawPropertyValue("Title", Title)
                         .AddRawPropertyValue("Url", Url)
                         .AddRawPropertyValue("IsDefault", IsDefault)
                         .AddRawPropertyValue("Query", Query)

                         .ToString();
        }

        #endregion
    }
}
