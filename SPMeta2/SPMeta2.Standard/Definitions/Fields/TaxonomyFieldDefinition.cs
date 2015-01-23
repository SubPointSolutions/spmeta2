using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy taxonomy field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Taxonomy.TaxonomyField", "Microsoft.SharePoint.Taxonomy")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Taxonomy.TaxonomyField", "Microsoft.SharePoint.Client.Taxonomy")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class TaxonomyFieldDefinition : FieldDefinition
    {
        #region constructors

        public TaxonomyFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.TaxonomyFieldType;

            TermSetLCID = 1033;
            TermLCID = 1033;
        }

        #endregion

        #region properties

        [ExpectValidation]
        public bool IsMulti { get; set; }

        [ExpectValidation]
        public string SspName { get; set; }

        [ExpectValidation]
        public Guid? SspId { get; set; }

        [ExpectValidation]
        public bool? UseDefaultSiteCollectionTermStore { get; set; }

        [ExpectValidation]
        public string TermSetName { get; set; }

        [ExpectValidation]
        public Guid? TermSetId { get; set; }

        [ExpectValidation]
        public int TermSetLCID { get; set; }

        [ExpectValidation]
        public string TermName { get; set; }

        [ExpectValidation]
        public Guid? TermId { get; set; }

        [ExpectValidation]
        public int TermLCID { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TaxonomyFieldDefinition>(this, base.ToString())

                          .AddPropertyValue(p => p.IsMulti)

                          .AddPropertyValue(p => p.SspName)
                          .AddPropertyValue(p => p.SspId)
                          .AddPropertyValue(p => p.UseDefaultSiteCollectionTermStore)

                          .AddPropertyValue(p => p.TermSetName)
                          .AddPropertyValue(p => p.TermSetId)
                          .AddPropertyValue(p => p.TermSetLCID)

                          .AddPropertyValue(p => p.TermName)
                          .AddPropertyValue(p => p.TermId)
                          .AddPropertyValue(p => p.TermLCID)

                          .ToString();
        }

        #endregion
    }
}
