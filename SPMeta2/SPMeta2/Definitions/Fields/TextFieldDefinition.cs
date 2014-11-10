using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy text field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldText", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldText", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class TextFieldDefinition : FieldDefinition
    {
    }
}
