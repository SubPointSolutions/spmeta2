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
        public bool Hidden { get; set; }

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

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ListViewDefinition>(this)
                         .AddPropertyValue(p => p.Title)
                         .AddPropertyValue(p => p.Url)
                         .AddPropertyValue(p => p.IsDefault)
                         .AddPropertyValue(p => p.Query)

                         .ToString();
        }

        #endregion
    }
}
