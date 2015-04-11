using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using SPMeta2.Definitions.Base;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{

    [DataContract]
    public class FieldValue
    {
        public string FieldName { get; set; }
        public Guid? FieldId { get; set; }

        public object Value { get; set; }
    }

    /// <summary>
    /// Base definition for pages.
    /// </summary>
    /// 
    [Serializable]
    [DataContract]
    public abstract class PageDefinitionBase : DefinitionBase
    {
        #region constructors

        public PageDefinitionBase()
        {
            NeedOverride = true;
            DefaultValues = new List<FieldValue>();
        }

        #endregion

        #region properties

        [DataMember]
        public List<FieldValue> DefaultValues { get; set; }

        /// <summary>
        /// Title of the target page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// File name of the target page.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string FileName { get; set; }

        /// <summary>
        /// Should page be overwritten during provision.
        /// </summary>
        /// 
        [DataMember]

        public bool NeedOverride { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] FileName:[{1}]", Title, FileName);
        }

        #endregion
    }
}
