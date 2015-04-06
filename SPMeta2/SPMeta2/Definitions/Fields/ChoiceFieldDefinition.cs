using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy choice field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldChoice", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldChoice", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class ChoiceFieldDefinition : MultiChoiceFieldDefinition
    {
        #region constructors

        public ChoiceFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Choice;
            EditFormat = BuiltInChoiceFormatType.Dropdown;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        public string EditFormat { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ChoiceFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.EditFormat)
                          .ToString();
        }

        #endregion
    }


}
