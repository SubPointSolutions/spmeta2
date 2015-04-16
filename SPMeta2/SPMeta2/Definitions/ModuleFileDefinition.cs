using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy module file.
    /// </summary>

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ModuleFileDefinition : DefinitionBase
    {
        #region constructors

        public ModuleFileDefinition()
        {
            Content = new byte[0];
            Overwrite = true;

            DefaultValues = new List<FieldValue>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeName { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<FieldValue> DefaultValues { get; set; }

        /// <summary>
        /// Target file name,
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string FileName { get; set; }

        /// <summary>
        /// Target file content.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectUpdate]
        [ExpectRequired]
        [DataMember]
        public byte[] Content { get; set; }

        /// <summary>
        /// Overwrite flag
        /// </summary>
        /// 
        [DataMember]
        public bool Overwrite { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ModuleFileDefinition>(this)
                          .AddPropertyValue(p => p.FileName)
                          .AddPropertyValue(p => p.Overwrite)

                          .ToString();
        }

        #endregion
    }
}
