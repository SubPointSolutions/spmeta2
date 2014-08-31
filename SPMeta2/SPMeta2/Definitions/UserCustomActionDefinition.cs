using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint custom user action.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPUserCustomAction", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.UserCustomAction", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(SiteDefinition))]
    [DefaultParentHostAttribute(typeof(SiteDefinition))]

    [Serializable]

    public class UserCustomActionDefinition : DefinitionBase
    {
        #region constructors

        public UserCustomActionDefinition()
        {
            Rights = new Collection<string>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Name of the target custom user action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Title of the target custom user action.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the target user custom action.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Group of the target user custom action.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Location of the target custom user action.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// ScriptSrc of the target user custom action.
        /// </summary>
        public string ScriptSrc { get; set; }

        /// <summary>
        /// ScriptBlock of the target custom user action.
        /// </summary>
        public string ScriptBlock { get; set; }

        /// <summary>
        /// Sequence of the target custom user action.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// URL of the target custom user action.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Permissions of the target custom user action.
        /// </summary>
        public Collection<string> Rights { get; set; }

        /// <summary>
        /// Registration ID of the target custom user action.
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// Registration type of the target custom user action.
        /// 
        /// BuiltInRegistrationTypes class can be uses to utilize out of the box SharePoint registration types.
        /// </summary>
        public string RegistrationType { get; set; }

        #endregion
    }
}
