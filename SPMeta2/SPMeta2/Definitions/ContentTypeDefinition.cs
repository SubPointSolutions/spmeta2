using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint content type.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPContentType", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ContentType", "Microsoft.SharePoint.Client")]

    [DefaultParentHostAttribute(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable]
    [ExpectAddHostExtensionMethod]
    [ExpectWithExtensionMethod]
    public class ContentTypeDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// ID of the target content type. 
        /// Final content type id is calculated based on ParentContentTypeId, IdNumberValue or Id properties with ContentTypeDefinitionSyntax.GetContentTypeId() method.
        /// </summary>
        /// 

        [ExpectValidation]
        public Guid Id { get; set; }

        /// <summary>
        /// IdNumberValue of the target content type. 
        /// Final content type id is calculated based on ParentContentTypeId, IdNumberValue or Id properties with ContentTypeDefinitionSyntax.GetContentTypeId() method.
        /// </summary>
        /// 

        [ExpectValidation]
        public string IdNumberValue { get; set; }

        /// <summary>
        /// Name of the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        public string Name { get; set; }

        /// <summary>
        /// Description if the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectUpdate]
        public string Description { get; set; }

        /// <summary>
        /// Group of the target content type.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectUpdate]
        public string Group { get; set; }

        /// <summary>
        /// Parent content type id. BuiltInContentTypeId class could be used to utilize out of the box content type ids.
        /// </summary>
        /// 
        public string ParentContentTypeId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Name:[{0}] Id:[{1}] IdNumberValue:[{2}]", Name, Id, IdNumberValue);
        }

        #endregion
    }
}
