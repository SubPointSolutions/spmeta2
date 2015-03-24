using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy SharePoint HTML field.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Fields.SummaryLinkField", "Microsoft.SharePoint.Publishing")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class SummaryLinkFieldDefinition : FieldDefinition
    {
        #region constructors

        public SummaryLinkFieldDefinition()
        {
            FieldType = BuiltInPublishingFieldTypes.SummaryLinks;
        }

        #endregion

        #region properties

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SummaryLinkFieldDefinition>(this, base.ToString())

                          .ToString();
        }

        #endregion
    }
}
