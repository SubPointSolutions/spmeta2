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
    /// Allows to define and deploy SharePoint custom user action.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPUserCustomAction", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.UserCustomAction", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class UserCustomActionDefinition : DefinitionBase
    {
        #region constructors

        public UserCustomActionDefinition()
        {
            Rights = new Collection<string>();
            RegistrationType = BuiltInRegistrationTypes.None;

            TitleResource = new List<ValueForUICulture>();
            DescriptionResource = new List<ValueForUICulture>();
            CommandUIExtensionResource = new List<ValueForUICulture>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        /// <summary>
        /// Title of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        //[ExpectRequired]
        public string Title { get; set; }


        /// <summary>
        /// Corresponds to TitleResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> TitleResource { get; set; }

        /// <summary>
        /// Description of the target user custom action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        //[ExpectRequired]
        public string Description { get; set; }


        /// <summary>
        /// Corresponds to DescriptionResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> DescriptionResource { get; set; }

        /// <summary>
        /// Group of the target user custom action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string Group { get; set; }

        /// <summary>
        /// Location of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string Location { get; set; }

        /// <summary>
        /// ScriptSrc of the target user custom action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [NotAbsoluteUrlCapability]
        public string ScriptSrc { get; set; }

        /// <summary>
        /// ScriptBlock of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string ScriptBlock { get; set; }

        /// <summary>
        /// Sequence of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public int Sequence { get; set; }

        /// <summary>
        /// URL of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Permissions of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public Collection<string> Rights { get; set; }

        /// <summary>
        /// Registration ID of the target custom user action.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string RegistrationId { get; set; }

        /// <summary>
        /// Registration type of the target custom user action.
        /// 
        /// BuiltInRegistrationTypes class can be uses to utilize out of the box SharePoint registration types.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        public string RegistrationType { get; set; }

        /// <summary>
        /// Gets and sets XML that defines an extension to the ribbon.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public string CommandUIExtension { get; set; }

        /// <summary>
        /// Corresponds to CommandUIExtensionResource property
        /// </summary>
        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public List<ValueForUICulture> CommandUIExtensionResource { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Title", Title)
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Url", Url)
                          .AddRawPropertyValue("Location", Location)
                          .AddRawPropertyValue("RegistrationId", RegistrationId)
                         
                          .AddRawPropertyValue("Sequence", Sequence)

                          .AddRawPropertyValue("ScriptSrc", ScriptSrc)
                          .AddRawPropertyValue("ScriptBlock", ScriptBlock)
                          .ToString();
        }

        #endregion
    }
}
