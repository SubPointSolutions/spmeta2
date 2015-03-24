using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy SharePoint image field.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Publishing.Fields.ImageField", "Microsoft.SharePoint.Publishing")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ImageFieldDefinition : FieldDefinition
    {
        #region constructors

        public ImageFieldDefinition()
        {
            FieldType = BuiltInPublishingFieldTypes.Image;
        }

        #endregion

        #region properties

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ImageFieldDefinition>(this, base.ToString())

                          .ToString();
        }

        #endregion
    }
}
