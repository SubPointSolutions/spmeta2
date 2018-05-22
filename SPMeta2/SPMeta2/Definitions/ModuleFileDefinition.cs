using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy module file.
    /// </summary>

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]
    [ExpectAddHostExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]

    public class ModuleFileDefinition : DefinitionBase
    {
        #region constructors

        public ModuleFileDefinition()
        {
            Content = new byte[0];
            Overwrite = true;

            DefaultValues = new List<FieldValue>();
            Values = new List<FieldValue>();
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

        [ExpectValidation]
        [DataMember]
        public List<FieldValue> Values { get; set; }

        /// <summary>
        /// Target file name,
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string FileName { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdate]
        public string Title { get; set; }

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
            return new ToStringResultRaw()
                          .AddRawPropertyValue("FileName", FileName)
                          .AddRawPropertyValue("Overwrite", Overwrite)

                          .ToString();
        }

        #endregion
    }
}
