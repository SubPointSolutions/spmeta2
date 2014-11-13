using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;

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
        public string EditFormat { get; set; }

        #endregion
    }
}
